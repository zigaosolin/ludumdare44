using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpMode
{
    Normal,
    WaitingForJumpSequence,
    InJump
}

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private ParticleSystem normalParticles;
    [SerializeField] private ParticleSystem inJumpParticles;
    [SerializeField] private ParticleSystem waitingForJumpParticles;

    [SerializeField] private ParticleSystem damageParticle;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSfx;

    private int dealtDamageFramesOpen = 5;

    public void SetJumpMode(JumpMode jumpMode)
    {
        switch(jumpMode)
        {
            case JumpMode.InJump:
                spriteRenderer.color = Color.blue;
                SetEmission(inJumpParticles);
                break;
            case JumpMode.Normal:
                spriteRenderer.color = Color.white;
                SetEmission(normalParticles);
                break;
            case JumpMode.WaitingForJumpSequence:
                spriteRenderer.color = Color.yellow;
                SetEmission(waitingForJumpParticles);
                break;
        }
    }

    public void TakeDamage(bool enabled)
    {
        dealtDamageFramesOpen = enabled ? 5 : dealtDamageFramesOpen;

        var emitter = damageParticle.emission;
        emitter.enabled = dealtDamageFramesOpen > 0;

        dealtDamageFramesOpen--;
    }

    public void DamageEvent()
    {
        audioSource.PlayOneShot(damageSfx, Random.Range(0.8f, 1.3f));
    }

    private void SetEmission(ParticleSystem which)
    {
        var emissionNormal = normalParticles.emission;
        emissionNormal.enabled = which == normalParticles;

        var emissionInJump = inJumpParticles.emission;
        emissionInJump.enabled = which == inJumpParticles;

        var emissionWait = waitingForJumpParticles.emission;
        emissionWait.enabled = which == waitingForJumpParticles;
    }
}
