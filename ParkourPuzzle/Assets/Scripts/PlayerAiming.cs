using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    InputManager inputManager;

    [Header("References")]
    public Transform bodyTransform;

    [Header("Sensitivity")]
    public float sensitivityMultiplier = 1f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;
    private Vector3 realRotation;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float xCameraMovement = inputManager.horizontalCameraInput * horizontalSensitivity * sensitivityMultiplier;
        float yCameraMovement = -inputManager.verticalCameraInput * verticalSensitivity * sensitivityMultiplier;

        realRotation = new Vector3(Mathf.Clamp(realRotation.x + yCameraMovement, -90f, 90f), realRotation.y + xCameraMovement, realRotation.z);
        realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);

        bodyTransform.eulerAngles = Vector3.Scale(realRotation, new Vector3(0f, 1f, 0f));
        transform.eulerAngles = realRotation;
    }
}
