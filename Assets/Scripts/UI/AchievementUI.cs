using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI achievementNameText;
    public TextMeshProUGUI achievementStatusText;
    public Image achievementIcon;

    public void Setup(string achievementName, int progress)
    {
        achievementNameText.text = achievementName;

        if (progress >= 1)
        {
            achievementStatusText.text = "✅ Получено";
            achievementStatusText.color = Color.green;
        }
        else
        {
            achievementStatusText.text = "⏳ В процессе";
            achievementStatusText.color = Color.yellow;
        }
    }
}