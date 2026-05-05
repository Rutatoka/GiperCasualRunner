using UnityEngine;

[CreateAssetMenu(menuName = "Game/Category")]
public class CategoryData : ScriptableObject
{
    public ProfessionData.ProfessionCategory category;
    public float[] vector = new float[10];
}