using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInfoUI : MonoBehaviour
{
    [Header("Follow Animal")]
    [SerializeField] Transform targetPosition;
    [SerializeField] float smoothSpeed;

    [Header("Stats")]
    [SerializeField] TMP_Text status;
    [SerializeField] Slider statistic1;
    [SerializeField] Slider statistic2;
    [SerializeField] Slider statistic3;
    [SerializeField] Slider statistic4;

    [Header("References")]
    [SerializeField] AnimalMale animalMale;
    [SerializeField] AnimalFemale animalFemale;

    void Update()
    {
        UpdatePosition();
        UpdateStats();
    }

    void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(targetPosition.position), smoothSpeed * Time.deltaTime);
    }

    void UpdateStats()
    {
        if (animalMale != null)
        {
            if (animalMale.currentNeed == Animal.Need.Food) status.text = "Looking for food";
            else if (animalMale.currentNeed == Animal.Need.Reproduction) status.text = "Approaching female";
            else if (animalMale.currentNeed == Animal.Need.Exploration) status.text = "Exploring";
            else if (animalMale.currentNeed == Animal.Need.Flee) status.text = "Fleeing from predator";

            statistic1.value = animalMale.hunger / 10f;

            if (!animalMale.isChild)
                statistic2.value = animalMale.reproductionUrge.value / 10f;
            else
                statistic2.value = animalMale.growthProgress / animalMale.growthTime.value;

            statistic3.value = animalMale.speed.value / 15f;
            statistic4.value = animalMale.spotRange.value / 50f;
        }

        if (animalFemale != null)
        {
            if (animalFemale.currentNeed == Animal.Need.Food) status.text = "Looking for food";
            else if (animalFemale.currentNeed == Animal.Need.Exploration) status.text = "Exploring";
            else if (animalFemale.currentNeed == Animal.Need.Flee) status.text = "Fleeing from predator";

            statistic1.value = animalFemale.hunger / 10f;

            if (!animalFemale.isChild)
                statistic2.value = animalFemale.gestationProgress / animalFemale.gestationTime.value;
            else
                statistic2.value = animalFemale.growthProgress / animalFemale.growthTime.value;

            statistic3.value = animalFemale.speed.value / 15f;
            statistic4.value = animalFemale.spotRange.value / 50f;
        }
    }
}
