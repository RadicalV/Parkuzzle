using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    private const string PLAYER_PREFS_SENSITIVITY = "MouseSensitivity";

    [Header("References")]
    public Transform bodyTransform;

    [Header("Sensitivity")]
    public float sensitivityMultiplier = 0.5f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;
    private Vector3 realRotation;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) gameObject.SetActive(false);
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (PlayerPrefs.HasKey(PLAYER_PREFS_SENSITIVITY))
        {
            horizontalSensitivity = verticalSensitivity = PlayerPrefs.GetFloat(PLAYER_PREFS_SENSITIVITY);
        }
    }

    void Update()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_SENSITIVITY))
        {
            var newSensitivity = PlayerPrefs.GetFloat(PLAYER_PREFS_SENSITIVITY);

            if (newSensitivity != horizontalSensitivity)
                horizontalSensitivity = verticalSensitivity = newSensitivity;
        }

        float xCameraMovement = InputManager.Instance.horizontalCameraInput * horizontalSensitivity * sensitivityMultiplier;
        float yCameraMovement = -InputManager.Instance.verticalCameraInput * verticalSensitivity * sensitivityMultiplier;

        realRotation = new Vector3(Mathf.Clamp(realRotation.x + yCameraMovement, -90f, 90f), realRotation.y + xCameraMovement, realRotation.z);
        realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);

        bodyTransform.eulerAngles = Vector3.Scale(realRotation, new Vector3(0f, 1f, 0f));
        transform.eulerAngles = realRotation;
    }

    public void SetYRotation(float yRotationValue)
    {
        realRotation.y = yRotationValue;
    }
}