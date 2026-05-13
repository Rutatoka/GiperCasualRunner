using UnityEngine;

public static class ShopAdapter
{
    public static bool HasEnoughGems(int price)
    {
        // ¬сегда работаем через PlayerPrefs, так как в магазине нет HappinessSystem
        int gems = PlayerPrefs.GetInt("Gems", 0);
        return gems >= price;
    }

    public static bool SpendGems(int price)
    {
        int gems = PlayerPrefs.GetInt("Gems", 0);
        if (gems >= price)
        {
            PlayerPrefs.SetInt("Gems", gems - price);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static int GetGems()
    {
        return PlayerPrefs.GetInt("Gems", 0);
    }
}