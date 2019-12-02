using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public enum State { Movement, Command };

    public Camera           gameCamera;
    public LayerMask        interactionLayers;
    public float            interactionRadius = 10.0f;
    public float            interactionTolerance = 0.25f;
    public TextBox          lookBox;
    public TMP_InputField   inputField;
    public bool             consecutiveInputs = false;

    float timeOfLastInput = 0;

    public State    state
    {
        get { return _state; }
        set
        {
            switch (value)
            {
                case State.Movement:
                    timeOfLastInput = Time.time;
                    controller.mouseLookEnable = true;
                    controller.moveEnable = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    inputField.DeactivateInputField(true);
                    inputField.gameObject.SetActive(false);
                    break;
                case State.Command:
                    controller.mouseLookEnable = false;
                    controller.moveEnable = false;
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                    inputField.gameObject.SetActive(true);
                    inputField.ActivateInputField();
                    inputField.text = "";
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
        state = State.Movement;
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

            if ((Time.time - timeOfLastInput) > 1.0f)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = State.Command;
                }
            }
        }
        if (_state == State.Command)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = State.Movement;
            }
        }
    }

    public void OnCommand()
    {
        string command = inputField.text;

        command = command.Trim();

        if (command == "")
        {
            state = State.Movement;
            return;
        }

        Debug.Log("Command = " + command);

        if (!consecutiveInputs)
        {
            state = State.Movement;
        }
    }

    public void OnCancelCommand()
    {
        state = State.Movement;
    }
}
