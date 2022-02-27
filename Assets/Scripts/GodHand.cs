using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GodHand : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnTap;

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private Animator anim;

    private Vector3 pointInGround;

    public bool InputBlock;

    private Plane plane;

    private void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void PointTowardsCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out float enter))
        {
            this.transform.forward = ray.GetPoint(enter) - this.transform.position;
        }
    }

    private void HandTap()
    {
        if (!InputBlock && Input.GetMouseButtonDown(0))
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

                this.OnTap?.Invoke(hit.point, hit.normal);
                anim.SetTrigger("Shoot");
                anim.SetInteger("ShootIndex", Random.Range(0, 2));
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
