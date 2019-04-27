using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DamageAreaData
{
    public float NextDamageTime;
    public DamageArea Area;
}

public class Player : MonoBehaviour
{
    const float DamageInterval = 0.2f;

    private float HitPoints;
    [SerializeField]
    private float MaxHitPoints;

    private List<DamageAreaData> enteredAreas = new List<DamageAreaData>();

    private void Awake()
    {
        HitPoints = MaxHitPoints;
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
        for(int i = 0; i < enteredAreas.Count; ++i)
        {
            var areaData = enteredAreas[i];
            areaData.NextDamageTime -= Time.deltaTime;
            if(areaData.NextDamageTime < 0)
            {
                DealDamage(areaData.Area.DamageAmount);
                areaData.NextDamageTime = DamageInterval;
            }
        }
    }

    private void DealDamage(float amount)
    {
        Trace.Info(TraceCategory.Damage, $"Deal damage {amount}");

        HitPoints -= amount;
        if(HitPoints < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Trace.Info(TraceCategory.Player, "Died");
        HitPoints = MaxHitPoints;
    }
}
