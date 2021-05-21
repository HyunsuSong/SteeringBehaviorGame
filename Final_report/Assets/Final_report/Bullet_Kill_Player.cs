using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Kill_Player : MonoBehaviour
{
    private bool isFire;
    public bool isKilled = false;
    private Vector3 direction_forward;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(direction_forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var PlayerPrefab = collision.collider.GetComponent<Agent>();
        var EnemyPrefab = collision.collider.GetComponent<AgentPursuit>();

        if(collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Character"))
        {
            if (PlayerPrefab != null)
            {
                PlayerPrefab.player_Hp -= 1;
            }
            Destroy(gameObject);
        }
        if(collision.collider.CompareTag("EnemyBoss"))
        {
            if (EnemyPrefab != null)
            {
                EnemyPrefab.hp -= 1;
            }
            Destroy(gameObject);
        }
    }

    public void Fire(Vector3 dir)
    {
        direction_forward = dir;
        isFire = true;

        Destroy(gameObject, 5f);
    }
}
