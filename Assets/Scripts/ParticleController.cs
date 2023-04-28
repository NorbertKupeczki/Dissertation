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

    /// <summary>
    /// Sets the amount of particles to be played by the effect, then plays the particle effect.
    /// </summary>
    /// <param name="particleAmount"></param>
    private void SetParticlesAmountAndPlay(ParticleSystem.MinMaxCurve particleAmount)
    {
        _emissionModule.rateOverTime = particleAmount;
        _particleSystem.Play();
    }

    /// <summary>
    /// Starts the particle effect with low amount of particles.
    /// </summary>
    public void SpawnParticlesLowAmount()
    {
        SetParticlesAmountAndPlay(ParticleData.LOW_PARTICLE_AMOUNT);
    }

    /// <summary>
    /// Starts the particle effect with medium amount of particles.
    /// </summary>
    public void SpawnParticlesMediumAmount()
    {
        SetParticlesAmountAndPlay(ParticleData.MEDIUM_PARTICLE_AMOUNT);
    }

    /// <summary>
    /// Starts the particle effect with high amount of particles.
    /// </summary>
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
