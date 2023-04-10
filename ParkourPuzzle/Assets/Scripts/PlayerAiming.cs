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
    float xRotation;
    float yRotation;

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
        yRotation += inputManager.horizontalCameraInput * horizontalSensitivity * sensitivityMultiplier;
        xRotation -= inputManager.verticalCameraInput * verticalSensitivity * sensitivityMultiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        bodyTransform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void SetYRotation(float yRotationValue)
    {
        yRotation = yRotationValue;
    }
}
