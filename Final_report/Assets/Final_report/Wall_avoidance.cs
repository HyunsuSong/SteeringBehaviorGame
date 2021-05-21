using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall_avoidance : MonoBehaviour
{
    public float CircleRadius = 1;
    public float TurnChance = 0.05f;
    public float MaxRadius = 30;

    public float wall_speed = 0;
    public float Mass = 15;
    public float MaxSpeed = 15;
    public float Speed = 15;
    public float MaxForce = 15;
    public float distance_wall = 3;

    public int hp = 5;

    public LayerMask m_layerMask = -1;

    private Vector3 velocity = Vector3.zero;
    private Vector3 wanderForce = Vector3.zero;
    private Vector3 wall_velocity = Vector3.zero;

    private RaycastHit hit_4;
    private RaycastHit hit_5;

    [SerializeField]
    private Transform target = null;

    public GameObject FirePrefab;

    private void Start()
    {
        velocity = Random.onUnitSphere;
        wanderForce = GetRandomWanderForce();
    }

    private void Update()
    {
        if(hp==0)
        {
            Destroy(gameObject);

            FirePrefab.GetComponent<Fire_Bullet>().count++;
        }

        hit_4.distance = distance_wall;
        hit_5.distance = distance_wall;

        Ray raycast_forward = new Ray();
        Ray raycast_right = new Ray();
        Ray raycast_left = new Ray();

        raycast_forward.origin = transform.position;
        raycast_right.origin = transform.position;
        raycast_left.origin = transform.position;

        raycast_forward.direction = transform.forward;
        raycast_right.direction = transform.forward + transform.right;
        raycast_left.direction = transform.forward - transform.right;

        Debug.DrawLine(transform.position, transform.position + raycast_forward.direction * distance_wall * 2, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_right.direction * hit_4.distance, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_left.direction * hit_5.distance, Color.red);

        if (Physics.Raycast(raycast_right, out hit_4, hit_4.distance))
        {
            if (hit_4.collider.tag == "Wall" || hit_4.collider.tag == "Tower")
            {
                wall_speed = ((raycast_forward.origin + new Vector3(0.0f, 0.0f, distance_wall * 2)) - hit_4.transform.position).magnitude;

                wall_velocity = hit_4.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                velocity = velocity + wall_velocity;
            }
        }
        else if (Physics.Raycast(raycast_left, out hit_5, hit_5.distance))
        {
            if (hit_5.collider.tag == "Wall" || hit_5.collider.tag == "Tower")
            {
                wall_speed = ((raycast_forward.origin + new Vector3(0.0f, 0.0f, distance_wall * 2)) - hit_5.transform.position).magnitude;

                wall_velocity = hit_5.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                velocity = velocity + wall_velocity;
            }
        }

        Vector3 desiredVelocity = GetWanderForce();
        desiredVelocity = desiredVelocity.normalized * MaxSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, MaxForce);
        steeringForce /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steeringForce, MaxSpeed);

        transform.position += velocity;
        transform.forward = velocity.normalized;

        velocity.y = 0;
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