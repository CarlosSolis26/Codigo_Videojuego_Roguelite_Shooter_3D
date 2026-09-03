using UnityEngine;

public class PlayerAimRotation : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 25f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RotateTowardsCamera();
        }
    }

    void RotateTowardsCamera()
    {
        Vector3 direction = cameraTransform.forward;

        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
