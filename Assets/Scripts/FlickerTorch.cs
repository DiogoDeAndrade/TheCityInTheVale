using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerTorch : MonoBehaviour
{
    public Light        torchLight;
    public Gradient     lightColor;
    public float        lightFlickerSpeed = 0.5f;
    public float        amplitude = 5.0f;
    public float        frequency = 0.5f;
    public Transform    baseTorch;
    
    float x = 0.0f;
    float y = 0.0f;
    float baseAngle = 0.0f;

    void Start()
    {
        baseAngle = baseTorch.rotation.eulerAngles.z;
    }

    void Update()
    {
        x += Time.deltaTime * lightFlickerSpeed;
        y += Time.deltaTime * frequency;

        if (torchLight)
        {
            torchLight.color = lightColor.Evaluate(Mathf.PerlinNoise(x, 0));
        }

        Vector3 e = baseTorch.rotation.eulerAngles;
        e.z = baseAngle + amplitude * Mathf.Sin(y);
        baseTorch.rotation = Quaternion.Euler(e);
    }
}
