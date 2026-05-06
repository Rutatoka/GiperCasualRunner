using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
     Button buttonStart;
     Button buttonExit;

    public void Start()
    {
        buttonStart = GameObject.Find("StartButton").GetComponent<Button>();
        buttonExit = GameObject.Find("ExitButton").GetComponent<Button>();
        buttonStart.onClick.AddListener(OnStartButtonClicked);
        buttonExit.onClick.AddListener(OnExittButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.StartGame();
    }
    private void OnExittButtonClicked()
    {
        Application.Quit();
    }
}
