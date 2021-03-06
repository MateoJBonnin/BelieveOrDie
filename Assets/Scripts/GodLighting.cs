using System;
using UnityEngine;

public class GodLighting : MonoBehaviour
{
    public event Action OnHitGround;

    [SerializeField]
    private LightingBaseParticleHandler lightingBaseParticlePrefab;
    [SerializeField]
    private ParticleSystem lightingMainParticle;
    [SerializeField]
    private LightingHandler lightingHandlerPrefab;
    [SerializeField]
    private float groundDistance;

    private LightingBaseParticleHandler baseLightingPS;
    private Vector3 targetPosition;
    private Vector3 targetNormal;
    private bool stop;
    private bool ended;

    public void Trigger(Vector3 position, Vector3 normal)
    {
        this.stop = false;
        this.targetPosition = position;
        this.targetNormal = normal;
        this.lightingMainParticle.Play();
    }

    private void Update()
    {
        this.CheckForCloseToGround();
    }

    private void CheckForCloseToGround()
    {
        if (this.lightingMainParticle == null)
        {
            return;
        }

        if (this.ended)
        {
            return;
        }

        ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[this.lightingMainParticle.particleCount];
        int c = this.lightingMainParticle.GetParticles(particleArray);

        ParticleSystem.NoiseModule noiseModule = this.lightingMainParticle.noise;
        ParticleSystem.VelocityOverLifetimeModule velocityModule = this.lightingMainParticle.velocityOverLifetime;

        if (c <= 0)
        {
            return;
        }

        ParticleSystem.Particle guideParticle = particleArray[0];
        if (guideParticle.position.y <= this.groundDistance)
        {
            if (!this.stop)
            {
                this.stop = true;
                noiseModule.enabled = false;
                velocityModule.enabled = false;
            }
            else
            {
                guideParticle.position = Vector3.Lerp(guideParticle.position, this.targetPosition, Time.deltaTime * 25f);

                if (guideParticle.position.y <= this.targetPosition.y + 0.1f)
                {
                    this.lightingMainParticle.Stop();
                    this.baseLightingPS = Instantiate(this.lightingBaseParticlePrefab);
                    this.baseLightingPS.transform.position = this.targetPosition;
                    this.baseLightingPS.transform.forward = -this.targetNormal;
                    LightingHandler lightingHandler = Instantiate(this.lightingHandlerPrefab);
                    lightingHandler.transform.position = this.targetPosition;
                    this.OnHitGround?.Invoke();
                    this.ended = true;
                }
            }
        }

        this.lightingMainParticle.SetParticles(new ParticleSystem.Particle[1] { guideParticle });
    }
}
