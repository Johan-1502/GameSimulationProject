using UnityEngine;

public class WalkToEntry : MonoBehaviour
{
    private GameObject endOfEntry;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.setEntry(endOfEntry);
        }
    }

    public void setEntry(GameObject entry)
    {
        endOfEntry = entry;
    }
}
