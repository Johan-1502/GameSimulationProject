using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfEntry : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.setSpeed(0f);
            player.stopMove();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

        }
    }
}
