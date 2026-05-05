using UnityEngine;

public class DecisionSystem : MonoBehaviour
{
    public DecisionData[] decisions;
    private int currentIndex;

    private UIManager ui;
    public DecisionTimer timer;
    private int totalDecisions;
    private void Start()
    {
        ui = FindFirstObjectByType<UIManager>();
        ui.Bind(this);
        totalDecisions = decisions.Length;

        timer.Init(this, totalDecisions);
    
        ShowCurrent();
    }

    public void ShowCurrent()
    {
        if (currentIndex >= decisions.Length)
        {
            GameManager.Instance.FinishRun();
            return;
        }

        ui.ShowDecision(decisions[currentIndex]);
        ui.ShowDecision(decisions[currentIndex]);
        timer.StartTimer(currentIndex, totalDecisions);
    }
    public void SkipDecision()
    {
        GameManager.Instance.PlayerStats.AddStat(StatType.Stability, -2);

        currentIndex++;
        ShowCurrent();
    }

    public void ChooseA() => Apply(decisions[currentIndex].choiceA);
    public void ChooseB() => Apply(decisions[currentIndex].choiceB);

    private void Apply(ChoiceData choice)
    {
        timer.StopTimer();
        foreach (var e in choice.effects)
            GameManager.Instance.PlayerStats.AddStat(e.stat, e.value);

        currentIndex++;
        ShowCurrent();
    }
}
