using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    [System.Serializable]
    public struct Credit
    {
        [TextArea]
        public string text;
        public float  time;
    };

    public List<Credit>     credits;
    public TextMeshProUGUI  creditText;

    int index = 0;
    float timeForNextCredit;


    void Start()
    {
        index = 0;
        NewCredit();
    }

    void NewCredit()
    {
        timeForNextCredit = credits[index].time;
        creditText.text = credits[index].text;
    }

    void Update()
    {
        timeForNextCredit -= Time.deltaTime;
        if (timeForNextCredit <= 0)
        {
            index = (index + 1) % credits.Count;
            NewCredit();
        }
    }

    public void OnSubmit(TMP_InputField input)
    {
        string command = input.text.ToLower();

        switch (command)
        {
            case "start":
                SceneManager.LoadScene("GameScene");
                break;
            case "quit":
            case "exit":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
            default:
                input.text = "";
                input.ActivateInputField();
                break;
        }
    }
}
