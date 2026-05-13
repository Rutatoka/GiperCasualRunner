using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerStats PlayerStats { get; private set; }

    public List<ProfessionData> professions;
    public ProfessionData.ProfessionCategory CategoryResult { get; private set; }
    public ProfessionData DirectionResult { get; private set; }
    public bool HasCategoryResult { get; private set; }
    public FlowState flowState;
    public enum GameState
    {
        Bootstrap,
        Menu,
        Game,
        Result,
        Shop,
        DailyTasks,
        Profile,
        MiniGames
    }

    public GameState State { get; private set; }
    public bool IsPaused { get; private set; }
    private void Awake()
    {
        Debug.Log("GameManager Awake: " + this);

        if (Instance != null)
        {
            Debug.Log("Destroy duplicate GameManager");
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

    public void GoToShop()
    {
        State = GameState.Shop;
        SceneManager.LoadScene("ShopScene");
    }

    public void GoToDailyTasks()
    {
        State = GameState.DailyTasks;
        SceneManager.LoadScene("DailyTasksScene");
    }

    public void GoToProfile()
    {
        State = GameState.Profile;
        SceneManager.LoadScene("ProfileScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMiniGames()
    {
        State = GameState.MiniGames;
        SceneManager.LoadScene("MiniGamesScene");
    }
    public void SetCategoryResult(ProfessionData.ProfessionCategory result)
    {
        CategoryResult = result;
        HasCategoryResult = true;
    }

    public void SetDirectionResult(ProfessionData result)
    {
        DirectionResult = result;
    }

    public int GetGems()
    {
        return PlayerPrefs.GetInt("Gems", 0);
    }

    public void AddGems(int amount)
    {
        int current = PlayerPrefs.GetInt("Gems", 0);
        current += amount;
        current = Mathf.Clamp(current, 0, 100);
        PlayerPrefs.SetInt("Gems", current);
        PlayerPrefs.Save();
    }

    public bool SpendGems(int amount)
    {
        int current = PlayerPrefs.GetInt("Gems", 0);
        if (current >= amount)
        {
            PlayerPrefs.SetInt("Gems", current - amount);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
    public void GoToMenu()
    {
        State = GameState.Menu;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        PlayerStats = new PlayerStats();

        // важно: сбрасываем результаты между играми
        CategoryResult = default;
        DirectionResult = null;
        HasCategoryResult = false;
        State = GameState.Game;
        SceneManager.LoadScene("Game");
    }

    public void FinishRun()
    {
        State = GameState.Result;
        SceneManager.LoadScene("Result");
    }

    public void UpdateDailyTaskProgress(string taskTitle, int amount)
    {
        string progressKey = $"daily_task_{taskTitle}_progress";
        int currentProgress = PlayerPrefs.GetInt(progressKey, 0);
        currentProgress += amount;
        PlayerPrefs.SetInt(progressKey, currentProgress);
        PlayerPrefs.Save();

        Debug.Log($"Обновлён прогресс задания '{taskTitle}': {currentProgress}");
    }

    public void CompleteDailyTask(string taskTitle)
    {
        string key = $"daily_task_{taskTitle}_completed";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public bool IsDailyTaskCompleted(string taskTitle)
    {
        string key = $"daily_task_{taskTitle}_completed";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public int GetDailyTaskProgress(string taskTitle)
    {
        string progressKey = $"daily_task_{taskTitle}_progress";
        return PlayerPrefs.GetInt(progressKey, 0);
    }

    public void SetPaused(bool value)
    {
        IsPaused = value;
        Time.timeScale = value ? 0f : 1f;
    }
    public static class ProfessionUtils
    {
        public static string GetCategoryName(ProfessionData.ProfessionCategory cat)
        {
            return cat switch
            {
                ProfessionData.ProfessionCategory.Key => "Ключевые направления",
                ProfessionData.ProfessionCategory.Creative => "Креативные",
                ProfessionData.ProfessionCategory.Social => "Социальные",
                ProfessionData.ProfessionCategory.Business => "Бизнес",
                ProfessionData.ProfessionCategory.Specialization => "Специализации",
                ProfessionData.ProfessionCategory.Additional => "Дополнительные",
                _ => "Неизвестно"
            };
        }
    }
}