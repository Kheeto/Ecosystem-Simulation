using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private float spawnRadius = 150f;
    [SerializeField] private int maleAmount = 50;
    [SerializeField] private int femaleAmount = 50;
    [SerializeField] private int grassAmount = 200;

    [Header("References")]
    [SerializeField] private GameObject malePrefab;
    [SerializeField] private GameObject femalePrefab;
    [SerializeField] private GameObject grassPrefab;

    private void Awake()
    {
        for (int i = 0; i < maleAmount; i++)
        {
            Vector3 spawnPosition;
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out spawnPosition))
            {
                Instantiate(malePrefab, spawnPosition, Quaternion.identity, transform);
            }
        }

        for (int i = 0; i < femaleAmount; i++)
        {
            Vector3 spawnPosition;
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out spawnPosition))
            {
                Instantiate(femalePrefab, spawnPosition, Quaternion.identity, transform);
            }
        }

        for (int i = 0; i < grassAmount; i++)
        {
            Vector3 spawnPosition;
            if (NavMeshUtility.RandomPoint(transform.position, spawnRadius, out spawnPosition))
            {
                Instantiate(grassPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}
