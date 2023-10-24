using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFemale : Animal
{
    [Header("Female Characteristics")]
    public Gene gestationTime;
    [SerializeField] private int birthAmount = 2;
    public GameObject femaleChildPrefab;
    public GameObject maleChildPrefab;
    public bool isPregnant;

    public void Approach (AnimalMale partner)
    {
        Debug.Log("Female was approached");
        StartCoroutine(GiveBirth(partner));
    }

    private IEnumerator GiveBirth (AnimalMale father)
    {
        isPregnant = true;
        Debug.Log("Female got pregnant");

        yield return new WaitForSeconds(gestationTime.value);

        Debug.Log("Female giving birth");
        for (int i = 0; i < birthAmount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere;

            // A male is born
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                Debug.Log("A male was born");
                GameObject child = Instantiate(maleChildPrefab, spawnPosition, Quaternion.identity);
                AnimalMale male = child.GetComponent<AnimalMale>();

                // Mutate the genes
                male.reproductionUrge = father.reproductionUrge.Mutate();
                male.approachRange = father.approachRange.Mutate();
                male.spotRange = InheritGene(father.spotRange.Mutate(), spotRange.Mutate());
                male.eatRange = InheritGene(father.eatRange.Mutate(), eatRange.Mutate());
                male.growthTime = InheritGene(father.growthTime.Mutate(), growthTime.Mutate());
                male.speed = InheritGene(father.speed.Mutate(), speed.Mutate());
                male.UpdateSpeed();

                male.SetChild(gestationTime.value);
            }
            // A female is born
            else
            {
                Debug.Log("A female was born");
                GameObject child = Instantiate(femaleChildPrefab, spawnPosition, Quaternion.identity);
                AnimalFemale female = child.GetComponent<AnimalFemale>();

                // Mutate the genes
                female.gestationTime = gestationTime.Mutate();
                female.growthTime = InheritGene(father.growthTime.Mutate(), growthTime.Mutate());
                female.spotRange = InheritGene(father.spotRange.Mutate(), spotRange.Mutate());
                female.eatRange = InheritGene(father.eatRange.Mutate(), eatRange.Mutate());
                female.speed = InheritGene(father.speed.Mutate(), speed.Mutate());
                female.UpdateSpeed();

                female.femaleChildPrefab = femaleChildPrefab;
                female.maleChildPrefab = maleChildPrefab;
                female.SetChild(gestationTime.value);
            }
        }
        isPregnant = false;
    }

    private Gene InheritGene (Gene father, Gene mother)
    {
        // Inherit gene from mother or father with equal probability
        return (Random.Range(0f, 1f) <= 0.5f) ? father : mother;
    }
}
