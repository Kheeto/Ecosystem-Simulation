using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour {

    [Header("Stats")]
    public Gene speed;

    [Header("Needs")]
    [SerializeField] protected Need currentNeed;

    [Range(0f, 10f)]
    [SerializeField] protected float hunger = 0f;
    [Tooltip("Animal will not search for food below this hunger level")]
    [SerializeField] protected float minimumHunger = 0f;
    [Range(0f, 10f)]
    [SerializeField] protected float thirst = 0f;

    [Tooltip("How much the Hunger need will increase per second")]
    [SerializeField] protected float hungerIncrease = .5f;
    [Tooltip("How much the Thirst need will increase per second")]
    [SerializeField] protected float thirstIncrease = .5f;

    [Header("Vision")]
    public Gene spotRange;
    [SerializeField] private float eatRange = 2f;
    [SerializeField] protected LayerMask whatIsFood;
    [SerializeField] protected LayerMask whatIsAnimal;

    [Header("Growth")]
    public Gene growthTime;
    public bool isChild;
    [SerializeField] private float agingTime = 60f;

    [Header("References")]
    [SerializeField] protected NavMeshAgent agent;

    public enum Need
    {
        Food,
        Water,
        Reproduction,
        Exploration
    }

    private void Awake()
    {
        UpdateSpeed();
        Invoke(nameof(Die), agingTime);
    }

    protected virtual void Update()
    {
        HandleNeeds();

        switch (currentNeed)
        {
            case Need.Food:
                HandleHunger();
                break;
            case Need.Water:
                HandleThirst();
                break;
            case Need.Exploration:
                Explore();
                break;
        }
    }

    protected virtual void HandleNeeds()
    {
        if (hunger > minimumHunger)
            currentNeed = Need.Food;
        else if (thirst > hunger)
            currentNeed = Need.Water;
        else
            currentNeed = Need.Exploration;

        hunger += hungerIncrease * speed.value * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 10f);
        thirst += thirstIncrease * speed.value * Time.deltaTime;
        thirst = Mathf.Clamp(thirst, 0f, 10f);

        if (hunger == 10f || thirst == 10f)
        {
            Debug.Log("Animal died from hunger or thirst");
            Die();
        }

        if (!agent.hasPath) currentNeed = Need.Exploration;
    }

    protected virtual void HandleHunger()
    {
        if (hunger == 0f) return;

        Collider[] food = Physics.OverlapSphere(transform.position, eatRange, whatIsFood);
        if (food.Length > 0)
        {
            food[0].GetComponentInParent<Food>()?.Eat(this);
        }

        food = Physics.OverlapSphere(transform.position, spotRange.value, whatIsFood);
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

            agent.SetDestination(closestPosition + Random.insideUnitSphere);
        }
    }

    protected virtual void HandleThirst()
    {

    }

    protected virtual void Explore()
    {
        if (!agent.hasPath)
        {
            Vector3 randomPoint;
            if (NavMeshUtility.RandomPoint(transform.position, spotRange.value, out randomPoint))
            {
                agent.SetDestination(randomPoint);
            }
        }
    }

    public void RestoreHunger(float amount)
    {
        hunger -= amount;
        hunger = Mathf.Clamp(hunger, 0f, 10f);
    }

    private void Die()
    {
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
        yield return new WaitForSeconds(growthTime.value);

        isChild = false;

        // The more underdeveloped the offspring is, the greater chance of dying during growth
        float developmentAmount = (gestationTime + growthTime.value) / 20f;
        if (Random.Range(0f, 1f) > developmentAmount)
        {
            Debug.Log("Animal died from underdevelopment");
            Die();
        }
        else
            Debug.Log("Animal has grown");
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
