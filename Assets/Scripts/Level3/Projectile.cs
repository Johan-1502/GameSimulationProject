using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 4f;
    public Vector2 direction;
    public int damage = 1;
    public bool healsPlayer = false;

    public Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (healsPlayer)
            StartCoroutine(GreenGlow());
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            PlayerLvl3 p = other.GetComponent<PlayerLvl3>();

            if (p != null)
            {
                if (healsPlayer)
                {
                    p.healLife(damage);        // ✔ CURA
                }
                else
                {
                    p.decreaseLife(damage);    // ✔ DAÑO
                }
            }

            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    IEnumerator GreenGlow()
    {
        while (true)
        {
            sr.color = Color.green;
            yield return new WaitForSeconds(0.1f);

            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
