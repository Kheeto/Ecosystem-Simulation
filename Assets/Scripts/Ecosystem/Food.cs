using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float hungerRestore = 3f;
    [SerializeField] private float regrowthTime = 10f;
    [SerializeField] private bool destroyWhenEaten;

    [Header("References")]
    [SerializeField] private GameObject meshHolder;

    public bool isGrown;

    private void Awake()
    {
        if (!isGrown) isGrown = true;
    }

    public void Eat(Animal animal)
    {
        animal.RestoreHunger(hungerRestore);

        if (destroyWhenEaten) Destroy(gameObject);
        else
        {
            meshHolder.SetActive(false);
            isGrown = false;
            StartCoroutine(Regrow());
        }
    }

    private IEnumerator Regrow()
    {
        yield return new WaitForSeconds(regrowthTime);

        meshHolder.SetActive(true);
        isGrown = true;
    }
}
