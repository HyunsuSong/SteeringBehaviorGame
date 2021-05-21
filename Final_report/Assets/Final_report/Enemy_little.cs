using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_little : MonoBehaviour
{
    public GameObject EnemyPrefab_Kill_Player;
    public float delay = 2.0f;
    public float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delay)
        {
            var bullet = Instantiate(EnemyPrefab_Kill_Player, transform.position, Quaternion.identity).GetComponent<Bullet_Enemy>();

            bullet.Fire(transform.forward);

            timer = 0.0f;
        }
    }
}
