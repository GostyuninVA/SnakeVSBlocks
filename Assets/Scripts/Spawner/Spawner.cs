using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _repeatCount;
    [SerializeField] private int _distanceBetweenFullLine;
    [SerializeField] private int _distanceBetweenRandomLine;

    [SerializeField] private Block _blockTempLate;
    [SerializeField] private int _blockSpawnChance;

    private SpawnPoint[] _blockSpawnPoints;

    private void Start()
    {
        _blockSpawnPoints = GetComponentsInChildren<SpawnPoint>();

        for(int i = 0; i < _repeatCount; i++)
        {
            GenerateFullLine(_blockSpawnPoints, _blockTempLate.gameObject);
            MoveSpawner(_distanceBetweenFullLine);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position, generatedElement);
        }
    }

    private void GenerateRandomElements()
    {

    }
    
    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement)
    {
        spawnPoint.y -= generatedElement.transform.localScale.y;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity, _container);
    }

    private void MoveSpawner(int distnaceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distnaceY, transform.position.z);
    }

}
