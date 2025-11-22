using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BossController : MonoBehaviour
{
    public enum BossState { Move, Attacking, Vulnerable }
    public BossState currentState = BossState.Move;

    [Header("Jugador")]
    public Transform player;

    [Header("Movimiento")]
    public float moveSpeed = 1.7f;
    public float safeDistance = 0.5f;

    [Header("Decisiones (Markov)")]
    public float decisionInterval = 1f;
    private float nextDecisionTime = 0f;

    [Header("Ataque Especial")]
    public GameObject libroPrefab;
    public int librosCount = 8;
    public float specialAttackChance = 0.10f;
    public float vulnerabilityDuration = 4f;

    private bool isDoingSpecial = false;
    public bool isVulnerable = false;

    [Header("Ataques Normales")]
    public GameObject cafePrefab;
    public GameObject papelPrefab;
    public GameObject nota3Prefab;
    public GameObject nota0Prefab;
    public GameObject gptPrefab;

    [Header("Vida del Boss")]
    public int maxHealth = 10;
    public int currentHealth = 10;
    public Slider bossHealthSlider;

    [Header("UI - Diálogo")]
    public TextMeshProUGUI bossDialog;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Animator anim;
    private SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }

        if (bossDialog != null)
            bossDialog.text = "";
    }

    void Update()
    {
        if (isDoingSpecial) return;

        if (Time.time >= nextDecisionTime)
        {
            nextDecisionTime = Time.time + decisionInterval;
            DecideAction();
        }
    }

    void FixedUpdate()
    {
        if (currentState == BossState.Move && !isVulnerable)
            rb.linearVelocity = moveDir * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    // =====================================================
    // DECISIONES PRINCIPALES
    // =====================================================
    void DecideAction()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        // POSIBLE ATAQUE ESPECIAL
        if (dist <= (safeDistance + 1f) && Random.value <= specialAttackChance)
        {
            StartCoroutine(DoSpecialAttack());
            return;
        }

        // MARKOV 70% mover, 30% atacar
        if (Random.value < 0.70f)
            DecideMovement(dist);
        else
            StartCoroutine(HacerAtaqueNormal());
    }

    // =====================================================
    // MOVIMIENTO INTELIGENTE
    // =====================================================
    void DecideMovement(float dist)
    {
        currentState = BossState.Move;

        if (dist > safeDistance)
            moveDir = (player.position - transform.position).normalized; // perseguir
        else
            moveDir = (transform.position - player.position).normalized; // huir
    }

    // =====================================================
    // ATAQUE NORMAL (3 disparos)
    // =====================================================
    IEnumerator HacerAtaqueNormal()
    {
        currentState = BossState.Attacking;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            AtaqueNormalMarkov();
            yield return new WaitForSeconds(1f);
        }

        currentState = BossState.Move;
    }

    void AtaqueNormalMarkov()
    {
        float r = Random.value;

        if (r < 0.40f) { Shoot(cafePrefab); return; }
        if (r < 0.70f) { Shoot(papelPrefab); return; }
        if (r < 0.90f) { Shoot(nota3Prefab); return; }
        if (r < 0.95f) { Shoot(nota0Prefab); return; }

        Shoot(gptPrefab);
    }

    void Shoot(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject p = Instantiate(prefab, transform.position, Quaternion.identity);
        Projectile proj = p.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.direction = (player.position - transform.position).normalized;
            proj.player = player;
            if (prefab == nota3Prefab || prefab == gptPrefab)
                proj.healsPlayer = true;
        }
    }

    // =====================================================
    // ATAQUE ESPECIAL (3 oleadas de libros)
    // =====================================================
    IEnumerator DoSpecialAttack()
    {
        currentState = BossState.Attacking;
        isDoingSpecial = true;
        rb.linearVelocity = Vector2.zero;

        // activar animación
        if (anim != null)
            anim.SetBool("isSpecialAttacking", true);

        // 3 oleadas circulares
        for (int wave = 0; wave < 3; wave++)
        {
            for (int i = 0; i < librosCount; i++)
            {
                float angle = (360f / librosCount) * i;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                          Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject libro = Instantiate(libroPrefab, transform.position, Quaternion.identity);
                Projectile p = libro.GetComponent<Projectile>();
                p.damage = 2;
                p.direction = dir;
                p.player = player;
                p.healsPlayer = false;
            }

            yield return new WaitForSeconds(1f);
        }

        // ENTRA EN MODO VULNERABLE
        isDoingSpecial = false;
        isVulnerable = true;
        currentState = BossState.Vulnerable;

        // mantener calavera
        anim.SetBool("isSpecialAttacking", true);

        // diálogo
        if (bossDialog != null)
        {
            bossDialog.text = "¡No se me acerque! Yo sé de software.";
            bossDialog.gameObject.SetActive(true);
        }

        // iniciar parpadeo rojo
        StartCoroutine(BlinkRed());

        yield return new WaitForSeconds(vulnerabilityDuration);

        // FIN DE VULNERABLE
        isVulnerable = false;
        currentState = BossState.Move;

        // apagar animación especial
        anim.SetBool("isSpecialAttacking", false);

        // detener parpadeo
        StopCoroutine(BlinkRed());
        sprite.color = Color.white;

        // ocultar diálogo
        if (bossDialog != null)
        {
            bossDialog.text = "";
            bossDialog.gameObject.SetActive(false);
        }
    }

    // =====================================================
    // PARPADEO ROJO (cuando está vulnerable)
    // =====================================================
    IEnumerator BlinkRed()
    {
        while (isVulnerable)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // =====================================================
    // VIDA DEL BOSS
    // =====================================================
    public void TakeDamage(int dmg)
    {
        if (!isVulnerable) return;

        currentHealth -= dmg;

        if (bossHealthSlider != null)
            bossHealthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Boss derrotado!!!");
        // puedes cambiar de escena aquí
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isVulnerable && collision.collider.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }
}
