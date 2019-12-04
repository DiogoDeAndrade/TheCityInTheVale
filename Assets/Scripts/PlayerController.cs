using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public enum State { Movement, Command };

    public Camera           gameCamera;
    public Camera           asciiCamera;
    public RenderTexture    targetTexture;
    public LayerMask        interactionLayers;
    public float            interactionRadius = 10.0f;
    public float            interactionTolerance = 0.25f;
    public TextBox          lookBox;
    public TMP_InputField   inputField;
    public bool             consecutiveInputs = false;
    public TextBox          outputWindow;
    public GameState        gameState;

    static public string loadFile = "";

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

        if (loadFile != "")
        {
            StartCoroutine(LoadCR());
        }

        gameState.SetBool("asciimode", true);
    }

    IEnumerator LoadCR()
    {
        yield return null;
        yield return null;

        string json = System.IO.File.ReadAllText(loadFile);

        JsonUtility.FromJsonOverwrite(json, gameState);

        outputWindow.AddText("Loaded " + loadFile);

        loadFile = "";

        transform.position = gameState.position;
        transform.rotation = gameState.rotation;

        var charController = GetComponent<CharacterController>();
        if (charController)
        {
            charController.enabled = false;
            transform.position = gameState.position;
            transform.rotation = gameState.rotation;
            charController.enabled = true;

            var gameActions = FindObjectsOfType<GameAction>();
            foreach (var action in gameActions)
            {
                action.OnLoad(this);
            }
        }

        bool b = gameState.GetBool("asciimode");
        SetASCIIMode(b);
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

        gameState.position = transform.position;
        gameState.rotation = transform.rotation;

        if (gameState.Exists("asciimode"))
        {
            bool b = gameState.GetBool("asciimode");
            SetASCIIMode(b);
        }
    }

    void SetASCIIMode(bool b)
    {
        if (b)
        {
            if (!asciiCamera.isActiveAndEnabled)
            {
                asciiCamera.gameObject.SetActive(true);
                gameCamera.targetTexture = targetTexture;
            }
        }
        else
        {
            if (asciiCamera.isActiveAndEnabled)
            {
                asciiCamera.gameObject.SetActive(false);
                gameCamera.targetTexture = null;
            }
        }
        gameState.SetBool("asciimode", b);
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

        List<string> commandString = new List<string>(command.Split(' ', '\t'));

        commandString.RemoveAll((s) => s == "in");
        commandString.RemoveAll((s) => s == "at");
        commandString.RemoveAll((s) => s == "on");
        commandString.RemoveAll((s) => s == "a");
        commandString.RemoveAll((s) => s == "an");
        commandString.RemoveAll((s) => s == "the");

        if (commandString.Count > 0)
        {            
            string verb = GetStdVerb(commandString[0]);
            bool   processed = false;

            List<GameAction> possibleActions = new List<GameAction>();
            GetAllPossibleActions(possibleActions);

            foreach (var action in possibleActions)
            {
                if (action.GetVerb() == verb)
                {
                    if (action.IsValidAction(this, commandString, 1))
                    {
                        processed = action.RunAction(this, commandString);
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
            case "examine":
            case "look":
                return "examine";
            case "load": return "load";
            case "save": return "save";
            case "set": return "set";
        }

        return "";
    }

    string GetErrorMessage(List<string> commandString)
    {
        string ret = "";
        string verb = GetStdVerb(commandString[0]);

        switch (verb)
        {
            case "pickup":
                if (commandString.Count == 1)
                    ret = "What do you want me to " + commandString[0] + "?";
                else if (IsThereAnObjectWithThatNameNearby(commandString[1]))
                    ret = "Can't " + commandString[0] + " " + commandString[1] + "!";
                else
                    ret = "I don't see a " + commandString[1] + "!";
                    break;
            case "examine":
                if (commandString.Count == 1)
                    ret = "What do you want me to " + commandString[0] + "?";
                else if (IsThereAnObjectWithThatNameNearby(commandString[1]))
                    ret = "Can't " + commandString[0] + " " + commandString[1] + "!";
                else
                    ret = "I don't see a " + commandString[1] + "!";
                break;
            case "set":
                if (commandString.Count != 3)
                {
                    ret = "Sintax: set [variable] [value]";
                }
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
                if (interactiveObject.gameItem)
                {
                    if (interactiveObject.gameItem.IsThisTheItem(objectName))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
