using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private float spawnRadius = 150f;
    [SerializeField] private int rabbitMaleAmount = 50;
    [SerializeField] private int rabbitFemaleAmount = 50;
    [SerializeField] private int foxMaleAmount = 4;
    [SerializeField] private int foxFemaleAmount = 4;
    [SerializeField] private int grassAmount = 200;

    [Header("References")]
    [SerializeField] private GameObject rabbitMalePrefab;
    [SerializeField] private GameObject rabbitFemalePrefab;
    [SerializeField] private GameObject foxMalePrefab;
    [SerializeField] private GameObject foxFemalePrefab;
    [SerializeField] private GameObject grassPrefab;

    public void Initialize(int rabbitMaleAmount, int rabbitFemaleAmount, int foxMaleAmount, int foxFemaleAmount, int grassAmount)
    {
        this.rabbitMaleAmount = rabbitMaleAmount;
        this.rabbitFemaleAmount = rabbitFemaleAmount;
        this.foxMaleAmount = foxMaleAmount;
        this.foxFemaleAmount = foxFemaleAmount;
        this.grassAmount = grassAmount;
    }

    public void SpawnEntities()
    {
        // Spawn rabbit males
        for (int i = 0; i < rabbitMaleAmount; i++)
        {
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out Vector3 spawnPosition))
                Instantiate(rabbitMalePrefab, spawnPosition, Quaternion.identity, transform);
        }

        // Spawn rabbit females
        for (int i = 0; i < rabbitFemaleAmount; i++)
        {
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out Vector3 spawnPosition))
                Instantiate(rabbitFemalePrefab, spawnPosition, Quaternion.identity, transform);
        }

        // Spawn fox males
        for (int i = 0; i < foxMaleAmount; i++)
        {
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out Vector3 spawnPosition))
                Instantiate(foxMalePrefab, spawnPosition, Quaternion.identity, transform);
        }

        // Spawn fox females
        for (int i = 0; i < foxFemaleAmount; i++)
        {
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out Vector3 spawnPosition))
                Instantiate(foxFemalePrefab, spawnPosition, Quaternion.identity, transform);
        }

        // Spawn grass
        for (int i = 0; i < grassAmount; i++)
        {
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out Vector3 spawnPosition))
                Instantiate(grassPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}
