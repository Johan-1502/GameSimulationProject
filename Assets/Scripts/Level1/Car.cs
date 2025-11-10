using Unity.Mathematics;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 1.5f;
    private int velocityDirection = -1;
    private Rigidbody2D rb2D;
    private GameObject player;
    public bool isLeftCar;
    public float velocity = 0f;
    public int continuousDamage = 10;

    public float damageDelay = 2f;

    private float timeInside = 0f;
    private int initialDamage = 20;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeftCar)
        {
            velocity = velocityDirection * speed + (math.abs(player.GetComponent<Player>().getxVelocity())-0.5f);
            rb2D.linearVelocity = new Vector2(velocity, rb2D.linearVelocity.y);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(velocityDirection * speed, rb2D.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.decreaseLife(initialDamage);
            player.startBeatenMode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeInside += Time.deltaTime; 
            if (timeInside >= damageDelay)
            {
                collision.GetComponent<Player>().decreaseLife(continuousDamage);
                timeInside = 0f;
            }
        }
    }

    public void setVelocityDirection(int velocity)
    {
        velocityDirection = velocity;
    }
    public int getVelocityDirection()
    {
        return velocityDirection;
    }

    public void setAsLeftCar()
    {
        isLeftCar = true;
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
