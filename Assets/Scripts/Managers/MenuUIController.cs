using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
     Button buttonStart;
     Button buttonExit;
    public Button shopButton;
    public Button dailyTasksButton;
    public Button profileButton;
    public Button miniGamesButton;
    public void Start()
    {
        buttonStart = GameObject.Find("StartButton").GetComponent<Button>();
        buttonExit = GameObject.Find("ExitButton").GetComponent<Button>();
        buttonStart.onClick.AddListener(() => GameManager.Instance.StartGame());
        buttonExit.onClick.AddListener(() => GameManager.Instance.ExitGame());

        shopButton.onClick.AddListener(() => GameManager.Instance.GoToShop());
        dailyTasksButton.onClick.AddListener(() => GameManager.Instance.GoToDailyTasks());
        profileButton.onClick.AddListener(() => GameManager.Instance.GoToProfile());
        miniGamesButton.onClick.AddListener(() => GameManager.Instance.GoToMiniGames());
    }
}
