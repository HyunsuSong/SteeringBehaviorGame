using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offsetPursuit : MonoBehaviour
{

    public Transform target = null;
    public GameObject target_GameObject = null;
    public GameObject enemyPrefab;
    public BoxCollider Little_coll;

    public float _maxSpeed_offset = 1.0f;
    public float speed = 1.0f;
    public float distance_wall = 5.0f;
    public float wall_speed = 0;
    public float Mass = 15;
    public int count = 0;
    public int hp = 10;
    public Vector3 offsetPos_to_target = new Vector3(0.0f, 0.0f, 0.0f);

    private float target_speed = 0.0f;
    private bool No_target = false;

    public Vector3 _velocity = Vector3.zero;
    private Vector3 target_velocity = Vector3.zero;
    private Vector3 offsetDis_from_target = Vector3.zero;
    private Vector3 targetLength = Vector3.zero;
    private Vector3 offsetPos = Vector3.zero;
    private Vector3 wall_velocity = Vector3.zero;

    private RaycastHit hit_3;
    private RaycastHit hit_4;
    private RaycastHit hit_5;

    private float LookAheadTime = 0.0f;

    private void Start()
    {
        target_speed = target_GameObject.GetComponent<AgentPursuit>()._maxSpeed;
        target_velocity = target_GameObject.GetComponent<AgentPursuit>()._velocity;
        Little_coll = Little_coll.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        No_target = target_GameObject.GetComponent<AgentPursuit>()._isDead;

        count = enemyPrefab.GetComponent<Fire_Bullet>().count;

        if (count < 5)
        {
            hp = 10;
        }

        if (count >= 5)
        {
            Little_coll.center = new Vector3(0.0f, 0.0f, 0.5f);
            Little_coll.size = new Vector3(2.0f, 0.0f, 3.0f);
        }

        if (hp == 0)
        {
            Destroy(gameObject);

            enemyPrefab.GetComponent<Fire_Bullet>().count++;
        }

        if (No_target)
        {
            Destroy(gameObject);
        }

        offsetPos = target.TransformPoint(offsetPos_to_target);

        offsetDis_from_target = offsetPos - transform.position;           //무조건맞음

        LookAheadTime = offsetDis_from_target.magnitude / (target_speed + _maxSpeed_offset);            //무조건맞음

        _velocity = _velocity + (Arrive(offsetPos + (target_velocity * LookAheadTime)) * Time.deltaTime);

        Wall_Avoidance();

        targetLength = offsetPos - transform.position;

        if (targetLength.magnitude < 1.0f)
        {
            _velocity = Vector3.zero;
        }
        else
        {
            _velocity.y = 0;
            transform.position = transform.position + _velocity;
            transform.forward = _velocity.normalized;
        }
    }

    private Vector3 Wall_Avoidance()
    {
        hit_3.distance = distance_wall;
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

        Debug.DrawLine(transform.position, transform.position + raycast_forward.direction * hit_3.distance * 3, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_right.direction * hit_4.distance, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_left.direction * hit_5.distance, Color.red);

        if (Physics.Raycast(raycast_right, out hit_4, hit_4.distance))
        {
            if (hit_4.collider.tag == "Wall" || hit_4.collider.tag == "Tower")
            {
                if (Physics.Raycast(raycast_forward, out hit_3, hit_3.distance * 3))
                    wall_speed = ((raycast_forward.direction * hit_3.distance * 3) - hit_3.transform.position).magnitude;

                wall_velocity = hit_4.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                _velocity = _velocity + wall_velocity;
            }
        }
        else if (Physics.Raycast(raycast_left, out hit_5, hit_5.distance))
        {
            if (hit_5.collider.tag == "Wall" || hit_5.collider.tag == "Tower")
            {
                if (Physics.Raycast(raycast_forward, out hit_3, hit_3.distance * 3))
                    wall_speed = ((raycast_forward.direction * hit_3.distance * 3) - hit_3.transform.position).magnitude;

                wall_velocity = hit_5.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                _velocity = _velocity + wall_velocity;
            }
        }

        return _velocity;
    }

    private Vector3 Arrive(Vector3 target_pos)
    {
        Vector3 targetVelocity = target_pos - transform.position;

        targetVelocity.y = 0.0f;

        float dist = targetVelocity.magnitude;

        if (dist < 1.0f)
        {
            _velocity = Vector3.zero;
            return _velocity;
        }

        if (dist > 40.0f)
        {
            speed = _maxSpeed_offset;
        }
        else
        {
            speed = _maxSpeed_offset * (dist / 40.0f);
        }

        targetVelocity.Normalize();
        targetVelocity *= speed;

        Vector3 acceleration = targetVelocity - _velocity;

        acceleration *= 1 / 0.1f;

        if (acceleration.magnitude > 15.0f)
        {
            acceleration.Normalize();
            acceleration *= 5.0f;
        }

        speed = Mathf.Min(speed, _maxSpeed_offset);

        return acceleration;
    }
}


