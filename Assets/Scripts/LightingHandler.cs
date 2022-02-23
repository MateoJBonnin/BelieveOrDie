using Unity.VisualScripting;
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
        Collider[] sphereCastColliders = Physics.OverlapSphere(this.transform.position, this.radius);
        foreach (Collider collider in sphereCastColliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Villager"))
            {
                Villager villager = collider.GetComponentInParent<Villager>();
                Rigidbody villRb = collider.attachedRigidbody;
                if (villRb == null)
                {
                    villRb = villager.AddComponent<Rigidbody>();
                }

                villager.Die();
                villRb.AddExplosionForce(this.explosionForce, this.transform.position, this.radius, 2f);
                //TODO: Villager should die here FIX THIS
            }
        }
    }
}
