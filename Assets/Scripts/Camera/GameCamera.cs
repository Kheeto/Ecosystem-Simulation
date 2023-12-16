using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float sensivity = 10f;

    [Header("Focus")]
    public GameObject focus;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 20f;

    bool isActive;

    private void Awake()
    {
        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            if (focus != null)
            {
                FollowFocus();
                StopFocus();
            }
            else
            {
                MoveCamera();
                LockCursor();
                RotateCamera();
            }
        }
    }

    private void FollowFocus()
    {
        transform.position = Vector3.Lerp(transform.position, focus.transform.position + offset, smoothSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(focus.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void StopFocus()
    {
        if (Input.GetMouseButtonDown(1))
        {
            focus = null;
            // Disable all the active animal info UIs
            foreach (AnimalInfoUI ui in FindObjectsOfType<AnimalInfoUI>())
            {
                ui.transform.parent.gameObject.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.Translate(Vector3.right * speed * horizontal, Space.Self);
        transform.Translate(Vector3.forward * speed * vertical, Space.Self);
    }

    private void LockCursor()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

            transform.Rotate(Vector3.up * sensivity * mouseX, Space.Self);
            transform.Rotate(-Vector3.right * sensivity * mouseY, Space.Self);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }
    }

    public void EnableCamera(bool active)
    {
        isActive = active;
    }
}
