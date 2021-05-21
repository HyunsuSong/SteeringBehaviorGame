using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_fallow : MonoBehaviour
{
    public Transform target;
    public float top_view = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, top_view, target.position.z);
        transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
    }
}
