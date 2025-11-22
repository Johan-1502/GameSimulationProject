using UnityEngine;
using System;

public class CapuchoScript : MonoBehaviour
{
    public Transform player;
    public GameObject bottle;

    public float detectionDistance = 0.66f;
    public float throwCooldown = 1.5f; 
    private float nextThrowTime = 0f; 

    void Update()
    {
        double euclidianDistance = Math.Sqrt(
            Math.Pow(player.position.x - transform.position.x, 2) +
            Math.Pow(player.position.y - transform.position.y, 2)
        );

        if (euclidianDistance <= detectionDistance && Time.time >= nextThrowTime)
        {
            ThrowBottle();
            nextThrowTime = Time.time + throwCooldown;
        }
    }

    void ThrowBottle()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        GameObject bottleCreated = Instantiate(bottle, spawnPosition, Quaternion.identity);

        Bottle bottleScript = bottleCreated.GetComponent<Bottle>();
        if (bottleScript != null)
        {
            bottleScript.player = player;
        }
    }
}
