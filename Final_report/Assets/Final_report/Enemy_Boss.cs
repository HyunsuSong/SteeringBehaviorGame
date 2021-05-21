using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    public GameObject EnemyPrefab_Kill_Player;
    public GameObject Enemy_Boss_Stop;
    public float delay = 2.0f;
    public float timer;

    public bool isEvade;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isEvade = Enemy_Boss_Stop.GetComponent<AgentPursuit>()._isEvade;

        timer += Time.deltaTime;

        if (!isEvade)
        {
            if (timer >= delay)
            {
                var bullet = Instantiate(EnemyPrefab_Kill_Player, transform.position, Quaternion.identity).GetComponent<Bullet_Enemy>();

                bullet.Fire(transform.forward);

                timer = 0.0f;
            }
        }
    }
}
