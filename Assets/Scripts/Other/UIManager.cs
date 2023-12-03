using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text dayCounter;
    [SerializeField] TMP_Text timeCounter;
    [SerializeField] TMP_Text maleRabbitCounter;
    [SerializeField] TMP_Text femaleRabbitCounter;
    [SerializeField] TMP_Text maleFoxCounter;
    [SerializeField] TMP_Text femaleFoxCounter;
    [SerializeField] TMP_Text plantCounter;

    private DayNightCycle dayNightCycle;

    private void Awake()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    private void Update()
    {
        TimeSpan time = TimeSpan.FromHours(dayNightCycle.GetTime());
        dayCounter.text = "Day " + dayNightCycle.GetDay().ToString();
        timeCounter.text = "Time of day: " + string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);

        int maleRabbits = 0, femaleRabbits = 0, maleFoxes = 0, femaleFoxes = 0;
        foreach (AnimalMale animal in FindObjectsOfType<AnimalMale>())
        {
            if (animal.type == EntityType.Rabbit) maleRabbits++;
            else maleFoxes++;
        }
        foreach (AnimalFemale animal in FindObjectsOfType<AnimalFemale>())
        {
            if (animal.type == EntityType.Rabbit) femaleRabbits++;
            else femaleFoxes++;
        }

        maleRabbitCounter.text = "Male rabbits: " + maleRabbits.ToString();
        femaleRabbitCounter.text = "Female rabbits: " + femaleRabbits.ToString();
        maleFoxCounter.text = "Male foxes: " + maleFoxes.ToString();
        femaleFoxCounter.text = "Female foxes: " + femaleFoxes.ToString();

        int food = 0;
        foreach (Food f in FindObjectsOfType<Food>())
        {
            if (f.isGrown && f.GetComponent<AnimalMale>() == null && f.GetComponent<AnimalFemale>() == null) food++;
        }

        plantCounter.text = "Plants: " + food.ToString();
    }
}
