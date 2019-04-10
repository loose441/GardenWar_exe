using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static SEManager singleton { get; private set; }

    [Header("Sound Clip")]
    [Space]
    [SerializeField]
    private AudioClip attackSE = null;
    [SerializeField]
    private AudioClip hitSE = null;
    [SerializeField]
    private AudioClip healSE = null;
    [SerializeField]
    private AudioClip fireSE = null;
    [SerializeField]
    private AudioClip moveSE = null;
    [SerializeField]
    private AudioClip powerUpSE = null;
    [SerializeField]
    private AudioClip deathSE = null;
    [SerializeField]
    private AudioClip unitClick = null;
    [SerializeField]
    private AudioClip phaseTransSE = null;
    [SerializeField]
    private AudioClip loseSE = null;
    [SerializeField]
    private AudioClip winSE = null;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        singleton = this;
        
    }

    public void PlayAttackSE()
    {
        audioSource.PlayOneShot(attackSE);
    }

    public void PlayHitSE()
    {
        audioSource.PlayOneShot(hitSE);
    }

    public void PlayHealSE()
    {
        audioSource.PlayOneShot(healSE);
    }

    public void PlayFireSE()
    {
        audioSource.PlayOneShot(fireSE);
    }

    public void PlayMoveSE()
    {
        audioSource.PlayOneShot(moveSE);
    }

    public void PlayPowerUpSE()
    {
        audioSource.PlayOneShot(powerUpSE);
    }

    public void PlayDeathSE()
    {
        audioSource.PlayOneShot(deathSE);
    }

    public void PlayUnitClickSE()
    {
        audioSource.PlayOneShot(unitClick);
    }

    public void PlayPhaseTransSE()
    {
        audioSource.PlayOneShot(phaseTransSE);

    }

    public void PlayWinSE()
    {
        audioSource.PlayOneShot(winSE);
    }

    public void PlayLoseSE()
    {
        audioSource.PlayOneShot(loseSE);
    }
}
