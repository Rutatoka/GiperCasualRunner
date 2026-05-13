using System.Collections.Generic;
using UnityEngine;

public class DecisionSystem : MonoBehaviour
{
    public QuestionBank categoryBank;
    public QuestionBank directionBank;
    public FlowConfig flow;

    private int index;
    private FlowState state;

    private GameUIManager ui;
    public DecisionTimer timer;
    private bool inputLocked;
    private void Start()
    {
        ui = FindAnyObjectByType<GameUIManager>();
        ui.Bind(this);

        state = FlowState.CategoryTest;
        index = 0;

        timer.Init(this, flow.categoryQuestionCount + flow.directionQuestionCount);

        ShowCurrent();
    }

    public void ShowCurrent()
    {
        if (state == FlowState.Finished)
        {
            GameManager.Instance.FinishRun();
            return;
        }

        // блокируем показ во время экрана результата категории
        if (state == FlowState.CategoryResult)
            return;

        ui.ShowDecision(GetCurrentQuestion());
        timer.StartTimer(index, flow.categoryQuestionCount + flow.directionQuestionCount);
    }

    private DecisionData GetCurrentQuestion()
    {
        if (state == FlowState.CategoryTest)
            return categoryBank.decisions[index];

        return directionBank.decisions[index - flow.categoryQuestionCount];
    }

    public void ChooseA() => Apply(GetCurrentQuestion().choiceA);
    public void ChooseB() => Apply(GetCurrentQuestion().choiceB);

    private void Apply(ChoiceData choice)
    {
        if (inputLocked) return; // 👈 СПАСАЕТ ВСЁ

        timer.StopTimer();

        GameManager.Instance.PlayerStats.AddStat(
            choice.primary.stat,
            2,
            true,
            state
        );

        GameManager.Instance.PlayerStats.AddStat(
            choice.secondary.stat,
            1,
            false,
            state
        );
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateDailyTaskProgress("Пройди тест", 1);
        }
        Next();
    }
    private void Next()
    {
        index++;

        // =========================
        // КАТЕГОРИЯ ЗАВЕРШЕНА
        // =========================
        if (state == FlowState.CategoryTest &&
            index >= flow.categoryQuestionCount)
        {
            state = FlowState.CategoryResult;
            CalculateCategory();
            return;
        }

        // =========================
        // ВСЁ ЗАВЕРШЕНО
        // =========================
        if (state == FlowState.DirectionTest &&
     index >= flow.categoryQuestionCount + flow.directionQuestionCount)
        {
            CalculateDirection();

            state = FlowState.Finished;
            GameManager.Instance.FinishRun();
            return;
        }
        ShowCurrent();
       
    }


    //private void CalculateCategory()
    //{
    //    var player = GameManager.Instance.PlayerStats.GetVector(FlowState.CategoryTest);

    //    ProfessionData.ProfessionCategory bestCategory = default;
    //    float bestScore = -1f;

    //    foreach (var p in GameManager.Instance.professions)
    //    {
    //        float score = Cosine(player, p.vector);

    //        if (score > bestScore)
    //        {
    //            bestScore = score;
    //            bestCategory = p.category;
    //        }
    //    }

    //    GameManager.Instance.SetCategoryResult(bestCategory);

    //    inputLocked = true; // 👈 ВОТ ЭТО

    //    ui.ShowCategoryResult(bestCategory.ToString());

    //    GameManager.Instance.SetPaused(true);

    //    Debug.Log("Категория: " + bestCategory);
    //}
    private void CalculateCategory()
    {
        var player = GameManager.Instance.PlayerStats.GetVector(FlowState.CategoryTest);

        Dictionary<ProfessionData.ProfessionCategory, float> scores = new();

        foreach (var p in GameManager.Instance.professions)
        {
            float score = Cosine(player, p.vector);

            if (!scores.ContainsKey(p.category))
                scores[p.category] = 0;

            scores[p.category] += score;
        }

        ProfessionData.ProfessionCategory bestCategory = default;
        float bestScore = -1f;

        foreach (var pair in scores)
        {
            if (pair.Value > bestScore)
            {
                bestScore = pair.Value;
                bestCategory = pair.Key;
            }
        }

        GameManager.Instance.SetCategoryResult(bestCategory);

        inputLocked = true;
        ui.ShowCategoryResult(GameManager.ProfessionUtils.GetCategoryName(bestCategory));

        GameManager.Instance.SetPaused(true);

        Debug.Log("Категория: " + bestCategory);
    }
    private float Cosine(float[] a, float[] b)
    {
        float dot = 0f;
        float magA = 0f;
        float magB = 0f;

        for (int i = 0; i < 10; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        if (magA == 0 || magB == 0) return 0f;

        return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB));
    }
    // =========================
    // НАПРАВЛЕНИЕ
    // =========================
    private void CalculateDirection()
    {
        if (!GameManager.Instance.HasCategoryResult)
        {
            Debug.LogError("Категория не определена — направление не считается");
            return;
        }
        var category = GameManager.Instance.CategoryResult;


        var player = GameManager.Instance.PlayerStats.GetVector(FlowState.DirectionTest);

        bool isEmpty = true;
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != 0)
            {
                isEmpty = false;
                break;
            }
        }

        if (isEmpty)
        {
            Debug.LogError("Вектор направления пустой — ты не ответила ни на один вопрос второго этапа");
        }

        var filtered = GameManager.Instance.professions.FindAll(p =>
            p.category == GameManager.Instance.CategoryResult
        );

        if (filtered.Count == 0)
        {
            Debug.LogError("Нет профессий для категории: " + GameManager.Instance.CategoryResult);
            return;
        }

        var matcher = new ProfessionMatcher(filtered);

        var (best, _) = matcher.GetBestMatch(player);

        GameManager.Instance.SetDirectionResult(best);

        Debug.Log("Направление: " + best.professionName);
       
    }
    public void ContinueToDirection()
    {
        if (!GameManager.Instance.HasCategoryResult)
        {
            Debug.LogError("Категория не установлена");
            return;
        }
        Debug.Log("Переход ко второму этапу. Категория: " + GameManager.Instance.CategoryResult);
        GameManager.Instance.SetPaused(false);

        inputLocked = false;

        state = FlowState.DirectionTest;

        ShowCurrent();
    }
    public void SkipDecision()
    {
        GameManager.Instance.PlayerStats.AddStat(
      StatType.Stability,
      -2,
      false,
      state
  );

        Next();
    }

}