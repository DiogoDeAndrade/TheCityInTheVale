using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteInEditMode]
public class Editor_PlaceOnTerrain : MonoBehaviour
{
    public bool     rotateToSurface;

    void Start()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (UnityEngine.Application.isPlaying) return;

        List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(new Ray(transform.position + Vector3.up * 10.0f, Vector3.down), 50.0f));

        hits.Sort((x, y) => (x.distance < y.distance)?(-1):((x.distance == y.distance)?(0):(1)));

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;
            if (hit.collider.transform.IsChildOf(transform)) continue;

            transform.position = hit.point;
            if (rotateToSurface) transform.up = hit.normal;

            break;
        }
#endif
    }
}
