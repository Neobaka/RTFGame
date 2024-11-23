using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType //Типы врагов
    {
        Normal,
        Fast,
        Tank,
        Flying
    }


    public EnemyType type;
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private int goldReward = 50;


    private float speed;
    private float health;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;



    public void Initialize(Transform[] points)
    {
        waypoints = points;
        transform.position = waypoints[0].position;

        // Настройка характеристик в зависимости от типа врага
        switch (type)
        {
            case EnemyType.Fast:
                speed = baseSpeed * 2f;
                health = baseHealth * 0.5f;
                break;
            case EnemyType.Tank:
                speed = baseSpeed * 0.5f;
                health = baseHealth * 2f;
                break;
            case EnemyType.Flying:
                speed = baseSpeed * 1.5f;
                health = baseHealth * 0.7f;
                break;
            default:
                speed = baseSpeed;
                health = baseHealth;
                break;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            ReachEnd();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }

    private void ReachEnd()
    {
        GameManager.Instance.ReduceHealth();
        Destroy(gameObject);
    }
}