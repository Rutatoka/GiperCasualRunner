using UnityEngine;
using UnityEngine.UI;

public class HappinessSystem : MonoBehaviour
{
    public static HappinessSystem Instance;

    public Slider slider;

    private int value = 0;
    private int max = 100;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(int amount)
    {
        value += amount;
        value = Mathf.Clamp(value, 0, max);

        slider.value = (float)value / max;
    }
}
