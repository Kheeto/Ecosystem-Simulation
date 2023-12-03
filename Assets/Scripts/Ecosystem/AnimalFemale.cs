using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFemale : Animal
{
    [Header("Female Characteristics")]
    public Gene gestationTime;
    [SerializeField] private int minBirthAmount = 3;
    [SerializeField] private int maxBirthAmount = 5;
    public string femaleChildPrefab;
    public string maleChildPrefab;
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

        isPregnant = false;

        Debug.Log("Female giving birth");
        for (int i = 0; i < Random.Range(minBirthAmount, maxBirthAmount+1); i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere;

            // A male is born
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                Debug.Log("A male was born");
                GameObject child = Instantiate(Resources.Load(maleChildPrefab) as GameObject, spawnPosition, Quaternion.identity);
                AnimalMale male = child.GetComponent<AnimalMale>();

                // Mutate the genes
                male.reproductionUrge = father.reproductionUrge.Mutate();
                male.approachRange = father.approachRange.Mutate();
                male.spotRange = InheritGene(father.spotRange.Mutate(), spotRange.Mutate());
                male.growthTime = InheritGene(father.growthTime.Mutate(), growthTime.Mutate());
                male.speed = InheritGene(father.speed.Mutate(), speed.Mutate());
                male.UpdateSpeed();

                male.SetChild(gestationTime.value);
            }
            // A female is born
            else
            {
                Debug.Log("A female was born");
                GameObject child = Instantiate(Resources.Load(femaleChildPrefab) as GameObject, spawnPosition, Quaternion.identity);
                AnimalFemale female = child.GetComponent<AnimalFemale>();

                // Mutate the genes
                female.gestationTime = gestationTime.Mutate();
                female.growthTime = InheritGene(father.growthTime.Mutate(), growthTime.Mutate());
                female.spotRange = InheritGene(father.spotRange.Mutate(), spotRange.Mutate());
                female.speed = InheritGene(father.speed.Mutate(), speed.Mutate());
                female.UpdateSpeed();

                female.femaleChildPrefab = femaleChildPrefab;
                female.maleChildPrefab = maleChildPrefab;
                female.SetChild(gestationTime.value);
            }
        }
    }

    private Gene InheritGene (Gene father, Gene mother)
    {
        // Inherit gene from mother or father with equal probability
        return (Random.Range(0f, 1f) <= 0.5f) ? father : mother;
    }
}
