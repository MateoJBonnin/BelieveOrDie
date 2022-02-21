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

    private Vector3 startPosition;
    Vector3 lastMousePosition;


    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 movement = new Vector3(Input.mousePosition.x - lastMousePosition.x, 0, Input.mousePosition.y - lastMousePosition.y);

            transform.position += -movement * Time.deltaTime * dragSensitivity;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            lastMousePosition = Input.mousePosition;
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), -Input.mouseScrollDelta.y, Input.GetAxisRaw("Vertical")).normalized;

            if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width * 0.1f)
            {
                movement.x = -1;
            }
            else if (Input.mousePosition.x > Screen.width - Screen.width * 0.1f && Input.mousePosition.x < Screen.width)
            {
                movement.x = 1;
            }

            if (Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height * 0.1f)
            {
                movement.z = -1;
            }
            else if (Input.mousePosition.y > Screen.height - Screen.height * 0.1f && Input.mousePosition.y < Screen.height)
            {
                movement.z = 1;
            }


            Vector3 toPosition = transform.position + new Vector3(movement.x * sensitivity, movement.y * scrollSensitivity, movement.z * sensitivity);
            transform.position = Vector3.Lerp(transform.position, toPosition, Time.deltaTime);
        }


        if (transform.position.x >= startPosition.x + xSize)
        {
            transform.position = new Vector3(startPosition.x + xSize, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < startPosition.x - xSize)
        {
            transform.position = new Vector3(startPosition.x - xSize, transform.position.y, transform.position.z);
        }

        if (transform.position.z >= startPosition.z + zSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + zSize);
        }
        else if (transform.position.z < startPosition.z - zSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z - zSize);
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
