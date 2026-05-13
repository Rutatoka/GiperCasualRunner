// MenuUIManager.cs (повесьте на Canvas в MainMenu)
using TMPro;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public TMP_Text gemsText;

    private void Start()
    {
        UpdateGems();
        InvokeRepeating(nameof(UpdateGems), 0f, 1f);
    }

    private void UpdateGems()
    {
        int gems = PlayerPrefs.GetInt("Gems", 0);
        gemsText.text = $"Твои гемы: {gems}";
    }

    public void GoToShop()
    {
        GameManager.Instance.GoToShop();
    }

    public void GoToProfile()
    {
        GameManager.Instance.GoToProfile();
    }

    public void GoToDailyTasks()
    {
        GameManager.Instance.GoToDailyTasks();
    }
}