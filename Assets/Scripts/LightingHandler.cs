using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LightingHandler : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private float explosionForce;

    private void Start()
    {
        this.CreateExplosionZone();
    }

    public void CreateExplosionZone()
    {
        Collider[] sphereCastColliders = Physics.OverlapSphere(this.transform.position, this.radius,1<< LayerMask.NameToLayer("Villager"));
        
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
