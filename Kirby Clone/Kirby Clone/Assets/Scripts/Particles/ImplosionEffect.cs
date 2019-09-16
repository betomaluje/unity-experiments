using System.Collections;
using UnityEngine;

public class ImplosionEffect : MonoBehaviour
{    
    public float reverseSpeed = -1f;
    public float timeToReverse = 0.25f;

    [HideInInspector] public ImplosionListener callback;

    private ParticleSystem implosionParticles;
    private ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        implosionParticles = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < implosionParticles.main.maxParticles)
            particles = new ParticleSystem.Particle[implosionParticles.main.maxParticles];

        StartCoroutine(ReverseSpeed());
    }

    private IEnumerator ReverseSpeed()
    {
        if (callback != null)
        {
            callback.onStartImplosion();
        }
            
        yield return new WaitForSeconds(timeToReverse);

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = implosionParticles.GetParticles(particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].velocity *= reverseSpeed;
        }

        // Apply the particle changes to the Particle System
        implosionParticles.SetParticles(particles, numParticlesAlive);

        yield return new WaitForSeconds(0.1f);
        var main = implosionParticles.main;
        main.simulationSpeed = 0;
        Destroy(gameObject);

        if (callback != null)
        {
            callback.onFinishImplosion();
        }        
    }
}
