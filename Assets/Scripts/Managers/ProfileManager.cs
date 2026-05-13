using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Main Info")]
    public TextMeshProUGUI professionName;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public Slider experienceSlider;

    [Header("Tabs")]
    public Button infoTab;
    public Button skillsTab;
    public Button achievementsTab;
    public Button backButton;

    [Header("Panels")]
    public GameObject infoPanel;
    public GameObject skillsPanel;
    public GameObject achievementsPanel;

    [Header("Skills")]
    public Transform skillGrid;
    public GameObject skillPrefab;

    [Header("Achievements")]
    public Transform achievementGrid;
    public GameObject achievementPrefab;

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.GoToMenu());

        infoTab.onClick.AddListener(() => SwitchTab(0));
        skillsTab.onClick.AddListener(() => SwitchTab(1));
        achievementsTab.onClick.AddListener(() => SwitchTab(2));

        LoadProfile();
        LoadSkills();
        LoadAchievements();
    }

    private void SwitchTab(int index)
    {
        infoPanel.SetActive(index == 0);
        skillsPanel.SetActive(index == 1);
        achievementsPanel.SetActive(index == 2);
    }

    private void LoadProfile()
    {
        var gm = GameManager.Instance;
        var stats = gm.PlayerStats;

        // Профессия
        if (gm.DirectionResult != null)
            professionName.text = gm.DirectionResult.professionName;
        else
            professionName.text = "Профессия не определена";

        // Уровень и опыт - БЕРЁМ ИЗ HAPPINESSSYSTEM
        if (HappinessSystem.Instance != null)
        {
            // Используем количество монет как уровень (или придумайте свою логику)
            int level = Mathf.FloorToInt(HappinessSystem.Instance.currentValue / 10) + 1;
            int experience = HappinessSystem.Instance.currentValue % 10 * 10;
            int maxExperience = 100;

            levelText.text = $"Уровень: {level}";
            experienceText.text = $"Опыт: {experience}/{maxExperience}";
            experienceSlider.maxValue = maxExperience;
            experienceSlider.value = experience;
        }
        else
        {
            // Если HappinessSystem нет - заглушка
            levelText.text = "Уровень: 1";
            experienceText.text = "Опыт: 0/100";
            experienceSlider.value = 0;
        }
    }

    private void LoadSkills()
    {
        // Очищаем предыдущие навыки
        foreach (Transform child in skillGrid)
        {
            Destroy(child.gameObject);
        }

        string[] skills = { "Аналитика", "Креативность", "Коммуникация", "Логика", "Стрессоустойчивость" };
        foreach (var skill in skills)
        {
            var obj = Instantiate(skillPrefab, skillGrid);
            var skillUI = obj.GetComponent<SkillUI>();
            if (skillUI != null)
            {
                // Вместо Random используем реальные данные
                // Для примера: 60% совпадения для всех навыков
                float value = 0.6f;
                skillUI.Setup(skill, value);
            }
        }
    }

    private void LoadAchievements()
    {
        // Очищаем предыдущие достижения
        foreach (Transform child in achievementGrid)
        {
            Destroy(child.gameObject);
        }

        string[] achievements = { "Первый тест", "Эксперт", "Стабильность", "Скорость" };
        foreach (var achievement in achievements)
        {
            var obj = Instantiate(achievementPrefab, achievementGrid);
            var achievementUI = obj.GetComponent<AchievementUI>();
            if (achievementUI != null)
            {
                // Используем количество монет для прогресса (просто для примера)
                int progress = 0;
                if (HappinessSystem.Instance != null && HappinessSystem.Instance.currentValue > 10)
                    progress = 1;

                achievementUI.Setup(achievement, progress);
            }
            else
            {
                Debug.LogError("AchievementUI компонент не найден на префабе!");
            }
        }
    }
}