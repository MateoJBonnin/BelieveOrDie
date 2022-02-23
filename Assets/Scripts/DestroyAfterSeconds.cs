using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private float secondsToBeDestroyed;

    private void Awake()
    {
        Destroy(this.gameObject, this.secondsToBeDestroyed);
    }
}
