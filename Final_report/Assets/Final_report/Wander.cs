using UnityEngine;

public class Wander : MonoBehaviour
{
    public float CircleRadius = 1;
    public float TurnChance = 0.05f;
    public float MaxRadius = 30;

    public float Mass = 15;
    public float MaxSpeed = 15;
    public float MaxForce = 15;

    private Vector3 velocity;
    private Vector3 wanderForce;

    [SerializeField]
    private Transform target = null;
    //private Vector3 target;

    private void Start()
    {
        velocity = Random.onUnitSphere;
        wanderForce = GetRandomWanderForce();
    }

    private void Update()
    {
        Vector3 desiredVelocity = GetWanderForce();
        desiredVelocity = desiredVelocity.normalized * MaxSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, MaxForce);
        steeringForce /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steeringForce, MaxSpeed);
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }

    private Vector3 GetWanderForce()
    {
        if (transform.position.magnitude > MaxRadius)
        {
            Vector3 directionToCenter = (target.position - transform.position).normalized;
            wanderForce = velocity.normalized + directionToCenter;
        }
        else if (Random.value < TurnChance)
        {
            wanderForce = GetRandomWanderForce();
        }

        wanderForce.y = 0;

        return wanderForce;
    }

    private Vector3 GetRandomWanderForce()
    {
        Vector3 circleCenter = velocity.normalized;
        Vector2 randomPoint = Random.insideUnitCircle;

        Vector3 displacement = new Vector3(randomPoint.x, randomPoint.y) * CircleRadius;
        displacement = Quaternion.LookRotation(velocity) * displacement;

        Vector3 wanderForce = circleCenter + displacement;

        wanderForce.y = 0;

        return wanderForce;
    }
}