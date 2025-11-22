using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
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
    public float vulnerabilityDuration = 3f;

    private bool isDoingSpecial = false;
    public bool isVulnerable = false;
    private bool isDead = false;
    private bool introFinished = false;

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

    [Header("UI - Globo de diálogo")]
    public GameObject bossDialogBubble;
    public TextMeshProUGUI bossDialogText;

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

        bossDialogText.text = "";
        bossDialogBubble.SetActive(false);

        StartCoroutine(IntroCutscene());
    }

    void Update()
    {
        if (isDead) return;
        if (!introFinished) return;

        // Globo sigue al boss
        bossDialogBubble.transform.position = transform.position + new Vector3(0, 1.3f, 0);

        if (isDoingSpecial) return;

        if (Time.time >= nextDecisionTime)
        {
            nextDecisionTime = Time.time + decisionInterval;
            DecideAction();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

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
        if (isDoingSpecial || isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Ataque especial
        if (dist <= (safeDistance + 1f) && Random.value <= specialAttackChance)
        {
            StartCoroutine(DoSpecialAttack());
            return;
        }

        // Markov 60% mover, 40% atacar
        if (Random.value < 0.60f)
            DecideMovement(dist);
        else
            StartCoroutine(HacerAtaqueNormal());
    }

    // =====================================================
    // MOVIMIENTO
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
    // ATAQUE NORMAL
    // =====================================================
    IEnumerator HacerAtaqueNormal()
    {
        if (isDead) yield break;
        if (isDoingSpecial) yield break;

        currentState = BossState.Attacking;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            AtaqueNormalMarkov();
            yield return new WaitForSeconds(0.5f);
        }

        currentState = BossState.Move;
    }

    void AtaqueNormalMarkov()
    {
        float r = Random.value;

        if (r < 0.50f) { Shoot(cafePrefab); return; }
        if (r < 0.80f) { Shoot(papelPrefab); return; }
        if (r < 0.90f) { Shoot(nota3Prefab); return; }
        if (r < 0.98f) { Shoot(nota0Prefab); return; }

        Shoot(gptPrefab);
    }

    void Shoot(GameObject prefab)
    {
        if (isDead) return;

        GameObject p = Instantiate(prefab, transform.position, Quaternion.identity);
        Projectile proj = p.GetComponent<Projectile>();

        proj.direction = (player.position - transform.position).normalized;
        proj.player = player;

        if (prefab == nota3Prefab || prefab == gptPrefab)
            proj.healsPlayer = true;
    }

    // =====================================================
    // ATAQUE ESPECIAL
    // =====================================================
    IEnumerator DoSpecialAttack()
    {
        if (isDead) yield break;

        currentState = BossState.Attacking;
        isDoingSpecial = true;

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isSpecialAttacking", true);

        for (int wave = 0; wave < 3; wave++)
        {
            for (int i = 0; i < librosCount; i++)
            {
                float angle = (360f / librosCount) * i;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                          Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject libro = Instantiate(libroPrefab, transform.position, Quaternion.identity);
                Projectile p = libro.GetComponent<Projectile>();
                p.direction = dir;
                p.player = player;
                p.healsPlayer = false;
            }

            yield return new WaitForSeconds(0.6f);
        }

        // Entra en modo vulnerable
        isDoingSpecial = false;
        isVulnerable = true;
        currentState = BossState.Vulnerable;

        ShowDialog("¡No se me acerque! Yo sé de software.");

        StartCoroutine(BlinkRed());
        yield return new WaitForSeconds(vulnerabilityDuration);

        isVulnerable = false;
        currentState = BossState.Move;

        anim.SetBool("isSpecialAttacking", false);
        sprite.color = Color.white;
        HideDialog();
    }

    // =====================================================
    // DIÁLOGO
    // =====================================================
    void ShowDialog(string text)
    {
        bossDialogBubble.SetActive(true);
        bossDialogText.text = text;
    }

    void HideDialog()
    {
        bossDialogBubble.SetActive(false);
        bossDialogText.text = "";
    }

    // =====================================================
    // PARPADEO
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
        if (!isVulnerable || isDead) return;

        currentHealth -= dmg;
        bossHealthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        isDoingSpecial = true;
        isVulnerable = false;
        currentState = BossState.Vulnerable;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        anim.SetBool("isSpecialAttacking", true);

        StartCoroutine(VictoryCutscene());
    }

    IEnumerator IntroCutscene()
    {
        ShowDialog("¿Así que tú eres el que piensa que puede pasar mi materia?… qué metro cuadrado de terquedad tan grande.");
        yield return new WaitForSeconds(4f);

        HideDialog();
        introFinished = true;
    }

    IEnumerator VictoryCutscene()
    {
        rb.linearVelocity = Vector2.zero;

        ShowDialog("¿Cómo lo hiciste?... Nadie había pasado mi materia desde el 10 de octubre de 1953…");
        yield return new WaitForSeconds(2f);

        ShowDialog("Con ChatGPT a mi lado, no hay nada que no pueda hacer.");
        yield return new WaitForSeconds(4f);

        HideDialog();
        yield return new WaitForSeconds(0.5f);

        // Activar animación de correr (si existe)
        if (anim != null)
            anim.SetFloat("Speed", 1f);

        StartCoroutine(EscapeRoutine());
        SceneManager.LoadScene("CreditsScene");
    }


    IEnumerator EscapeRoutine()
    {
        // Permitir movimiento
        rb.bodyType = RigidbodyType2D.Dynamic;

        float escapeSpeed = 3f;
        float escapeDuration = 3f;
        float timer = 0f;

        while (timer < escapeDuration)
        {
            rb.linearVelocity = Vector2.right * escapeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isVulnerable && collision.collider.CompareTag("Player") && !isDead)
            TakeDamage(1);
    }
}
