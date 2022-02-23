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

    private void Start()
    {
        this.godHand.OnTap += this.OnGodTapHandler;
    }

    private void OnGodTapHandler(Vector3 position)
    {
        this.SummonLighting(position);
    }

    private void SummonLighting(Vector3 position)
    {
        GodLighting newLightingParticle = Random.Range(0, 1f) > 0.5f ? Instantiate(this.godLightingPrefab) : Instantiate(this.godLightingPrefabIter2);
        newLightingParticle.transform.position = new Vector3(position.x, this.rayHeight, position.z);
        newLightingParticle.Trigger(position);
    }
}
