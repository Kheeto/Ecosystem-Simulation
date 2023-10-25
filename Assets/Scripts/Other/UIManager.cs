using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text timeCounter;
    [SerializeField] TMP_Text maleCounter;
    [SerializeField] TMP_Text femaleCounter;
    [SerializeField] TMP_Text plantCounter;

    private DayNightCycle dayNightCycle;

    private void Awake()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    private void Update()
    {
        TimeSpan time = TimeSpan.FromHours(dayNightCycle.GetTime());
        timeCounter.text = "Time of day: " + string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
        maleCounter.text = "Male animals: " + FindObjectsOfType<AnimalMale>().Length.ToString();
        femaleCounter.text = "Female animals: " + FindObjectsOfType<AnimalFemale>().Length.ToString();
        plantCounter.text = "Plants: " + FindObjectsOfType<Plant>().Length.ToString();
    }
}
