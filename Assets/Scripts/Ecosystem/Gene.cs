using UnityEngine;

[System.Serializable]
public class Gene
{
    public float value;
    public float mutationAmount;
    public float mutationChance;
    public float minValue;
    public float maxValue;

    public Gene(float value, float mutationAmount, float mutationChance, float minValue, float maxValue)
    {
        this.value = value;
        this.mutationAmount = mutationAmount;
        this.mutationChance = mutationChance;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public Gene Mutate()
    {
        float mutatedValue = value;

        if (Random.Range(0f, 1f) <= mutationChance)
            mutatedValue += Random.Range(-mutationAmount, mutationAmount);

        mutatedValue = Mathf.Clamp(mutatedValue, minValue, maxValue);

        return new Gene(mutatedValue, mutationAmount, mutationChance, minValue, maxValue);
    }
}
