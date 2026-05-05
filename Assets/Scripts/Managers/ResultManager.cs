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
        var stats = GameManager.Instance.PlayerStats;

        var matcher = new ProfessionMatcher(GameManager.Instance.professions);
        var profession = matcher.GetBestMatch(stats);

        if (profession == null)
        {
            Debug.LogError("No profession found!");
            return;
        }

        professionText.text = profession.professionName;
        descriptionText.text = "Ты подходишь под эту профессию.";

        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(GameManager.Instance.GoToMenu);
    }

}
