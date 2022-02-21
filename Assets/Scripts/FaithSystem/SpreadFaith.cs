using UnityEngine;

public class SpreadFaith : MonoBehaviour
{
    public float faithPerSecond;

    public void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent(out Faith faith))
        {
            faith.Add(faithPerSecond * Time.deltaTime);
        }
    }
}
