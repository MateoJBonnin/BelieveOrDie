using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LightingHandler : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private float explosionForce;

    public float shakeDuration = .2f;
    private void Start()
    {
        this.CreateExplosionZone();
    }

    public void CreateExplosionZone()
    {
        Collider[] sphereCastColliders = Physics.OverlapSphere(this.transform.position, this.radius,1<< LayerMask.NameToLayer("Villager"));
        Camera.main.GetComponent<CameraController>().DoShake(shakeDuration,Random.insideUnitSphere * UnityEngine.Random.Range(.3f,.6f),20,180);

        foreach (Collider collider in sphereCastColliders)
        {
            Villager villager = collider.GetComponentInParent<Villager>();

            if (villager.isDead)
            {
                continue;
            }

            villager.Die();
        }
    }
}
