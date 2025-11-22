using UnityEngine;

public class HoleLv2 : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.multiplySpeed(slowMultiplier);
            player.decreaseLife(5);
            player.startBeatenMode();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.ResetSpeed();
        }
    }
}
