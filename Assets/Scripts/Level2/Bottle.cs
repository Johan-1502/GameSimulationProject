using UnityEngine;

public class Bottle : MonoBehaviour
{
    public Transform player;
    private Vector2 direction;
    private Rigidbody2D rb2D;

    public int velocity = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = (player.position - transform.position).normalized;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.linearVelocity = direction * 1f;
    }
}
