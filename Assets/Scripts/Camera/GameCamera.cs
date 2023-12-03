using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sensivity = 10f;

    bool isActive;

    private void Awake()
    {
        isActive = false;
    }

    public void Activate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            MoveCamera();
            RotateCamera();
        }
    }

    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.Translate(Vector3.right * speed * horizontal, Space.Self);
        transform.Translate(Vector3.forward * speed * vertical, Space.Self);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

        transform.Rotate(Vector3.up * sensivity * mouseX, Space.Self);
        transform.Rotate(-Vector3.right * sensivity * mouseY, Space.Self);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
    }

    public void EnableCamera(bool active)
    {
        isActive = active;
    }
}
