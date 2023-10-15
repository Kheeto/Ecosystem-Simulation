using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private Status status;
    [SerializeField] private Need currentNeed;
    [Range(0f, 10f)] [SerializeField] private float hunger = 0f;
    [Range(0f, 10f)] [SerializeField] private float thirst = 0f;
    [Range(0f, 10f)] [SerializeField] private float reproduction = 0f;

    [Header("Vision")]
    [SerializeField] private float spotRange = 15f;
    [SerializeField] private float eatRange = 2f;
    [SerializeField] private LayerMask whatIsFood;

    [Header("Needs")]
    [SerializeField] private float hungerIncrease = .5f;
    [SerializeField] private float thirstIncrease = .5f;
    [SerializeField] private float reproductionIncrease = .5f;

    [Header("Child")]
    [SerializeField] private bool isChild;
    [SerializeField] private Animal parent;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;

    public enum Need
    {
        Food,
        Water,
        Reproduction
    }

    public enum Status
    {
        Idle,
        SearchingWater,
        Drinking,
        SearchingFood,
        Eating,
        SearchingPartner,
        Reproducing,
        FollowingParent
    }

    private void Update()
    {
        float strongestNeed = hunger;
        currentNeed = Need.Food;

        // Check if other needs are greater than the need for food
        if (thirst > strongestNeed)
        {
            strongestNeed = thirst;
            currentNeed = Need.Water;
        }
        if (reproduction > strongestNeed)
            currentNeed = Need.Reproduction;

        switch (currentNeed)
        {
            case Need.Food:
                HandleFood();
                break;
            case Need.Water:
                HandleWater();
                break;
            case Need.Reproduction:
                HandleReproduction();
                break;
        }

        hunger += hungerIncrease * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 10f);
        thirst += thirstIncrease * Time.deltaTime;
        thirst = Mathf.Clamp(thirst, 0f, 10f);
        reproduction += reproductionIncrease * Time.deltaTime;
        reproduction = Mathf.Clamp(reproduction, 0f, 10f);

        if (hunger == 10f || thirst == 10f) Die();
    }

    private void HandleFood()
    {
        Collider[] food = Physics.OverlapSphere(transform.position, eatRange, whatIsFood);
        if (food.Length > 0)
        {
            food[0].GetComponentInParent<Food>()?.Eat(this);
        }

        food = Physics.OverlapSphere(transform.position, spotRange, whatIsFood);
        if (food.Length > 0)
        {
            // Find the closest food
            Vector3 closestPosition = food[0].transform.position;
            for (int i = 1; i < food.Length; i++)
            {
                if (Vector3.Distance(transform.position, food[i].transform.position)
                    < Vector3.Distance(transform.position, closestPosition))
                    closestPosition = food[i].transform.position;
            }

            agent.SetDestination(closestPosition);
        }
    }

    private void HandleWater()
    {
        // TODO search water and drink
    }

    private void HandleReproduction()
    {
        // TODO search partner and mate
    }

    public void RestoreHunger(float amount)
    {
        hunger -= amount;
        hunger = Mathf.Clamp(hunger, 0f, 10f);
    }

    public void SetParent(Animal parent)
    {
        this.parent = parent;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, spotRange);
    }
}
