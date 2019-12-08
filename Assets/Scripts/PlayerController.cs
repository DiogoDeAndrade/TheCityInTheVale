using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public enum State { Movement, Command, Dead };

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
    public LayerMask        environmentMask;
    public float            sanityLossSpeed = 4.0f;
    public MeshRenderer     asciiRenderMesh;
    public GameState        gameState;

    static public string loadFile = "";

    float timeOfLastInput = 0;
    Material asciiRenderMaterial;

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
                case State.Dead:
                    controller.mouseLookEnable = false;
                    controller.moveEnable = false;
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
        gameState.InitExpressionEvaluator();

        if (loadFile != "")
        {
            StartCoroutine(LoadCR());
        }

        gameState.SetBool("asciimode", true);
        gameState.SetFloat("max_sanity", 100.0f);
        gameState.SetFloat("sanity", gameState.GetFloat("max_sanity"));
        gameState.SetBool("sanity_enabled", true);
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

            if ((Time.time - timeOfLastInput) > 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = State.Command;
                }
            }
        }
        else if (_state == State.Command)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameState.GetFloat("sanity") > 0.0f)
                    state = State.Movement;
                else
                    state = State.Dead;
            }
        }
        else if (_state == State.Dead)
        {
            if ((Time.time - timeOfLastInput) > 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = State.Command;
                }
            }
        }

        gameState.position = transform.position;
        gameState.rotation = transform.rotation;

        if (gameState.Exists("asciimode"))
        {
            bool b = gameState.GetBool("asciimode");
            SetASCIIMode(b);
        }

        UpdateSanity();
    }

    void UpdateSanity()
    {
        if (!gameState.GetBool("sanity_enabled")) return;
        if (state != State.Movement) return;

        Light[] lights = FindObjectsOfType<Light>();

        bool allOccluded = true;

        foreach (var l in lights)
        {
            //if (l.shadows == LightShadows.None) continue;

            Ray ray = new Ray(transform.position, Vector3.up);
            float maxDistance;

            if (l.type == LightType.Directional)
            {
                ray.origin = transform.position - l.transform.forward * 1000.0f;
                ray.direction = l.transform.forward;
                maxDistance = 1000.0f;
            }
            else
            {
                Vector3 rayDir = (l.transform.position - ray.origin);
                ray.direction = rayDir;
                maxDistance = rayDir.magnitude;
                ray.direction.Normalize();
            }

            if (!Physics.Raycast(ray, maxDistance, environmentMask))
            {
                allOccluded = false;
                break;
            }
        }

        float s = gameState.GetFloat("sanity");

        if (allOccluded)
        {            
            if (s > 0.0f)
            {
                s = Mathf.Max(0, s - Time.deltaTime * sanityLossSpeed);
                gameState.SetFloat("sanity", s);

                if (s <= 0.0f)
                {
                    outputWindow.AddText("");
                    outputWindow.AddText("Insanity takes you...");
                    outputWindow.AddText("All reason lost, you wander the vale until thirst and starvation takes you...");
                    state = State.Dead;
                }
            }
        }
        else
        {
            float maxSanity = gameState.GetFloat("max_sanity");
            s = Mathf.Min(maxSanity, s + Time.deltaTime * sanityLossSpeed * 2);
            gameState.SetFloat("sanity", s);
        }

        if (asciiRenderMaterial == null)
        {
            if (asciiRenderMesh != null)
            {
                asciiRenderMaterial = new Material(asciiRenderMesh.material);
                asciiRenderMesh.material = asciiRenderMaterial;
            }
        }

        if (asciiRenderMaterial)
        {
            s = s / gameState.GetFloat("max_sanity");
            float d = Mathf.Clamp01(Mathf.Lerp(1.0f, -1.0f, s));
            asciiRenderMaterial.SetFloat("_Distortion", d);
            asciiRenderMaterial.SetFloat("_Speed", Mathf.Lerp(0.0f, 0.025f, d));
            asciiRenderMaterial.SetColor("_Tint", Color.Lerp(Color.white, Color.red, d));
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
            if (gameState.GetFloat("sanity") > 0.0f)
                state = State.Movement;
            else
                state = State.Dead;
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
            if (gameState.GetFloat("sanity") > 0.0f)
                state = State.Movement;
            else
                state = State.Dead;
        }
        else
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    public void OnCancelCommand()
    {
        if (gameState.GetFloat("sanity") > 0.0f)
            state = State.Movement;
        else
            state = State.Dead;
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
            case "inv":
            case "inventory":
                return "inventory";
        }

        return verb;
    }

    string GetErrorMessage(List<string> commandString)
    {
        if (gameState.GetFloat("sanity") <= 0.0f)
        {
            return "You are dead... You can only restart or load!";
        }

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

    public void DisplayInventory()
    {
        if ((gameState.inventory == null) || (gameState.inventory.Count == 0))
        {
            outputWindow.AddText("You are carrying nothing...");
        }
        else
        {
            outputWindow.AddText("You are carrying:");
            foreach (var item in gameState.inventory)
            {
                if (item.count > 1)
                {
                    outputWindow.AddText(item.item.itemName + " x" + item.count);
                }
                else
                {
                    outputWindow.AddText(item.item.itemName);
                }
            }
        }
    }
}
