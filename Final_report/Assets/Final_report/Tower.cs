using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab_Kill_Player;
    public float delay = 0.01f;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        transform.eulerAngles += new Vector3(0.0f, 0.5f, 0.0f);

        if (timer >= delay)
        {
            var bullet = Instantiate(bulletPrefab_Kill_Player, transform.position, Quaternion.identity).GetComponent<Bullet_Kill_Player>();

            bullet.Fire(transform.forward);

            timer = 0.0f;
        }
    }
}
