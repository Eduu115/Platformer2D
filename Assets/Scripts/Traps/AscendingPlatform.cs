using UnityEngine;

public class AscendingPlatform : MonoBehaviour
{
    [Header("Movimiento")]
    public float ascendSpeed = 3f;
    public float ascendDistance = 5f;

    [Header("Respawn")]
    public float waitBeforeDisappear = 2f;
    public float fadeOutDuration = 0.5f;
    public float delayBeforeRespawn = 1.5f;
    public float fadeInDuration = 0.5f;

    private Vector3 startPosition;
    private SpriteRenderer sr;
    private float timer;
    private Color originalColor;

    private enum State { Idle, Ascending, Waiting, FadingOut, Delayed, FadingIn }
    private State currentState = State.Idle;

    void Start()
    {
        startPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Ascending:
                transform.position += Vector3.up * ascendSpeed * Time.deltaTime;

                if (transform.position.y >= startPosition.y + ascendDistance)
                {
                    transform.position = new Vector3(transform.position.x,
                                                     startPosition.y + ascendDistance,
                                                     transform.position.z);
                    timer = waitBeforeDisappear;
                    currentState = State.Waiting;
                }
                break;

            case State.Waiting:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = fadeOutDuration;
                    currentState = State.FadingOut;
                }
                break;

            case State.FadingOut:
                timer -= Time.deltaTime;
                float alphaOut = Mathf.Clamp01(timer / fadeOutDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaOut);

                if (timer <= 0f)
                {
                    // Invisible y de vuelta a la posición original
                    sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
                    transform.position = startPosition;
                    GetComponent<Collider2D>().enabled = false;
                    timer = delayBeforeRespawn;
                    currentState = State.Delayed;
                }
                break;

            case State.Delayed:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = fadeInDuration;
                    currentState = State.FadingIn;
                }
                break;

            case State.FadingIn:
                timer -= Time.deltaTime;
                float alphaIn = Mathf.Clamp01(1f - (timer / fadeInDuration));
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaIn);

                if (timer <= 0f)
                {
                    sr.color = originalColor;
                    GetComponent<Collider2D>().enabled = true;
                    currentState = State.Idle;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == State.Idle && collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < -0.5f)
            {
                currentState = State.Ascending;
            }
        }
    }
}