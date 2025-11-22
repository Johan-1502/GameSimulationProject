using System.Collections;
using UnityEngine;

public class WalkToEntryLv2 : MonoBehaviour
{
    private GameObject endOfEntry;
    private bool hasStopped = false;
    private GameObject message;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(wait(collision));
        }
    }

    public void setEntry(GameObject entry)
    {
        endOfEntry = entry;
    }

    public void setMessage(GameObject messageText)
    {
        message = messageText;
    }

    IEnumerator wait(Collider2D collision)
    {
        
            Player player = collision.GetComponent<Player>();
            message.SetActive(true);
            player.detain();
            yield return new WaitForSeconds(4f);
            message.SetActive(false);        
            player.continueMoving();
            player.setEntry(endOfEntry);
        
    }
}
