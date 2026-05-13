using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI professionText;
    public TextMeshProUGUI descriptionText;
    public Button menuButton;

    private void Start()
    {
        var gm = GameManager.Instance;
        var stats = gm.PlayerStats;

        var category = gm.CategoryResult;
        var direction = gm.DirectionResult;

        // -------------------------
        // защита от кривых состояний
        // -------------------------
        if (direction == null)
        {
            Debug.LogError("DirectionResult = null в ResultManager");
            professionText.text = "Результат не определён";
            descriptionText.text = "Ошибка расчёта направления.";
            SetupButton();
            return;
        }

        // -------------------------
        // фильтр ТОЛЬКО направлений
        // -------------------------
        var filtered = gm.professions.FindAll(p =>
            p.category == category && !p.isCategory
        );

        if (filtered.Count == 0)
        {
            Debug.LogError("Нет направлений в категории: " + category);
            professionText.text = "Ошибка данных";
            descriptionText.text = "Нет направлений для выбранной категории.";
            SetupButton();
            return;
        }

        var matcher = new ProfessionMatcher(filtered);
        var (_, ranked) = matcher.GetBestMatch(
            stats.GetVector(FlowState.DirectionTest)
        );

        // -------------------------
        // UI
        // -------------------------
        professionText.text =
            $"{GameManager.ProfessionUtils.GetCategoryName(category)} → {direction.professionName}";

        float confidence = stats.GetConfidence();

        descriptionText.text =
            $"Категория: {GameManager.ProfessionUtils.GetCategoryName(category)}\n" +
            $"Направление: {direction.professionName}\n\n" +
            $"Топ-3 направлений:\n" +
            $"{GetSafeRank(ranked, 0)}\n" +
            $"{GetSafeRank(ranked, 1)}\n" +
            $"{GetSafeRank(ranked, 2)}\n" +
            $"Уверенность: {confidence:0}";

        SetupButton(); // Теперь кнопка "Меню" сама вызывает рекламу
    }

    private void SetupButton()
    {
        menuButton.onClick.RemoveAllListeners();
        // 👇 Вместо прямого перехода — сначала реклама
        menuButton.onClick.AddListener(TryShowAdThenMenu);
    }

    private void TryShowAdThenMenu()
    {
        menuButton.interactable = false; // Блокируем, чтобы не нажали дважды

        if (RuStoreAdManager.Instance != null)
        {
            // Подписываемся на событие и запускаем рекламу
            RuStoreAdManager.Instance.OnRewardedAdFinished.AddListener(OnAdFinished);
            RuStoreAdManager.Instance.ShowRewardedAd();
        }
        else
        {
            // Если менеджер не найден — просто переходим
            GoToMenu();
        }
    }

    private void OnAdFinished()
    {
        if (RuStoreAdManager.Instance != null)
            RuStoreAdManager.Instance.OnRewardedAdFinished.RemoveListener(OnAdFinished);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateDailyTaskProgress("Посмотри рекламу", 1);
        }
        GoToMenu();
    }

    private void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }

    private string GetSafeRank(List<(ProfessionData, float)> ranked, int index)
    {
        if (ranked == null || ranked.Count <= index)
            return "-";

        var item = ranked[index];
        if (item.Item1 == null || item.Item1.isCategory)
            return "-";

        float percent = Mathf.Clamp01(item.Item2) * 100f;
        return $"{item.Item1.professionName}\n<size=80%>{percent:0}% совпадения</size>";
    }
}