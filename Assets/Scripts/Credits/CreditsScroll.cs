using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 400f;
    public float endY = 2000f;   // posición Y donde termina y cambia de escena

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // Cuando el texto supere el límite → volver al menú
        if (transform.position.y >= endY)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
