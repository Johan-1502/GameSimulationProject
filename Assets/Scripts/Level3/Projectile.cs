using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 4f;
    public Vector2 direction;
    public int damage = 1;
    public bool healsPlayer = false;

    public Transform player;  // ← referencia directa al jugador
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
        // Golpea solo si el objeto que tocó ES el jugador asignado
        if (other.transform == player)
        {
            PlayerLvl3 playerScript = other.GetComponent<PlayerLvl3>();

            if (playerScript != null)
            {
                if (healsPlayer)
                {
                    // Cura
                    playerScript.decreaseHealthBar(-damage);
                }
                else
                {
                    // Daño real
                    playerScript.decreaseLife(damage);
                    playerScript.startBeatenMode();
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
