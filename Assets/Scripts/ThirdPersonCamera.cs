using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inspired by Sebastian's youtube series:
/// https://www.youtube.com/watch?v=sNmeK3qK7oA&list=PLFt_AvWsXl0djuNM22htmz3BUtHHtOh7v&index=8
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private float mouseSensitivity = 5;

    // target for camera
    [SerializeField] private Transform target;
    // distance camera is from target
    [SerializeField] private float distanceFromTarget = 2;
    // so we don't roll/tumble over or under character
    [SerializeField] private float minPitch = -40;
    [SerializeField] private float maxPitch = 85;

    // smooth the camera movement
    [SerializeField] private float rotationSmoothTime = 0.12f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    private float yaw;
    private float pitch;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        // update yaw and pitch based on mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        // -= most people prefer this alignment, += would be inverted mouse
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        currentRotation = Vector3.SmoothDamp(
            currentRotation,
            new Vector3(pitch, yaw),
            ref rotationSmoothVelocity,
            rotationSmoothTime
        );
        transform.eulerAngles = currentRotation;

        // This orbits us around the target
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}