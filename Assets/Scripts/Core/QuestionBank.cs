using UnityEngine;

[CreateAssetMenu(menuName = "Game/QuestionBank")]
public class QuestionBank : ScriptableObject
{
    public DecisionData[] decisions;
}