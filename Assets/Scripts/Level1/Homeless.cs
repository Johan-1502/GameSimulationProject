using UnityEngine;

public class Homeless : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public float speed = 1.5f;
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb2D.linearVelocity = new Vector2(-1*speed,rb2D.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.multiplySpeed(slowMultiplier);
                player.decreaseLife(10);
                player.startBeatenMode();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.ResetSpeed();
        }
    }
}
