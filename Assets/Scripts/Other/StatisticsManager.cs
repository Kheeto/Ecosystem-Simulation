using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameCamera gameCamera;

    [Header("References")]
    [SerializeField] TMP_Text title;
    [SerializeField] GameObject background;
    [SerializeField] GameObject buttonsHolder;
    [SerializeField] GameObject populationGraph;
    [SerializeField] GameObject speedGraph;
    [SerializeField] GameObject spotRangeGraph;
    [SerializeField] GameObject growthDurationGraph;
    [SerializeField] GameObject gestationDurationGraph;
    [SerializeField] GameObject reproductiveUrgeGraph;

    private void Update()
    {
        if (gameManager.HasGameStarted() && Input.GetKeyDown(KeyCode.Tab))
        {
            if (background.activeSelf)
            {
                title.gameObject.SetActive(false);
                background.SetActive(false);
                buttonsHolder.SetActive(false);
                HideAllGraphs();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameCamera.EnableCamera(true);
            }
            else
            {
                title.gameObject.SetActive(true);
                background.SetActive(true);
                buttonsHolder.SetActive(true);
                ShowPopulationGraph();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameCamera.EnableCamera(false);
            }
        }
    }

    public void ShowPopulationGraph()
    {
        HideAllGraphs();
        populationGraph.SetActive(true);
        title.text = "Population";
    }

    public void ShowSpeedGraph()
    {
        HideAllGraphs();
        speedGraph.SetActive(true);
        title.text = "Speed";
    }

    public void ShowSpotRangeGraph()
    {
        HideAllGraphs();
        spotRangeGraph.SetActive(true);
        title.text = "Spot Range";
    }

    public void ShowGrowthDurationGraph()
    {
        HideAllGraphs();
        growthDurationGraph.SetActive(true);
        title.text = "Growth Duration";
    }

    public void ShowGestationDurationGraph()
    {
        HideAllGraphs();
        gestationDurationGraph.SetActive(true);
        title.text = "Gestation Duration";
    }

    public void ShowReproductiveUrgeGraph()
    {
        HideAllGraphs();
        reproductiveUrgeGraph.SetActive(true);
        title.text = "Reproductive Urge";
    }

    private void HideAllGraphs()
    {
        populationGraph.SetActive(false);
        speedGraph.SetActive(false);
        spotRangeGraph.SetActive(false);
        growthDurationGraph.SetActive(false);
        gestationDurationGraph.SetActive(false);
        reproductiveUrgeGraph.SetActive(false);
    }

    public void ResetStatistics()
    {
        populationGraph.GetComponentInParent<Graph>().ResetGraph();
        speedGraph.GetComponentInParent<Graph>().ResetGraph();
        spotRangeGraph.GetComponentInParent<Graph>().ResetGraph();
        growthDurationGraph.GetComponentInParent<Graph>().ResetGraph();
        gestationDurationGraph.GetComponentInParent<Graph>().ResetGraph();
        reproductiveUrgeGraph.GetComponentInParent<Graph>().ResetGraph();
    }
}
