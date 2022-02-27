using UnityEngine;

public class LightingTap : MonoBehaviour
{
    [SerializeField]
    private GodLighting godLightingPrefabIter2;
    [SerializeField]
    private GodLighting godLightingPrefab;
    [SerializeField]
    private float rayHeight;
    [SerializeField]
    private GodHand godHand;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private AudioSource thunderSfx;

    private float nextAvailableTime;

    private void Start()
    {
        this.godHand.OnTap += this.OnGodTapHandler;
    }

    private void OnGodTapHandler(Vector3 position, Vector3 normal)
    {
        if (this.nextAvailableTime < Time.time)
        {
            this.SummonLighting(position, normal);
            this.nextAvailableTime = Time.time + this.cooldown;

            thunderSfx.pitch = Random.Range(0.5f, 1.5f);
            thunderSfx.Play();
        }
    }

    private void SummonLighting(Vector3 position, Vector3 normal)
    {
        GodLighting newLightingParticle = Random.Range(0, 1f) > 0.5f ? Instantiate(this.godLightingPrefab) : Instantiate(this.godLightingPrefabIter2);
        newLightingParticle.transform.position = new Vector3(position.x, this.rayHeight, position.z);
        newLightingParticle.Trigger(position, normal);
    }
}
