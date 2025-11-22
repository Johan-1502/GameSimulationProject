using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfEntryLvl2 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerLvl2 player = collision.GetComponent<PlayerLvl2>();
            player.setSpeed(0f);
            player.stopMove();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

        }
    }
}
