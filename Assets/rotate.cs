using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float speed;
    public float plusCos = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotateVal = (Mathf.Cos(Time.time) + 1 * plusCos) * speed;
        transform.Rotate(rotateVal * Time.deltaTime, -rotateVal * Time.deltaTime, rotateVal * Time.deltaTime);
    }
}
