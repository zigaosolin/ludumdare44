using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class DamageAreaData
{
    public float NextDamageTime;
    public DamageArea Area;
}

public class Player : MonoBehaviour
{
    const float DamageInterval = 0.2f;
    private int hitPoints;

    public int HitPoints
    {
        get => hitPoints;
        set
        {
            hitPoints = Mathf.Min(100, value);
            PlayerUi.SetHitPoint(hitPoints, 100);
        }
    }
    [SerializeField] private PlayerUi PlayerUi;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PointerMovement pointerMovement;
    [SerializeField] private AnimationCurve jumpCurve;

    private bool wasPreviousInJump = false;
    private Vector2 jumpLandLocation;
    private Vector2 jumpStartLocation;
    private float jumpTimeNormalized;
    private List<DamageAreaData> enteredAreas = new List<DamageAreaData>();


    // Properties that are leveled up
    public float BaseJumpDuration = 0.3f;
    public float BaseInterpolateSpeed = 3.0f;

    public float JumpDuration { get; set; }
    public float InterpolateSpeed { get; set; }

    private void Awake()
    {
        HitPoints = 70;

        JumpDuration = BaseJumpDuration;
        InterpolateSpeed = BaseInterpolateSpeed;
    }

    public void EnterDamageZone(DamageArea area)
    {
        Trace.Info(TraceCategory.Damage, "Entered damage zone");

        enteredAreas.Add(new DamageAreaData()
        {
            NextDamageTime = 0,
            Area = area
        });
    }

    public void ExitDamageZone(DamageArea area)
    {
        Trace.Info(TraceCategory.Damage, "Exit damage zone");

        enteredAreas.RemoveAll(x => x.Area == area);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateDamageAreas();
    }

    private void UpdatePosition()
    {
        if (jumpTimeNormalized > 0.0f)
        {
            jumpTimeNormalized -= Time.deltaTime / JumpDuration;
            if (jumpTimeNormalized < 0)
                jumpTimeNormalized = 0;
            transform.position = Vector2.Lerp(jumpStartLocation, jumpLandLocation,
                jumpCurve.Evaluate(1 - jumpTimeNormalized));

            playerView.SetJumpMode(JumpMode.InJump);
            return;
        }

        var (targetPosition, isInJumpMovement) = pointerMovement.GetTarget();

        playerView.SetJumpMode(isInJumpMovement ? JumpMode.WaitingForJumpSequence : JumpMode.Normal);

        if (wasPreviousInJump && !isInJumpMovement)
        {
            SetupJump(targetPosition);
        }
        else if (!isInJumpMovement)
        {
            var currentPosition = transform.position;
            transform.position = Vector2.Lerp(currentPosition, targetPosition,
                Time.deltaTime * InterpolateSpeed);
        }
        else
        {
            // No transform, wait at location
        }

        wasPreviousInJump = isInJumpMovement;
    }

    private void SetupJump(Vector2 landLocation)
    {
        jumpLandLocation = landLocation;
        jumpStartLocation = transform.position;
        jumpTimeNormalized = 1.0f;
    }

    private void UpdateDamageAreas()
    {
        // No damage in jump
        if (jumpTimeNormalized > 0.0f)
            return;

        bool dealtDamage = false;
        for (int i = 0; i < enteredAreas.Count; ++i)
        {
            var areaData = enteredAreas[i];
            areaData.NextDamageTime -= Time.deltaTime;
            if (areaData.NextDamageTime < 0)
            {
                if (!dealtDamage)
                {
                    DealDamage(areaData.Area.DamageAmount);
                    dealtDamage = true;
                }
                areaData.NextDamageTime = DamageInterval;
            }
        }
    }

    private void DealDamage(float amount)
    {
        Trace.Info(TraceCategory.Damage, $"Deal damage {amount}");

        HitPoints -= (int)amount;
        if (HitPoints < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Trace.Info(TraceCategory.Player, "Died");
        SceneManager.LoadScene(0);
    }
}
