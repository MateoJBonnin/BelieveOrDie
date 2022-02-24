using System.Collections;
using System.Collections.Generic;
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
    private Vector3 currentDiffDragPosition;
    private Vector3 startDragPosition;
    private Vector3 maxDragPosition;
    Vector3 lastMousePosition;
    private float currentScrollZoom;

    private void Start()
    {
        startPosition = transform.position;
        this.startDragPosition = this.transform.position;
        this.maxDragPosition = (this.transform.forward * this.zoomInDistance) + this.startDragPosition;
        this.currentDragPosition = this.startDragPosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            targetMovementOffset -= new Vector3(Input.mousePosition.x - lastMousePosition.x, 0, Input.mousePosition.y - lastMousePosition.y) * dragSensitivity;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            lastMousePosition = Input.mousePosition;
            this.targetMovementOffset += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            this.currentScrollZoom += Input.mouseScrollDelta.y * this.scrollSensitivity * Time.deltaTime;
            this.currentScrollZoom = Mathf.Clamp01(this.currentScrollZoom);

            if ((Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width * 0.1f) && targetMovementOffset.x >= startPosition.x - xSize)
            {
                this.targetMovementOffset.x -= 1;
            }
            else if ((Input.mousePosition.x > Screen.width - Screen.width * 0.1f && Input.mousePosition.x < Screen.width) && targetMovementOffset.x < startPosition.x + xSize)
            {
                this.targetMovementOffset.x += 1;
            }

            if ((Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height * 0.1f) && targetMovementOffset.z >= startPosition.x - zSize)
            {
                this.targetMovementOffset.z -= 1;
            }
            else if ((Input.mousePosition.y > Screen.height - Screen.height * 0.1f && Input.mousePosition.y < Screen.height) && targetMovementOffset.z < startPosition.x + zSize)
            {
                this.targetMovementOffset.z += 1;
            }
        }

        this.currentTargetDragPosition = Vector3.Lerp(this.startDragPosition, this.maxDragPosition, this.currentScrollZoom);
        this.currentDragPosition = Vector3.Lerp(this.currentDragPosition, this.currentTargetDragPosition, Time.deltaTime * 20f);
        this.currentMovementOffset = Vector3.Lerp(this.currentMovementOffset, this.targetMovementOffset * sensitivity, Time.deltaTime * 15f);
        Vector3 movementDiff = (currentDragPosition + this.currentMovementOffset) - this.transform.position;
        transform.position = Vector3.Lerp(transform.position, transform.position + movementDiff, Time.deltaTime * 5f);

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
}
