using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xSize;
    public float zSize;
    public float ySize;

    public float sensitivity;
    public float scrollSensitivity;
    public float dragSensitivity;
    public float zoomInDistance;

    private Vector3 startPosition;
    private Vector3 targetMovementOffset;
    private Vector3 currentMovementOffset;
    private Vector3 currentTargetDragPosition;
    private Vector3 currentDragPosition;
    private Vector3 startDragPosition;
    private Vector3 maxDragPosition;
    Vector3 lastMousePosition;
    private float currentScrollZoom;

    public bool canZoom = true;
    public bool canDrag = true;
    public bool canMove = true;

    public static bool CanShowMessages;

    public float showMessagesYDistance = 25;
    
    public float movementAccumulated;
    public float dragAccumulated;
    
    public Action<Vector3> OnMove;
    public Action<Vector3> OnDrag;
    public Action<float>OnZoom;

    public float ZoomPercent
    {
        get
        {
            return this.currentTargetDragPosition.magnitude / this.maxDragPosition.magnitude;
        }
    }

    private void Start()
    {
        startPosition = transform.position;
        this.startDragPosition = this.transform.position;
        this.maxDragPosition = (this.transform.forward * this.zoomInDistance) + this.startDragPosition;
        this.currentDragPosition = this.startDragPosition;
    }

    private void Update()
    {
        CanShowMessages = transform.position.y < showMessagesYDistance;
        
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (canDrag)
            {
                Vector3 direction = new Vector3(Input.mousePosition.x - lastMousePosition.x, 0, Input.mousePosition.y - lastMousePosition.y);
                targetMovementOffset -= new Vector3(Input.mousePosition.x - lastMousePosition.x, 0, Input.mousePosition.y - lastMousePosition.y) * dragSensitivity;
                lastMousePosition = Input.mousePosition;
                dragAccumulated += direction.sqrMagnitude;
            }
        }
        else
        {
            lastMousePosition = Input.mousePosition;
            if (canMove)
            {
                Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                this.targetMovementOffset += direction;
                movementAccumulated += direction.sqrMagnitude;
            }
            if (canZoom)
            {
                this.currentScrollZoom += Input.mouseScrollDelta.y * this.scrollSensitivity * Time.deltaTime;
                this.currentScrollZoom = Mathf.Clamp01(this.currentScrollZoom);    
            }
        }

        this.currentTargetDragPosition = Vector3.Lerp(this.startDragPosition, this.maxDragPosition, this.currentScrollZoom);
        this.currentDragPosition = Vector3.Lerp(this.currentDragPosition, this.currentTargetDragPosition, Time.deltaTime * 20f);
        this.currentMovementOffset = Vector3.Lerp(this.currentMovementOffset, this.targetMovementOffset * sensitivity, Time.deltaTime * 15f);
        Vector3 movementDiff = (currentDragPosition + this.currentMovementOffset) - this.transform.position;
        transform.position = Vector3.Lerp(transform.position, transform.position + movementDiff, Time.deltaTime * 5f);

        OnMove?.Invoke(movementDiff);

        if (transform.position.x >= startPosition.x + xSize)
        {
            transform.position = new Vector3(startPosition.x + xSize, transform.position.y, transform.position.z);
            targetMovementOffset.x = Mathf.Lerp(targetMovementOffset.x, 0, Time.deltaTime * 1);
        }
        else if (transform.position.x < startPosition.x - xSize)
        {
            transform.position = new Vector3(startPosition.x - xSize, transform.position.y, transform.position.z);
            targetMovementOffset.x = Mathf.Lerp(targetMovementOffset.x, 0, Time.deltaTime * 1);
        }

        if (transform.position.z >= startPosition.z + zSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + zSize);
            targetMovementOffset.z = Mathf.Lerp(targetMovementOffset.z, 0, Time.deltaTime * 1);
        }
        else if (transform.position.z < startPosition.z - zSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z - zSize);
            targetMovementOffset.z = Mathf.Lerp(targetMovementOffset.z, 0, Time.deltaTime * 1);
        }

        if (transform.position.y >= startPosition.y + ySize)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y + ySize, transform.position.z);
        }
        else if (transform.position.y < startPosition.y - ySize)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y - ySize, transform.position.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(xSize, ySize, zSize));
    }
    
    
    public void SetMovementActivate(bool active)
    {
        canMove = active;
    }
        
    public void SetDragActivate(bool active)
    {
        canDrag = active;
    }

    public void SetZoomActivate(bool active)
    {
        canZoom = active;
    }

    
}
