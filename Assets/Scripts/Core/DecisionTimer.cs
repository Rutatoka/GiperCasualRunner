using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionTimer : MonoBehaviour
{
    public Slider timerSlider;
    public TextMeshProUGUI progressText;

    public float maxTime = 10f;

    private float currentTime;
    private DecisionSystem decisionSystem;
    private bool isActive;

    public void Init(DecisionSystem system, int total)
    {
        decisionSystem = system;
        progressText.text = $"0/{total}";
    }

    public void StartTimer(int current, int total)
    {
        currentTime = maxTime;
        isActive = true;

        progressText.text = $"{current}/{total}";
    }


    private void Update()
    {
        if (!isActive) return;

        currentTime -= Time.deltaTime;
        timerSlider.value = currentTime / maxTime;

        if (currentTime <= 0)
        {
            isActive = false;
            TimeExpired();
        }

    }
    public void StopTimer()
    {
        isActive = false;
    }

    void TimeExpired()
    {
        decisionSystem.SkipDecision();
    }
}
