using UnityEngine;

public class GodHand : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayerMask;

    private Vector3 pointInGround;

    private void PointTowardsCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, this.groundLayerMask))
        {
            this.pointInGround = hit.point;
            this.transform.forward = this.pointInGround - this.transform.position;
            //Debug.LogError("test");
        }
    }

    private void Update()
    {
        this.PointTowardsCursor();
    }

    private void OnDrawGizmos()
    {
        if (this.pointInGround != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.pointInGround, 1f);
        }
    }
}
