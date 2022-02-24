using UnityEngine;

public class LightingBaseParticleHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem baseParticleSystem;

    public void ReposForward(Vector3 newForward)
    {
        ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[this.baseParticleSystem.particleCount];
        int c = this.baseParticleSystem.GetParticles(particleArray);

        if (c <= 0)
        {
            return;
        }

        ParticleSystem.Particle guideParticle = particleArray[0];
        guideParticle.rotation3D = newForward;

        this.baseParticleSystem.SetParticles(new ParticleSystem.Particle[1] { guideParticle });
    }
}
