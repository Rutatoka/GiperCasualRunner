using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Question UI")]
    public TextMeshProUGUI questionText;

    public Button buttonA;
    public Button buttonB;

    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;

    [Header("Category Result UI")]
    public GameObject categoryResultPanel;
    public TextMeshProUGUI categoryText;
    public Button continueButton;
    public Button menuButton;
    public void Bind(DecisionSystem system)
    {
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(system.ChooseA);
        buttonB.onClick.AddListener(system.ChooseB);

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(system.ContinueToDirection);
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(GoToMenu);
    }

    // 👉 ВОПРОСЫ
    public void ShowDecision(DecisionData decision)
    {
        categoryResultPanel.SetActive(false);

        questionText.gameObject.SetActive(true);
        buttonA.gameObject.SetActive(true);
        buttonB.gameObject.SetActive(true);

        questionText.text = decision.question;
        textA.text = decision.choiceA.text;
        textB.text = decision.choiceB.text;
    }
    private void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }
    // 👉 ЭКРАН КАТЕГОРИИ
    public void ShowCategoryResult(string categoryName)
    {
        categoryResultPanel.SetActive(true);

        categoryText.text =
          $"🎯 Твоя категория:\n" +
          $"<b>{GameManager.ProfessionUtils.GetCategoryName(GameManager.Instance.CategoryResult)}</b>";

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            categoryResultPanel.SetActive(false);
            FindAnyObjectByType<DecisionSystem>().ContinueToDirection();
        });
    }
  
}