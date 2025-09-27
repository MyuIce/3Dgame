using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float sensitivity = 1000f;
    public float pitchMin = -20f;
    public float pitchMax = 45f;

    private float yaw = 0f;
    private float pitch = 0f;

    public float CurrentYaw => yaw;

    public bool canRotate = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (!canRotate) return;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * 1.0f);
        

        }

}


