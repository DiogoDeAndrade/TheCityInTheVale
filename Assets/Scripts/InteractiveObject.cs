using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameItem gameItem;
    public bool     canLook = true;

    Collider interactiveCollider;

    private void Start()
    {
        interactiveCollider = GetComponentInChildren<Collider>();
    }

    public Vector3 GetCenter()
    {
        if (interactiveCollider)
        {
            return interactiveCollider.bounds.center;
        }

        return transform.position;
    }
}
