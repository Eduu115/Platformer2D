using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [Header("Tiempo entre rotaciones")]
    public float minTime = 2f;
    public float maxTime = 5f;

    [Header("Velocidad de rotación")]
    public float rotationSpeed = 180f;

    [Header("Temblor previo")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.05f;
    public float shakeSpeed = 30f;

    private enum State { Waiting, Shaking, Rotating }
    private State currentState = State.Waiting;

    private float timer;
    private float shakeTimer;
    private float currentAngle = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        ScheduleNextRotation();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Waiting:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    StartShaking();
                }
                break;

            case State.Shaking:
                shakeTimer -= Time.deltaTime;

                // Movimiento oscilante en X
                float offsetX = Mathf.Sin(shakeTimer * shakeSpeed) * shakeMagnitude;
                transform.localPosition = originalPosition + new Vector3(offsetX, 0f, 0f);

                if (shakeTimer <= 0f)
                {
                    // Restaurar posición antes de rotar
                    transform.localPosition = originalPosition;
                    currentState = State.Rotating;
                    currentAngle = 0f;
                }
                break;

            case State.Rotating:
                float step = rotationSpeed * Time.deltaTime;
                float remaining = 360f - currentAngle;

                if (step >= remaining)
                {
                    transform.Rotate(0f, 0f, remaining);
                    currentAngle = 0f;

                    Vector3 euler = transform.eulerAngles;
                    transform.eulerAngles = new Vector3(euler.x, euler.y, 0f);

                    currentState = State.Waiting;
                    ScheduleNextRotation();
                }
                else
                {
                    transform.Rotate(0f, 0f, step);
                    currentAngle += step;
                }
                break;
        }
    }

    void StartShaking()
    {
        originalPosition = transform.localPosition;
        shakeTimer = shakeDuration;
        currentState = State.Shaking;
    }

    void ScheduleNextRotation()
    {
        timer = Random.Range(minTime, maxTime);
    }
}