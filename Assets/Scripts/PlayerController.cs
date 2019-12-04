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
    public TextBox          outputWindow;
    public GameState        gameState;

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
            _state = value;
        }
    }

    State           _state;
    FPSController   controller;

    private void Awake()
    {
        lookBox.gameCamera = gameCamera;
        if (gameState == null)
        {
            gameState = new GameState();
        }
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

        command = command.ToLower().Trim();

        if (command == "")
        {
            state = State.Movement;
            return;
        }

        command = command.Replace("at", "");
        command = command.Replace("on", "");
        command = command.Replace("the", "");
        command = command.Replace("in", "");

        string[] commandString = command.Split(' ', '\t');

        if (commandString.Length > 0)
        {            
            string verb = GetStdVerb(commandString[0]);
            bool   processed = false;

            List<GameAction> possibleActions = new List<GameAction>();
            GetAllPossibleActions(possibleActions);

            foreach (var action in possibleActions)
            {
                if (action.GetVerb() == verb)
                {
                    if (action.IsValidAction(gameState, commandString, 1))
                    {
                        processed = action.RunAction(gameState);
                        if (processed) break;
                    }
                }
            }

            if (!processed)
            {
                string error = GetErrorMessage(commandString);

                outputWindow.AddText(error);
            }
        }

        if (!consecutiveInputs)
        {
            state = State.Movement;
        }
        else
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    public void OnCancelCommand()
    {
        state = State.Movement;
    }

    void GetAllPossibleActions(List<GameAction> possibleActions)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayers);

        foreach (var c in colliders)
        {
            InteractiveObject interactiveObject = c.GetComponentInChildren<InteractiveObject>();
            if (interactiveObject == null) interactiveObject = c.GetComponentInParent<InteractiveObject>();

            if (interactiveObject != null)
            {
                // Get all possible actions
                GameAction[] actions = interactiveObject.GetComponentsInChildren<GameAction>();
                foreach (var a in actions)
                {
                    possibleActions.Add(a);
                }
            }
        }
    }

    string GetStdVerb(string verb)
    {
        switch (verb)
        {
            case "pickup":
            case "get":
            case "grab":
                return "pickup";
        }

        return "";
    }

    string GetErrorMessage(string[] commandString)
    {
        string ret = "";
        string verb = GetStdVerb(commandString[0]);

        switch (verb)
        {
            case "pickup":
                if (commandString.Length == 1)
                    ret = "What do you want me to " + commandString[0] + "?";
                else if (IsThereAnObjectWithThatNameNearby(commandString[1]))
                    ret = "Can't " + commandString[0] + " " + commandString[1] + "!";
                else
                    ret = "I don't see a " + commandString[1] + "!";
                    break;
            default:
                ret = "Don't know what " + commandString[0] + " is!";
                break;
        }

        return ret;
    }

    bool IsThereAnObjectWithThatNameNearby(string objectName)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayers);

        foreach (var c in colliders)
        {
            InteractiveObject interactiveObject = c.GetComponentInChildren<InteractiveObject>();
            if (interactiveObject == null) interactiveObject = c.GetComponentInParent<InteractiveObject>();

            if (interactiveObject != null)
            {
                if (interactiveObject.gameItem.IsThisTheItem(objectName))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
