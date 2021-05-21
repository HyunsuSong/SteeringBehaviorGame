using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPursuit : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;

    public GameObject enemyPrefab;
    private float wall_speed = 0;

    public float _maxSpeed = 0.2f;
    public float _maxSpeed_offset = 1.0f;
    public float speed = 1.0f;
    public float Mass = 15;
    public float distance_wall = 3;
    public float flee_dis = 15.0f;
    public int hp = 1;
    public int count = 0;


    private bool _isPursuit = false;
    private bool _isClick = false;
    public bool _isEvade = false;
    public bool _isDead = false;

    public BoxCollider Boss_coll;

    private RaycastHit hit_4;
    private RaycastHit hit_5;

    public Vector3 _velocity = Vector3.zero;
    private Vector3 wall_velocity = Vector3.zero;

    private void Start()
    {
        Boss_coll = Boss_coll.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        count = enemyPrefab.GetComponent<Fire_Bullet>().count;

        if (count < 9)
        {
            hp = 15;
        }
        if (count >= 9)
        {
            _isEvade = true;
            _isPursuit = false;

            Boss_coll.center = new Vector3(0.0f, 1.0f, 0.5f);
            Boss_coll.size = new Vector3(2.0f, 4.0f, 3.0f);

            flee_dis = 20.0f;
        }

        if (hp == 0)
        {
            _isDead = true;

            Destroy(gameObject);
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

        Debug.DrawLine(transform.position, transform.position + raycast_forward.direction * distance_wall * 3, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_right.direction * hit_4.distance, Color.red);
        Debug.DrawLine(transform.position, transform.position + raycast_left.direction * hit_5.distance, Color.red);

        if (!_isClick)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isClick = true;
                _isPursuit = true;
            }
        }

        if (Physics.Raycast(raycast_right, out hit_4, hit_4.distance))
        {
            if (hit_4.collider.tag == "Wall" || hit_4.collider.tag == "Tower")
            {
                wall_speed = ((raycast_forward.origin + new Vector3(0.0f, 0.0f, distance_wall * 3)) - hit_4.transform.position).magnitude;

                wall_velocity = hit_4.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                _velocity = _velocity + wall_velocity;
            }
        }
        else if (Physics.Raycast(raycast_left, out hit_5, hit_5.distance))
        {
            if (hit_5.collider.tag == "Wall" || hit_5.collider.tag == "Tower")
            {
                wall_speed = ((raycast_forward.origin + new Vector3(0.0f, 0.0f, distance_wall * 3)) - hit_5.transform.position).magnitude;

                wall_velocity = hit_5.normal * wall_speed / Mass;

                wall_velocity.y = 0;

                _velocity = _velocity + wall_velocity;
            }
        }

        Vector3 targetVelocity = _target.position - transform.position;

        if (_isPursuit)
        {
            _velocity = _velocity + Pursuit(_target) * Time.deltaTime;

            if (targetVelocity.magnitude < 15.0f)
            {
                _velocity = Vector3.zero;
            }
        }

        else
        {
            _velocity = _velocity + Evade(_target) * Time.deltaTime;
        }

        if (_velocity != Vector3.zero)
        {
            transform.position += _velocity;
            transform.forward = _velocity.normalized;
        }

        _velocity.y = 0;
    }

    private Vector3 Pursuit(Transform target_agent)
    {
        return Arrive(target_agent.position);
    }

    private Vector3 Evade(Transform target_agent)
    {
        Vector3 targetVelocity = _target.position - transform.position;

        targetVelocity.y = 0.0f;

        float dist = targetVelocity.magnitude;

        if (dist < flee_dis)
            return Flee(target_agent.position);

        else
            return -_velocity;
    }

    private Vector3 Arrive(Vector3 target_pos)
    {
        Vector3 targetVelocity = target_pos - transform.position;

        targetVelocity.y = 0.0f;

        float dist = targetVelocity.magnitude;

        if (dist < 10.0f)
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

    private Vector3 Flee(Vector3 target_pos)
    {
        Vector3 dir = (transform.position - target_pos).normalized;

        if (dir.sqrMagnitude > 0.0f)
        {
            transform.forward = dir;
        }

        Vector3 desired_velocity = dir * _maxSpeed;

        return (desired_velocity - _velocity);
    }
}