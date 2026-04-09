using UnityEngine;

public class RotateLab : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 50f;
    public float maxRotation = 360f;

    private float currentAngle = 0f;
    private float startAngle = 0f;

    void Start()
    {
        startAngle = transform.localEulerAngles.x;
        currentAngle = startAngle;
    }

    void Update()
    {
        // rotate when space is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            currentAngle += rotateSpeed * Time.deltaTime;

            // clamp at 360 degrees from start
            if (currentAngle > startAngle + maxRotation)
                currentAngle = startAngle + maxRotation;
        }
        else
        {
            // rotate counterclockwise when space released
            currentAngle -= rotateSpeed * Time.deltaTime;

            // rotates till initialrotation
            if (currentAngle < startAngle)
                currentAngle = startAngle;
        }

        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }
}