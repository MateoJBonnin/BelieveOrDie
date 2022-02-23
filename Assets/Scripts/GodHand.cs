using System;
using UnityEngine;

public class GodHand : MonoBehaviour
{
    public event Action<Vector3> OnTap;

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
        }
    }

    private void HandTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, this.groundLayerMask))
            {
                GodTapHandler tapHandler = hit.collider.gameObject.transform.root.GetComponentInChildren<GodTapHandler>();
                if (tapHandler != null)
                {
                    tapHandler.ProcessDeath();
                }

                this.OnTap?.Invoke(hit.point);
            }
        }
    }

    private void Update()
    {
        this.HandTap();
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
