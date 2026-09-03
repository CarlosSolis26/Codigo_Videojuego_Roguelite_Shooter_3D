using UnityEngine;
using Unity.Cinemachine;

public class PlayerAimCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public float normalFOV = 40f;
    public float aimFOV = 30f;

    public float zoomSpeed = 10f;

    void Update()
    {
        bool aiming = Input.GetMouseButton(1);

        float targetFOV = aiming ? aimFOV : normalFOV;

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
            virtualCamera.m_Lens.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }
}
