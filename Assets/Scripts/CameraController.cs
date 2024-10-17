using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private float maxYAngle = 80.0f;
    private Vector2 currentRotation;

    private void Start()
    {
        GameManager.Instance.OnPauseEvent += OnPause;
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f;

        // Handle camera rotation
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        // Handle camera movement
        float moveForward = Input.GetAxis("Vertical") * speed * speedMultiplier * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * speedMultiplier * Time.deltaTime;
        transform.Translate(moveSide, 0, moveForward);
    }

    private void OnPause(bool isPaused)
    {
        if (isPaused)
        {
            UnlockMouse();
        }
        else
        {
            LockMouse();
        }
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
