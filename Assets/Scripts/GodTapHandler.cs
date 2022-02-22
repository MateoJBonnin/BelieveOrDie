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

        this.rb = this.gameObject.AddComponent<Rigidbody>();
        this.hasBeenTapped = true;
        this.PlaceholderRandomDeath();
        this.villager.OnDieHandler();
    }

    private void PlaceholderRandomDeath()
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        Vector3 force = new Vector3(randomCircle.x, 0, randomCircle.y) * 15f;
        force.y = Random.Range(0.1f, 1f) * 5f;
        this.rb.AddForce(force, ForceMode.VelocityChange);
    }
}
