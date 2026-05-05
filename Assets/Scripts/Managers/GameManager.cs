using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int gems;

    public PlayerStats PlayerStats { get; private set; }

    // =========================
    // DATA
    // =========================
   // public List<CategoryData> categories;
    public List<ProfessionData> professions;

    // =========================
    // RESULTS
    // =========================
    public ProfessionData.ProfessionCategory CategoryResult { get; private set; }
    public ProfessionData DirectionResult { get; private set; }
    public bool HasCategoryResult { get; private set; }
    // =========================
    // FLOW
    // =========================
    public FlowState flowState;

    // =========================
    // STATE
    // =========================
    public enum GameState
    {
        Bootstrap,
        Menu,
        Game,
        Result
    }

    public GameState State { get; private set; }
    public bool IsPaused { get; private set; }

    // =========================
    // SINGLETON
    // =========================
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

    // =========================
    // RESULTS SETTERS
    // =========================
    public void SetCategoryResult(ProfessionData.ProfessionCategory result)
    {
        CategoryResult = result;
        HasCategoryResult = true;
    }

    public void SetDirectionResult(ProfessionData result)
    {
        DirectionResult = result;
    }

    // =========================
    // GAME FLOW
    // =========================
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

    // =========================
    // PAUSE
    // =========================
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