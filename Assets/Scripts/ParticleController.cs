using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralData;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticleSystem.EmissionModule _emissionModule;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SetParticlesAmountAndPlay(ParticleSystem.MinMaxCurve particleAmount)
    {
        _emissionModule.rateOverTime = particleAmount;
        _particleSystem.Play();
    }

    public void SpawnParticlesLowAmount()
    {
        SetParticlesAmountAndPlay(ParticleData.LOW_PARTICLE_AMOUNT);
    }

    public void SpawnParticlesMediumAmount()
    {
        SetParticlesAmountAndPlay(ParticleData.MEDIUM_PARTICLE_AMOUNT);
    }

    public void SpawnParticlesHighAmount()
    {
        SetParticlesAmountAndPlay(ParticleData.HIGH_PARTICLE_AMOUNT);
    }

#if UNITY_EDITOR
    private void HandleKeyboardInput() // ONLY FOR TESTING
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpawnParticlesLowAmount();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SpawnParticlesMediumAmount();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpawnParticlesHighAmount();
        }
    }
#endif
}
