using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DailyTasksManager : MonoBehaviour
{
    [Header("UI")]
    public Transform tasksContainer;
    public GameObject taskPrefab;
    public Button backButton;
    public TextMeshProUGUI timerText;

    private List<DailyTask> tasks;
    private float timer = 0f;

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.GoToMenu());
        timer = PlayerPrefs.GetFloat("DailyTimer", 0f);

        // ЗАГРУЖАЕМ ВСЁ ПРИ СТАРТЕ
        LoadTasks();
        CheckDailyReset();
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
    }

    private void LoadTasks()
    {
        // Очищаем контейнер
        foreach (Transform child in tasksContainer)
        {
            Destroy(child.gameObject);
        }

        tasks = GetDailyTasks();

        // Загружаем прогресс из PlayerPrefs для каждой задачи
        foreach (var task in tasks)
        {
            string completedKey = $"daily_task_{task.title}_completed";
            task.isCompleted = PlayerPrefs.GetInt(completedKey, 0) == 1;

            string progressKey = $"daily_task_{task.title}_progress";
            task.currentProgress = PlayerPrefs.GetInt(progressKey, 0);

            // Создаём UI
            var obj = Instantiate(taskPrefab, tasksContainer);
            var ui = obj.GetComponent<DailyTaskUI>();
            if (ui != null)
            {
                ui.Setup(task, OnTaskClaimed);
            }
        }
    }

    private List<DailyTask> GetDailyTasks()
    {
        return new List<DailyTask>
        {
            new DailyTask("Пройди тест", "Ответь на 10 вопросов", 0, 10, 50),
            new DailyTask("Посмотри рекламу", "Получи усиление", 0, 1, 25),
            new DailyTask("Заработай монет", "Собери 100 монет", 0, 100, 75),
        };
    }

    private void OnTaskClaimed(DailyTask task)
    {
        if (HappinessSystem.Instance != null)
        {
            HappinessSystem.Instance.Add(task.reward);
        }

        task.isCompleted = true;

        // Сохраняем в PlayerPrefs
        string completedKey = $"daily_task_{task.title}_completed";
        PlayerPrefs.SetInt(completedKey, 1);
        PlayerPrefs.Save();

        // Обновляем UI
        UpdateAllTaskUI();
    }

    private void UpdateTimer()
    {
        timer += 1f;
        PlayerPrefs.SetFloat("DailyTimer", timer);
        PlayerPrefs.Save();

        int remainingSeconds = 86400 - (int)timer;
        if (remainingSeconds < 0) remainingSeconds = 0;

        int hours = remainingSeconds / 3600;
        int minutes = (remainingSeconds % 3600) / 60;
        int seconds = remainingSeconds % 60;

        timerText.text = $"Обновление: {hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    private void CheckDailyReset()
    {
        string lastDate = PlayerPrefs.GetString("LastDailyDate", "");
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastDate != today)
        {
            timer = 0f;
            PlayerPrefs.SetFloat("DailyTimer", 0f);

            foreach (var task in tasks)
            {
                string completedKey = $"daily_task_{task.title}_completed";
                PlayerPrefs.SetInt(completedKey, 0);

                string progressKey = $"daily_task_{task.title}_progress";
                PlayerPrefs.SetInt(progressKey, 0);
            }

            PlayerPrefs.SetString("LastDailyDate", today);
            PlayerPrefs.Save();

            LoadTasks(); // Перезагружаем UI
        }
    }

    private void UpdateAllTaskUI()
    {
        // Просто перезагружаем всё
        LoadTasks();
    }
}