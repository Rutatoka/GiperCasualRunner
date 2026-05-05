using UnityEngine;

[CreateAssetMenu(menuName = "Game/Profession")]
public class ProfessionData : ScriptableObject
{
    public string professionName;
    public ProfessionCategory category;
    public float[] vector = new float[10];
    public bool isCategory;
    public enum ProfessionCategory
    {
        None = -1,
        Key,
        Creative,
        Social,
        Business,
        Specialization,
        Additional
    }
}
