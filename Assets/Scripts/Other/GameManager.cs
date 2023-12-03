using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject gameStartUI;
    [SerializeField] GameObject simulationInfo;

    [SerializeField] Slider rabbitMaleSlider;
    [SerializeField] Slider rabbitFemaleSlider;
    [SerializeField] Slider foxMaleSlider;
    [SerializeField] Slider foxFemaleSlider;
    [SerializeField] Slider grassSlider;

    [SerializeField] TMP_Text rabbitMaleValue;
    [SerializeField] TMP_Text rabbitFemaleValue;
    [SerializeField] TMP_Text foxMaleValue;
    [SerializeField] TMP_Text foxFemaleValue;
    [SerializeField] TMP_Text grassValue;

    [Header("Entity Spawner")]
    [SerializeField] EntitySpawner entitySpawner;

    [Header("Camera")]
    [SerializeField] GameCamera gameCamera;

    [Header("Graphs")]
    [SerializeField] Graph[] graphs;

    bool hasGameStarted;

    private void Awake()
    {
        hasGameStarted = false;
    }

    public void StartGame()
    {
        if (!hasGameStarted)
        {
            hasGameStarted = true;

            gameStartUI.SetActive(false);
            entitySpawner.Initialize(
                Mathf.RoundToInt(rabbitMaleSlider.value),
                Mathf.RoundToInt(rabbitFemaleSlider.value),
                Mathf.RoundToInt(foxMaleSlider.value),
                Mathf.RoundToInt(foxFemaleSlider.value),
                Mathf.RoundToInt(grassSlider.value));
            entitySpawner.SpawnEntities();

            gameCamera.Activate();

            foreach (Graph g in graphs) g.StartGraph();

            simulationInfo.SetActive(true);
        }
    }

    public bool HasGameStarted()
    {
        return hasGameStarted;
    }

    public void UpdateRabbitMaleAmount()
    {
        rabbitMaleValue.text = Mathf.RoundToInt(rabbitMaleSlider.value).ToString();
    }

    public void UpdateRabbitFemaleAmount()
    {
        rabbitFemaleValue.text = Mathf.RoundToInt(rabbitFemaleSlider.value).ToString();
    }

    public void UpdateFoxMaleAmount()
    {
        foxMaleValue.text = Mathf.RoundToInt(foxMaleSlider.value).ToString();
    }

    public void UpdateFoxFemaleAmount()
    {
        foxFemaleValue.text = Mathf.RoundToInt(foxFemaleSlider.value).ToString();
    }

    public void UpdateGrassAmount()
    {
        grassValue.text = Mathf.RoundToInt(grassSlider.value).ToString();
    }
}
