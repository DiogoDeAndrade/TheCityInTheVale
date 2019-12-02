using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State { Movement, Command };

    public Camera       gameCamera;
    public LayerMask    interactionLayers;
    public float        interactionRadius = 10.0f;
    public float        interactionTolerance = 0.25f;
    public TextBox      lookBox;

    public State    state
    {
        get { return _state; }
        set
        {
            switch (value)
            {
                case State.Movement:
                    controller.mouseLookEnable = true;
                    controller.moveEnable = true;
                    break;
                case State.Command:
                    controller.mouseLookEnable = false;
                    controller.moveEnable = false;
                    break;
            }
        }
    }

    State           _state;
    FPSController   controller;

    private void Awake()
    {
        lookBox.gameCamera = gameCamera;
    }

    void Start()
    {
        controller = GetComponent<FPSController>();       
    }

    void Update()
    {
        if (_state == State.Movement)
        {
            RaycastHit[] hits = Physics.SphereCastAll(gameCamera.transform.position, interactionTolerance, gameCamera.transform.forward, interactionRadius, interactionLayers);

            foreach (var hit in hits)
            {
                var interactive = hit.collider.GetComponentInParent<InteractiveObject>();
                if (interactive)
                {
                    if (interactive.canLook)
                    {
                        lookBox.SetText(interactive.gameItem.onScreenDescription, interactive.transform);
                        break;
                    }
                }
            }
        }
    }
}
