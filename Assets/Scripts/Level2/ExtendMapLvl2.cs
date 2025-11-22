using System.Collections;
using UnityEngine;

public class ExtendMapLvl2 : MonoBehaviour
{
    public GameObject[] mapPieces;
    public GameObject finalMapPiece;
    public GameObject mapSpreader;
    public Transform player;
    public float yposition = -0.04f;
    private bool haveExtended = false;
    public GameObject startOfEntry;
    public GameObject endOfEntry;
    //public GameObject message; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !haveExtended)
        {
            PlayerLvl2 playerScript = collision.GetComponent<PlayerLvl2>();
            if (!playerScript.hasEnded)
            {
                playerScript.increaseDistance();
                if (playerScript.canContinue())
                {
                    Vector3 spawnPosition = new Vector3(player.position.x + 7f, yposition, 0f);
                    GameObject mapPiece = mapPieces[Random.Range(0, mapPieces.Length)];
                    Instantiate(mapPiece, spawnPosition, Quaternion.identity);
                    Vector3 spawnSpreaderPosition = new Vector3(player.position.x + 5.256539f, 0.02661736f, 0f);
                    Instantiate(mapSpreader, spawnSpreaderPosition, Quaternion.identity);
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(player.position.x + 5.21f, -0.022f, 0f);
                    GameObject finalMap = Instantiate(finalMapPiece, spawnPosition, Quaternion.identity);
                    Vector3 spawnSpreaderPosition = new Vector3(player.position.x + 5.05f, -0.023f, 0f);
                    Instantiate(mapSpreader, spawnSpreaderPosition, Quaternion.identity);
                    playerScript.endTravel();
                    Vector3 startEntryPosition = new Vector3(finalMap.transform.position.x+0.51998f, -0.13f, 0f);
                    GameObject startEntryObject = Instantiate(startOfEntry, startEntryPosition, Quaternion.identity);
                    Vector3 endEntryPosition = new Vector3(finalMap.transform.position.x+1.920004f,-0.13f, 0f);
                    GameObject endEntryObject = Instantiate(endOfEntry, endEntryPosition, Quaternion.identity);
                    WalkToEntryLvl2 startEntryScript = startEntryObject.GetComponent<WalkToEntryLvl2>();
                    startEntryScript.setEntry(endEntryObject);
                    //startEntryScript.setMessage(message);
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
