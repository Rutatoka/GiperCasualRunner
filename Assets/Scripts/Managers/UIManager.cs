using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;

    public Button buttonA;
    public Button buttonB;

    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;

    public void Bind(DecisionSystem system)
    {
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(system.ChooseA);
        buttonB.onClick.AddListener(system.ChooseB);
    }

    public void ShowDecision(DecisionData decision)
    {
        questionText.text = decision.question;
        textA.text = decision.choiceA.text;
        textB.text = decision.choiceB.text;
    }
}
