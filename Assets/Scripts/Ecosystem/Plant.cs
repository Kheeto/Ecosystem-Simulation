using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Plant : MonoBehaviour
{
    [Header("Plant Characteristics")]
    [SerializeField] private float reproduceTime = 5f;
    [SerializeField] private float reproduceChance = 0.3f;
    [SerializeField] private float spreadRange = 10f;
    [SerializeField] private int minimumSeeds = 0;
    [SerializeField] private int maximumSeeds = 2;

    [Header("References")]
    [SerializeField] private GameObject plantPrefab;

    private void Awake()
    {
        StartCoroutine(Reproduce());
    }

    private IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(reproduceTime);

        if (Random.Range(0f, 1f) <= reproduceChance)
        {
            int seedAmount = Random.Range(minimumSeeds, maximumSeeds + 1);
            for (int i = 0; i < seedAmount; i++)
            {
                Vector3 spawnPosition;
                if (NavMeshUtility.RandomPoint(transform.position, spreadRange, out spawnPosition))
                {
                    Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }

        StartCoroutine(Reproduce());
    }
}
