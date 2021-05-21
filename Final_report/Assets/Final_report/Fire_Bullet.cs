using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Bullet : MonoBehaviour
{
    private float delay = 0.1f;
    private float timer_bullet;

    public int count;

    public GameObject bulletPrefab;

    private void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer_bullet += Time.deltaTime;

        if (Input.GetKeyDown("`"))
        {
            if (timer_bullet >= delay)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();

                bullet.Fire(transform.forward);
                timer_bullet = 0.0f;
            }
        }
    }
}
