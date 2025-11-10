using System.Collections;
using UnityEngine;

public class ExtendMap : MonoBehaviour
{
    public GameObject[] mapPieces;
    public GameObject finalMapPiece;
    public GameObject mapSpreader;
    public Transform player;
    public float yposition = 0.09288f;
    private bool haveExtended = false;
    public GameObject startOfEntry;
    public GameObject endOfEntry;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !haveExtended)
        {
            Player playerScript = collision.GetComponent<Player>();
            if (!playerScript.hasEnded)
            {
                playerScript.increaseDistance();
                if (playerScript.canContinue())
                {
                    Vector3 spawnPosition = new Vector3(player.position.x + 5.21f, yposition, 0f);
                    GameObject mapPiece = mapPieces[Random.Range(0, mapPieces.Length)];
                    Instantiate(mapPiece, spawnPosition, Quaternion.identity);
                    Vector3 spawnSpreaderPosition = new Vector3(player.position.x + 4.6020005f, yposition, 0f);
                    Instantiate(mapSpreader, spawnSpreaderPosition, Quaternion.identity);
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(player.position.x + 5.21f, yposition, 0f);
                    GameObject finalMap = Instantiate(finalMapPiece, spawnPosition, Quaternion.identity);
                    Vector3 spawnSpreaderPosition = new Vector3(player.position.x + 4.6020005f, yposition, 0f);
                    Instantiate(mapSpreader, spawnSpreaderPosition, Quaternion.identity);
                    playerScript.endTravel();
                    Vector3 startEntryPosition = new Vector3(finalMap.transform.position.x+0.384f, finalMap.transform.position.y + 0.322233139f, 0f);
                    GameObject startEntryObject = Instantiate(startOfEntry, startEntryPosition, Quaternion.identity);
                    Vector3 endEntryPosition = new Vector3(finalMap.transform.position.x+1.620004f, finalMap.transform.position.y + 0.322233f, 0f);
                    GameObject endEntryObject = Instantiate(endOfEntry, endEntryPosition, Quaternion.identity);
                    WalkToEntry startEntryScript = startEntryObject.GetComponent<WalkToEntry>();
                    startEntryScript.setEntry(endEntryObject);
                    playerScript.setEntry(startEntryObject);
                }
            } else
            {
                playerScript.stopMoving();
            }
            haveExtended = true;
        }
    }

}
