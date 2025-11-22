using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLvl3 : MonoBehaviour
{
    private float xSpeed = 1.5f;
    private float ySpeed = 2f;
    public float frozenTime = 0.6f;

    private Rigidbody2D rb2D;
    private float moveX;
    private float moveY;

    public int lifeCount = 100; // vida base real
    public int lifeReference = 100; // vida base real
    private int currentHealth;

    public TMP_Text textLife;
    private Animator animator;

    // TIEMPO DEL NIVEL → 2 MINUTOS
    public float timeRemaining = 120f;
    private bool isDecreasing = false;

    public HealthBar healthBar;
    public TMP_Text timerText;
    public GameOverManager gameOverManager;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = lifeCount;
        lifeReference = lifeCount;
        healthBar.SetMaxHealth(lifeCount);
        textLife.text = "life: " + lifeCount;
    }

    void Update()
    {
        // Movimiento del jugador
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        rb2D.linearVelocity = new Vector2(moveX * xSpeed, moveY * ySpeed);

        animator.SetFloat("Speed", Mathf.Abs(moveX) + Mathf.Abs(moveY));

        // ===================================================
        // MANEJO DEL TIEMPO
        // ===================================================
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            showTime(timeRemaining);
        }
        else
        {
            timerText.text = "00:00";

            if (!isDecreasing)
            {
                isDecreasing = true;
                InvokeRepeating("LoseLifeOverTime", 0f, 1f);
            }
        }
    }

    // Mostrar tiempo en formato MM:SS
    void showTime(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // ===================================================
    // PÉRDIDA DE VIDA POR FIN DEL TIEMPO
    // ===================================================
    void LoseLifeOverTime()
    {
        decreaseLife(1);
    }

    // ===================================================
    // RECIBIR DAÑO NORMAL
    // ===================================================
    public void decreaseLife(int quantity)
    {
        lifeCount -= quantity;
        currentHealth -= quantity;

        if (lifeCount <= 0 || currentHealth <= 0)
        {
            gameOverManager.ShowGameOver();
        }

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth);
        textLife.text = "life: " + lifeCount;

        startBeatenMode();
    }

    // ===================================================
    // CURACIÓN DEL JUGADOR
    // ===================================================
    public void healLife(int amount)
    {
        lifeCount += amount;
        currentHealth += amount;

        if (currentHealth > lifeReference)
        {
            currentHealth = lifeReference;
            lifeCount = lifeReference;
        }

        healthBar.SetHealth(currentHealth);
        textLife.text = "life: " + lifeCount;
    }

    // ===================================================
    // ANIMACIÓN DE DAÑO
    // ===================================================
    private IEnumerator beatenMode()
    {
        animator.SetBool("Beaten", true);
        yield return new WaitForSeconds(frozenTime);
        animator.SetBool("Beaten", false);
    }

    public void startBeatenMode()
    {
        StartCoroutine(beatenMode());
    }
}
