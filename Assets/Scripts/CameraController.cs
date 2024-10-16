using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private float maxYAngle = 80.0f;
    private Vector2 currentRotation;

    void Update()
    {
        // Handle camera rotation
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        // Handle camera movement
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(moveSide, 0, moveForward);
    }
}
