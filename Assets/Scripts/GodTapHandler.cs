using UnityEngine;

public class GodTapHandler : MonoBehaviour
{
    [SerializeField]
    private Villager villager;

    private Rigidbody rb;
    private bool hasBeenTapped;

    public void ProcessDeath()
    {
        if (this.hasBeenTapped)
        {
            return;
        }

        if (this.rb == null)
        {
            this.rb = this.transform.root.GetComponentInChildren<Rigidbody>();
            if (this.rb == null)
            {
                this.rb = this.gameObject.AddComponent<Rigidbody>();
            }
        }

        this.hasBeenTapped = true;
        this.PlaceholderRandomDeath();
    }

    private void PlaceholderRandomDeath()
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        Vector3 force = new Vector3(randomCircle.x, 0, randomCircle.y) * 15f;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        force.y = Random.Range(0.1f, 1f) * 5f;
        this.rb.AddForce(force, ForceMode.VelocityChange);
    }
}
