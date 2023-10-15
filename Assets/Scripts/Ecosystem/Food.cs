using UnityEngine;

public class Food : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float hungerRestore = 3f;
    
    public void Eat(Animal animal)
    {
        animal.RestoreHunger(hungerRestore);
        Destroy(gameObject);
    }
}
