using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EntityType
{
    Rabbit,
    Fox,
    Plant
}

public class Animal : MonoBehaviour {

    public EntityType type;

    [Header("Stats")]
    public Gene speed;

    [Header("Needs")]
    public Need currentNeed;

    [Range(0f, 10f)]
    public float hunger = 0f;
    [Tooltip("Animal will not search for food below this hunger level")]
    [SerializeField] protected float minimumHunger = 0f;
    [Tooltip("Animal will not flee above this hunger level, they will search for food")]
    [SerializeField] protected float maxFleeHunger = 0f;

    [Tooltip("How much the Hunger need will increase per second")]
    [SerializeField] protected float hungerIncrease = .3f;
    [Tooltip("How much the speed value will affect the hunger increase")]
    [SerializeField] protected float speedHungerIncrease = .1f;


    [Header("Vision")]
    public Gene spotRange;
    [SerializeField] private float eatRange = 2f;
    [SerializeField] protected LayerMask whatIsFood;
    [SerializeField] protected LayerMask whatIsAnimal;
    [SerializeField] protected LayerMask whatIsPredator;

    [Header("Growth")]
    public Gene growthTime;
    [Tooltip("The needed total of growth time + gestation time, if an animals takes shorter to grow up it has a chance of dying")]
    [SerializeField] private float growthRequirement = 20f;
    public float growthProgress = 0f;
    public bool isChild;
    [SerializeField] private float agingTime = 60f;

    [Header("References")]
    [SerializeField] protected NavMeshAgent agent;
    public GameObject canvas;
    [SerializeField] GameObject offspringInfoUI;
    [SerializeField] GameObject adultInfoUI;

    public enum Need
    {
        Food,
        Reproduction,
        Exploration,
        Flee
    }

    private void Awake()
    {
        UpdateSpeed();
        StartCoroutine(Aging());
    }

    protected virtual void Update()
    {
        if (isChild) growthProgress += Time.deltaTime;

        HandleNeeds();

        switch (currentNeed)
        {
            case Need.Food:
                HandleHunger();
                break;
            case Need.Exploration:
                Explore();
                break;
            case Need.Flee:
                break;
        }
    }

    protected virtual void HandleNeeds()
    {
        if (hunger > minimumHunger)
            currentNeed = Need.Food;
        else
            currentNeed = Need.Exploration;

        hunger += (hungerIncrease + (speed.value * speedHungerIncrease)) * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 10f);

        if (hunger == 10f) Die("Hunger");

        if (!agent.hasPath) currentNeed = Need.Exploration;

        if (hunger < maxFleeHunger) Flee();
    }

    protected virtual void HandleHunger()
    {
        if (hunger == 0f) return;

        if (type == EntityType.Fox) Debug.Log("FOX SEARCHES FOOD");

        Collider[] foods = Physics.OverlapSphere(transform.position, eatRange, whatIsFood);
        foreach (Collider food in foods)
        {
            Food f = food.GetComponentInParent<Food>();
            if (f != null && f.isGrown) f.Eat(this);
        }

        foods = Physics.OverlapSphere(transform.position, spotRange.value, whatIsFood);
        Transform closest = null;

        foreach (Collider food in foods)
        {
            if (closest == null)
            {
                if (food.GetComponentInParent<Food>().isGrown) closest = food.transform;
                continue;
            }
            if (food.GetComponentInParent<Food>().isGrown &&
                Vector3.Distance(transform.position, food.transform.position)
                < Vector3.Distance(transform.position, closest.position))
                closest = food.transform;
        }

        if (closest != null) agent.SetDestination(closest.position + Random.insideUnitSphere);
    }

    protected virtual void Flee()
    {
        // Check predators
        Collider[] predators = Physics.OverlapSphere(transform.position, spotRange.value, whatIsPredator);

        if (predators.Length == 0) return;

        // Calculate the average direction predators are coming from
        Vector3 predatorsDirection = Vector3.zero;
        foreach (Collider predator in predators)
        {
            predatorsDirection += (predator.transform.position - transform.position).normalized;
        }

        Vector3 fleeDirection = -predatorsDirection;

        agent.SetDestination(transform.position + fleeDirection);
        currentNeed = Need.Flee;
    }

    protected virtual void Explore()
    {
        if (!agent.hasPath)
        {
            Vector3 randomPoint;
            if (NavMeshUtility.RandomPoint(transform.position, spotRange.value, out randomPoint))
            {
                if (!agent.isOnNavMesh)
                {
                    agent.Warp(randomPoint);
                }

                agent.SetDestination(randomPoint);
            }
        }
    }

    public void RestoreHunger(float amount)
    {
        hunger -= amount;
        hunger = Mathf.Clamp(hunger, 0f, 10f);
    }

    private void Die (string reason = "unknown reason")
    {
        Debug.Log("Animal died from " + reason);
        Destroy(gameObject);
    }

    public void UpdateSpeed()
    {
        agent.speed = speed.value;
    }

    public void SetChild(float gestationTime)
    {
        isChild = true;
        StartCoroutine(Grow(gestationTime));
    }

    private IEnumerator Grow(float gestationTime)
    {
        offspringInfoUI.SetActive(true);
        adultInfoUI.SetActive(false);

        yield return new WaitForSeconds(growthTime.value);

        isChild = false;
        offspringInfoUI.SetActive(false);
        adultInfoUI.SetActive(true);

        // The more underdeveloped the offspring is, the greater chance of dying during growth
        float developmentAmount = (gestationTime + growthTime.value) / growthRequirement;
        if (Random.Range(0f, 1f) > developmentAmount)
        {
            Die("Underdevelopment");
        }
        else
            Debug.Log("Animal has grown");
    }

    private IEnumerator Aging()
    {
        yield return new WaitForSeconds(agingTime);
        Die("Aging");
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, spotRange.value);

        Gizmos.DrawLine(transform.position, agent.destination);
    }
}
