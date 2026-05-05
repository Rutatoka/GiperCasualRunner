using UnityEngine;

[CreateAssetMenu(menuName = "Game/Profession")]
public class ProfessionData : ScriptableObject
{
    public string professionName;

    public int tech;
    public int human;
    public int manager;
    public int worker;
    public int introvert;
    public int extrovert;
    public int analyst;
    public int intuitive;
    public int stability;
    public int openness;
}
