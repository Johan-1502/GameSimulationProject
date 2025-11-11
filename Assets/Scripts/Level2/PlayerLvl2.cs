using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLvl2 : MonoBehaviour
{
    private float xSpeed = 1.5f;
    private float ySpeed = 2f;
    public float frozenTime = 0.6f;
    private Rigidbody2D rb2D;
    private float moveX;
    private float moveY;
    public int lifeCount = 100;
    public TMP_Text textLife;
    private Animator animator;
    public float timeRemaining = 90f;
    private bool isDecreasing = false;
    private int currentHealth;
    public HealthBar healthBar;
    public TMP_Text timerText;
    public GameOverManager gameOverManager;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = lifeCount;
        healthBar.SetMaxHealth(lifeCount);
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        rb2D.linearVelocity = new Vector2(moveX * xSpeed, moveY * ySpeed);

        animator.SetFloat("Speed", Mathf.Abs(moveX) + Mathf.Abs(moveY));
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            showTime(timeRemaining);
        }
        else
        {
            timerText.text = "00:00";
            if (lifeCount <= 0)
            {
                gameOverManager.ShowGameOver();
            }
            if (!isDecreasing)
            {
                isDecreasing = true;
                InvokeRepeating("decreaseLife", 0f, 1f);
            }
        }
    }
    void showTime(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    public void decreaseLife(int quantity)
    {
        lifeCount -= quantity;
        if (lifeCount <= 0)
        {
            gameOverManager.ShowGameOver();
        }
        decreaseHealthBar(quantity);
        textLife.text = "life: " + lifeCount.ToString();

    }

    public void decreaseLife()
    {
        lifeCount -= 3;
        if (lifeCount <= 0)
        {
            gameOverManager.ShowGameOver();
        }
        decreaseHealthBar(3);
        textLife.text = "life: " + lifeCount.ToString();
        startBeatenMode();
    }

    public void decreaseHealthBar(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth);
    }

    public void ResetSpeed()
    {
        xSpeed = 1.5f;
    }

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

    public void multiplySpeed(float factor)
    {
        xSpeed = xSpeed * factor;
    }

    public void setSpeed(float speedToSet)
    {
        animator.SetFloat("Speed", speedToSet);
        xSpeed = speedToSet;
    }

    public float getxVelocity()
    {
        return xSpeed * moveX;
    }
}
