using UnityEngine;

[System.Serializable]
public class PlayerStats
{
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

    public void AddStat(StatType type, int value)
    {
        switch (type)
        {
            case StatType.Tech: tech += value; break;
            case StatType.Human: human += value; break;
            case StatType.Manager: manager += value; break;
            case StatType.Worker: worker += value; break;
            case StatType.Introvert: introvert += value; break;
            case StatType.Extrovert: extrovert += value; break;
            case StatType.Analyst: analyst += value; break;
            case StatType.Intuitive: intuitive += value; break;
            case StatType.Stability: stability += value; break;
            case StatType.Openness: openness += value; break;
        }
    }
}

public enum StatType
{
    Tech, Human, Manager, Worker,
    Introvert, Extrovert,
    Analyst, Intuitive,
    Stability, Openness
}
