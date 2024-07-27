using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float slowMotionPercent = 0.2f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)] private float upwardsForceOnCollisionPercent = 0.5f;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Vector3 initialMousePosition;
    private Vector3 direction;
    private bool canKill = false;
    private float initialCooldown;
    private bool firstCollision = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        initialCooldown = cooldown;
    }

    void Update()
    {
        if (cooldown >= 0.0f)
        {
            cooldown -= Time.unscaledDeltaTime;
        }
        else
        {
            DetectInput();
        }
    }

    private void DetectInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Time.timeScale = slowMotionPercent;
            direction = (Input.mousePosition - initialMousePosition).normalized;
            Debug.DrawRay(transform.position, direction * 2, Color.red);
        }

        if (Input.GetMouseButtonUp(0))
        {
            rb2d.velocity = direction * speed;
            Time.timeScale = 1.0f;
            cooldown = initialCooldown;
            firstCollision = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canKill)
        {
            canKill = false;
        }
        else
        {
            canKill = true;
        }

        canKill = canKill ? false : true;

        canKill = !canKill;

        sr.color = canKill ? Color.white : Color.red;

        if (!canKill)
        {
            rb2d.velocity += new Vector2(0.0f, 1.0f) * rb2d.velocity.magnitude * upwardsForceOnCollisionPercent;
        }
    }
}
