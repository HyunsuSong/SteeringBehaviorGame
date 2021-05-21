using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isFire;
    private Vector3 direction;
    public float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isFire)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var MobPrefab = collision.collider.GetComponent<Wall_avoidance>();
        var EnemyPrefab = collision.collider.GetComponent<offsetPursuit>();

        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Monster"))
        {
            if (MobPrefab != null)
            {
                MobPrefab.hp -= 1;
            }
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Enemy"))
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
        direction = dir;
        isFire = true;

        Destroy(gameObject, 5f);
    }
}
