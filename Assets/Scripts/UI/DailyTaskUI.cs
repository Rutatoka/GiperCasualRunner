using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class DailyTask
{
    public string title;
    public string description;
    public int currentProgress;
    public int targetProgress;
    public int reward;
    public bool isCompleted;

    public DailyTask(string title, string description, int current, int target, int reward)
    {
        this.title = title;
        this.description = description;
        this.currentProgress = current;
        this.targetProgress = target;
        this.reward = reward;
        isCompleted = false;
    }
}
public class DailyTaskUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public Slider progressSlider;
    public Button claimButton;
    public GameObject completedBadge;

    private DailyTask currentTask;
    private System.Action<DailyTask> onClaimed;

    public void Setup(DailyTask task, System.Action<DailyTask> onClaimedCallback)
    {
        currentTask = task;
        onClaimed = onClaimedCallback;

        titleText.text = task.title;
        descriptionText.text = task.description;

        UpdateUI();

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClaimClicked);
    }

    private void UpdateUI()
    {
        progressText.text = $"{currentTask.currentProgress}/{currentTask.targetProgress}";
        progressSlider.maxValue = currentTask.targetProgress;
        progressSlider.value = currentTask.currentProgress;

        bool isComplete = currentTask.currentProgress >= currentTask.targetProgress;
        claimButton.interactable = isComplete && !currentTask.isCompleted;
        claimButton.gameObject.SetActive(!currentTask.isCompleted);

        if (completedBadge != null)
            completedBadge.SetActive(currentTask.isCompleted);
    }
    // В DailyTaskUI.cs добавьте:
    public void RefreshProgress(int newProgress)
    {
        if (currentTask != null)
        {
            currentTask.currentProgress = newProgress;
            UpdateUI();
        }
    }
    public void UpdateProgressManually()
    {
        // Просто перезагружаем данные из PlayerPrefs
        if (currentTask != null)
        {
            string progressKey = $"daily_task_{currentTask.title}_progress";
            currentTask.currentProgress = PlayerPrefs.GetInt(progressKey, 0);
            UpdateUI();
        }
    }
    private void OnClaimClicked()
    {
        if (currentTask.isCompleted) return;
        if (currentTask.currentProgress < currentTask.targetProgress) return;

        // Отмечаем как выполненное
        currentTask.isCompleted = true;

        // Вызываем колбэк
        onClaimed?.Invoke(currentTask);

        UpdateUI();
    }

    // Метод для обновления прогресса извне
    public void UpdateProgress(int amount)
    {
        if (currentTask.isCompleted) return;

        currentTask.currentProgress += amount;
        currentTask.currentProgress = Mathf.Min(currentTask.currentProgress, currentTask.targetProgress);
        UpdateUI();
    }
}