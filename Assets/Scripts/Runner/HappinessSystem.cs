using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HappinessSystem : MonoBehaviour
{
    public static HappinessSystem Instance;

    public Slider slider;
    public TMP_Text numGem;

    public int currentValue = 0;
    private int max = 100;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Загружаем при старте игры
        currentValue = PlayerPrefs.GetInt("Gems", 0);
        UpdateUI();
    }

    void OnDestroy()
    {
        // Сохраняем при уничтожении сцены
        PlayerPrefs.SetInt("Gems", currentValue);
        PlayerPrefs.Save();
    }

    public void Add(int amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, max);
        UpdateUI();
    }

    void UpdateUI()
    {
        numGem.text = currentValue.ToString();
        slider.value = (float)currentValue / max;
    }
}
