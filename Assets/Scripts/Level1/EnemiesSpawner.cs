using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

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
    private LinealCongruenceGenerator numbersGenerator;
    void Start()
    {
        numbersGenerator = new LinealCongruenceGenerator();
        markovAssignation = new MarkovAssignation(probabilityMatrix, 0);
        playerScript = player.GetComponent<Player>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        StartCoroutine(Spawn());
    }

    public int quantityOfRN()
    {
        int maxExtraTime = (int)(playerScript.timeRemaining/playerScript.lifeToDecreaseBySec);
        int maxQuantityToAskLane = (int)((maxExtraTime+playerScript.timeRemaining)/TimeConversor.minTime);
        int maxQuantityToAskSkin = maxQuantityToAskLane;
        int maxTimeChanges = maxQuantityToAskLane;
        int totalRandomNumbers = maxQuantityToAskLane + maxQuantityToAskSkin+maxTimeChanges;
        return totalRandomNumbers;
    }

    IEnumerator Spawn()
    {
        while (!playerScript.hasEnded)
        {
            float rightLimit = Camera.main.ViewportToWorldPoint(
                new Vector3(1, 0.5f, cameraTransform.position.z * -1)
            ).x;
            float randomValue = popNumber();
            int chosenLane = markovAssignation.transformValue(randomValue);
            print(chosenLane);
            if (chosenLane > 0 && chosenLane < 5)
            {
                int yPosition = chosenLane - 1;
                if (yPosition % 2 == 0)
                {
                    Vector3 spawnPosition = new Vector3(rightLimit + spawnOffset, carsSpawnPoints[yPosition], 0f);
                    GameObject carPrefab = carsPrefabs[(int)numbersGenerator.uniformTransform(0, carsPrefabs.Length, popNumber())];
                    GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
                    car.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, cameraTransform.position.z * -1)).x;
                    Vector3 spawnPosition = new Vector3(leftLimit, carsSpawnPoints[yPosition], 0f);
                    GameObject carPrefab = carsPrefabs[(int)numbersGenerator.uniformTransform(0, carsPrefabs.Length, popNumber())];
                    GameObject carInstance = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
                    Car car = carInstance.GetComponent<Car>();
                    car.setVelocityDirection(1);
                    car.setPlayer(player);
                    car.setAsLeftCar();
                }
            }
            else
            {
                int yposition = chosenLane == 5 ? 1 : chosenLane;
                Vector3 spawnPosition = new Vector3(rightLimit + spawnOffset, homelessSpawnPoints[yposition], 0f);
                GameObject homelessPrefab = homelessPrefabs[(int)numbersGenerator.uniformTransform(0, homelessPrefabs.Length, popNumber())];
                Instantiate(homelessPrefab, spawnPosition, Quaternion.identity);
            }
            float time = TimeConversor.transform2(popNumber());
            print(time);
            yield return new WaitForSeconds(time);
        }
    }

    public float popNumber(){
        print("Intentando imprimir valor");
        return numbersGenerator.getValue();

    }
}
