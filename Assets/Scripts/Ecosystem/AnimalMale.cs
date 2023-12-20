using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMale : Animal
{
    [Header("Male Characteristics")]
    public Gene approachRange;
    public Gene reproductionUrge;

    protected override void Update()
    {
        if (isChild) growthProgress += Time.deltaTime;

        HandleNeeds();

        switch (currentNeed)
        {
            case Need.Food:
                HandleHunger();
                break;
            case Need.Reproduction:
                HandleReproduction();
                break;
            case Need.Exploration:
                Explore();
                break;
        }
    }

    protected override void HandleNeeds()
    {
        base.HandleNeeds();
        if (isChild) return;

        if (reproductionUrge.value > hunger)
            currentNeed = Need.Reproduction;
    }

    private void HandleReproduction()
    {
        // Check if any female can be approached
        Collider[] animals = Physics.OverlapSphere(transform.position, approachRange.value, whatIsAnimal);
        if (animals.Length > 0)
        {
            AnimalFemale female = animals[0].GetComponentInParent<AnimalFemale>();
            if (female != null && !female.isPregnant && !female.isChild)
                female.Approach(this);
        }

        // Check if there is any female further away that could be approached
        animals = Physics.OverlapSphere(transform.position, spotRange.value, whatIsAnimal);
        bool femaleFound = false;
        if (animals.Length > 0)
        {
            foreach (Collider c in animals)
            {
                AnimalFemale female = c.GetComponentInParent<AnimalFemale>();
                if (female != null && !female.isPregnant && !female.isChild)
                {
                    femaleFound = true;
                    agent.SetDestination(c.transform.position);
                    return;
                }
            }
        }

        if (!femaleFound)
        {
            if (hunger > minimumHunger)
            {
                currentNeed = Need.Food;
                base.HandleHunger();
            }
            else
            {
                currentNeed = Need.Exploration;
                base.Explore();
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, approachRange.value);
    }
}
