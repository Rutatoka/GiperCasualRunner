using UnityEngine;

[CreateAssetMenu(menuName = "Game/Decision")]
public class DecisionData : ScriptableObject
{
    public string question;
    public ChoiceData choiceA;
    public ChoiceData choiceB;

}
[System.Serializable]
public class ChoiceData
{
    public string text;

    public StatEffect primary;
    public StatEffect secondary;
}
[System.Serializable]
public class StatEffect
{
    public StatType stat;
    public int value;
}
