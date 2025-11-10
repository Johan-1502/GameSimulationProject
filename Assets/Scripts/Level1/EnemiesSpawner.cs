using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CarSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject[] carsPrefabs;
    public GameObject[] homelessPrefabs;
    public float[] carsSpawnPoints;
    public float[] homelessSpawnPoints;
    public float spawnOffset = 2f;
    public Transform cameraTransform;
    private Player playerScript;
    private MarkovAssignation markovAssignation;
    float[,] probabilityMatrix = {
    {0.05f, 0.15f, 0.3f, 0.3f, 0.15f, 0.05f},
    {0.05f, 0.05f, 0.15f, 0.3f, 0.3f, 0.15f},
    {0.15f, 0.05f, 0.05f, 0.15f, 0.3f, 0.3f},
    {0.3f, 0.15f, 0.05f, 0.05f, 0.15f, 0.3f},
    {0.3f, 0.3f, 0.15f, 0.05f, 0.05f, 0.15f},
    {0.15f, 0.3f, 0.3f, 0.15f, 0.05f, 0.05f},
};

    void Start()
    {
        markovAssignation = new MarkovAssignation(probabilityMatrix, 0);
        playerScript = player.GetComponent<Player>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (!playerScript.hasEnded)
        {
            float rightLimit = Camera.main.ViewportToWorldPoint(
                new Vector3(1, 0.5f, cameraTransform.position.z * -1)
            ).x;
            float randomValue = Random.Range(0.0f, 1f);
            int chosenLane = markovAssignation.transformValue(randomValue);
            print(chosenLane);
            if (chosenLane>0 && chosenLane<5)
            {
                int yPosition = chosenLane-1;
                if (yPosition % 2 == 0)
                {
                    Vector3 spawnPosition = new Vector3(rightLimit + spawnOffset, carsSpawnPoints[yPosition], 0f);
                    GameObject carPrefab = carsPrefabs[Random.Range(0, carsPrefabs.Length)];
                    GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
                    car.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, cameraTransform.position.z * -1)).x;
                    Vector3 spawnPosition = new Vector3(leftLimit, carsSpawnPoints[yPosition], 0f);
                    GameObject carPrefab = carsPrefabs[Random.Range(0, carsPrefabs.Length)];
                    GameObject carInstance = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
                    carInstance.transform.rotation = Quaternion.Euler(0, 0, 90);
                    Car car = carInstance.GetComponent<Car>();
                    car.setVelocityDirection(1);
                    car.setPlayer(player);
                    car.setAsLeftCar();
                }
            }
            else
            {
                int yposition = chosenLane==5?1:chosenLane;
                Vector3 spawnPosition = new Vector3(rightLimit + spawnOffset, homelessSpawnPoints[yposition], 0f);
                GameObject homelessPrefab = homelessPrefabs[Random.Range(0, homelessPrefabs.Length)];
                Instantiate(homelessPrefab, spawnPosition, Quaternion.identity);
            }
            float time = TimeConversor.transform2(Random.Range(0f, 1f));
            print(time);
            yield return new WaitForSeconds(time);
        }
    }
}
