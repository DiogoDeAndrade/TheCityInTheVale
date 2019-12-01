using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, Time.deltaTime * 90, 0.0f);        
    }
}
