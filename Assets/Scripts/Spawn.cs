using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class Spawn : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> letterPrefabs = new List<GameObject>();

    public void Start()
    {
        if (Random.Range (10, 30)< 40)
        {
            int randomNumber = Random.Range (0, spawnPoints.Count);
            Transform spawnPoint = GetRandomSpawnPoint();
            GameObject letter = SpawnLetter(spawnPoint);
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    private GameObject SpawnLetter(Transform spawnPoint)
    {
        var prefab = letterPrefabs[Random.Range(0, letterPrefabs.Count)];
        return Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}