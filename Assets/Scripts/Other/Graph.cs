using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Color rabbitColor;
    [SerializeField] private Color foxColor;
    [SerializeField] private Color plantColor;
    [SerializeField] private float yMaximum = 10f;
    [SerializeField] private float xSize = 10f;
    [SerializeField] private float updateInterval = 10f;
    [SerializeField] RectTransform graphContainer;

    private int amount;

    private GameObject previousRabbit;
    private GameObject previousFox;
    private GameObject previousPlant;

    private float foxStatistic;
    private float rabbitStatistic;
    private float plantAmount;

    [SerializeField] Statistic statistic;

    private enum Statistic
    {
        Population,
        Speed,
        SpotRange,
        GrowthDuration,
        GestationDuration,
        ReproductiveUrge
    }

    private void Awake()
    {
        amount = 0;
        previousRabbit = null;
        previousFox = null;
        previousPlant = null;
    }

    public void StartGraph()
    {
        InvokeRepeating(nameof(UpdateGraph), 1f, updateInterval);
    }

    private void UpdateGraph()
    {
        UpdateStatistics();

        AddValue(rabbitStatistic, EntityType.Rabbit);
        AddValue(foxStatistic, EntityType.Fox);

        if (statistic == Statistic.Population)
            AddValue(plantAmount, EntityType.Plant);

        amount++;
    }

    private void AddValue(float value, EntityType type)
    {
        float graphHeight = graphContainer.sizeDelta.y;

        float xPosition = xSize + amount * xSize;
        float yPosition = (value / yMaximum) * graphHeight;

        GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), type);

        if (type == EntityType.Rabbit)
        {
            if (previousRabbit != null)
            {
                CreateDotConnection(previousRabbit.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition, type);
            }
            previousRabbit = circleGameObject;
        }
        else if (type == EntityType.Fox)
        {
            if (previousFox != null)
            {
                CreateDotConnection(previousFox.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition, type);
            }
            previousFox = circleGameObject;
        }
        else if (type == EntityType.Plant)
        {
            if (previousPlant != null)
            {
                CreateDotConnection(previousPlant.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition, type);
            }
            previousPlant = circleGameObject;
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, EntityType type)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        if (type == EntityType.Rabbit)
            gameObject.GetComponent<Image>().color = rabbitColor;
        else if (type == EntityType.Fox)
            gameObject.GetComponent<Image>().color = foxColor;
        else if (type == EntityType.Plant)
            gameObject.GetComponent<Image>().color = plantColor;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3, 3);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, EntityType type)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        if (type == EntityType.Rabbit)
            gameObject.GetComponent<Image>().color = rabbitColor;
        else if (type == EntityType.Fox)
            gameObject.GetComponent<Image>().color = foxColor;
        else if (type == EntityType.Plant)
            gameObject.GetComponent<Image>().color = plantColor;

        gameObject.transform.SetParent(graphContainer, false);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    private void UpdateStatistics()
    {
        float rabbitTotal = 0f, foxTotal = 0f;
        int rabbitCount = 0, foxCount = 0;

        foreach (Animal animal in FindObjectsOfType<Animal>())
        {
            if (animal.type == EntityType.Rabbit)
            {
                if (statistic == Statistic.Population) rabbitTotal++;
                else if (statistic == Statistic.Speed) rabbitTotal += animal.speed.value;
                else if (statistic == Statistic.SpotRange) rabbitTotal += animal.spotRange.value;
                else if (statistic == Statistic.GrowthDuration) rabbitTotal += animal.growthTime.value;
                else if (statistic == Statistic.GestationDuration)
                {
                    if (animal is AnimalFemale) rabbitTotal += ((AnimalFemale)animal).gestationTime.value;
                }
                else if (statistic == Statistic.ReproductiveUrge)
                {
                    if (animal is AnimalMale) rabbitTotal += ((AnimalMale)animal).reproductionUrge.value;
                }

                rabbitCount++;
            }
            else
            {
                if (statistic == Statistic.Population) foxTotal++;
                else if (statistic == Statistic.Speed) foxTotal += animal.speed.value;
                else if (statistic == Statistic.SpotRange) foxTotal += animal.spotRange.value;
                else if (statistic == Statistic.GrowthDuration) foxTotal += animal.growthTime.value;
                else if (statistic == Statistic.GestationDuration)
                {
                    if (animal is AnimalFemale) foxTotal += ((AnimalFemale)animal).gestationTime.value;
                }
                else if (statistic == Statistic.ReproductiveUrge)
                {
                    if (animal is AnimalMale) foxTotal += ((AnimalMale)animal).reproductionUrge.value;
                }

                foxCount++;
            }
        }

        if (statistic == Statistic.Population)
        {
            rabbitCount = 1;
            foxCount = 1;
        }

        rabbitStatistic = rabbitTotal / rabbitCount;
        foxStatistic = foxTotal / foxCount;

        if (statistic == Statistic.Population)
        {
            plantAmount = 0f;
            foreach (Food food in FindObjectsOfType<Food>())
            {
                // Food is a plant, not an animal
                if (food.GetComponent<AnimalMale>() == null && food.GetComponent<AnimalFemale>() == null && food.isGrown)
                {
                    plantAmount++;
                }
            }
        }
    }

    public void ResetGraph()
    {
        amount = 0;
        foreach (Transform t in graphContainer)
        {
            Destroy(t.gameObject);
        }
    }
}
