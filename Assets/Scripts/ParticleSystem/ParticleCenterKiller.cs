using UnityEngine;

public class ParticleCenterKiller : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    public float killDistance = 0.1f; // 중심에 얼마나 가까워지면 죽일지

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void Update()
    {
        int aliveParticles = ps.GetParticles(particles);

        Vector3 center = Vector3.zero; // 월드 중앙 (0,0,0)

        for (int i = 0; i < aliveParticles; i++)
        {
            float distance = Vector3.Distance(particles[i].position, center);
            if (distance < killDistance)
            {
                particles[i].remainingLifetime = 0f;
            }
        }

        ps.SetParticles(particles, aliveParticles);
    }
}