using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int gems;
    public PlayerStats PlayerStats { get; private set; }

    [Header("Data")]
    public List<ProfessionData> professions;

    public enum GameState
    {
        Bootstrap,
        Menu,
        Game,
        Result
    }

    public GameState State { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerStats = new PlayerStats();
        State = GameState.Bootstrap;
    }

    private void Start()
    {
        GoToMenu();
    }
    public void AddGem()
    {
        gems++;
    }
    public void GoToMenu()
    {
        State = GameState.Menu;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        PlayerStats = new PlayerStats();
        State = GameState.Game;
        SceneManager.LoadScene("Game");
    }

    public void FinishRun()
    {
        State = GameState.Result;
        SceneManager.LoadScene("Result");
    }
}
