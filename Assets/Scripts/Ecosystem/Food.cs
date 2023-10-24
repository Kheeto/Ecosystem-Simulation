using UnityEngine;

public class Food : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float hungerRestore = 3f;
    [SerializeField] private float regrowthTime = 10f;

    [Header("References")]
    [SerializeField] private GameObject meshHolder;

    public bool isGrown;

    private void Awake()
    {
        Regrow();
    }

    public void Eat(Animal animal)
    {
        isGrown = false;
        meshHolder.SetActive(false);
        Invoke(nameof(Regrow), regrowthTime);

        animal.RestoreHunger(hungerRestore);
    }

    private void Regrow()
    {
        meshHolder.SetActive(true);
        isGrown = true;
    }
}
