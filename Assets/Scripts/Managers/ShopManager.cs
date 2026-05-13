using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("Navigation")]
    public Button backButton;
    public Button cosmeticsTab;
    public Button boostersTab;
    public Button customizationTab;

    [Header("Panels")]
    public GameObject cosmeticsPanel;
    public GameObject boostersPanel;
    public GameObject customizationPanel;

    [Header("Cosmetics")]
    public Transform cosmeticsGrid;
    public GameObject cosmeticsItemPrefab;

    [Header("Boosters")]
    public Transform boostersGrid;
    public GameObject boosterItemPrefab;

    [Header("Customization")]
    public Transform customizationGrid;
    public GameObject customizationItemPrefab;

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.GoToMenu());

        // Табы
        cosmeticsTab.onClick.AddListener(() => SwitchTab(0));
        boostersTab.onClick.AddListener(() => SwitchTab(1));
        customizationTab.onClick.AddListener(() => SwitchTab(2));

        LoadItems();
    }

    private void SwitchTab(int index)
    {
        cosmeticsPanel.SetActive(index == 0);
        boostersPanel.SetActive(index == 1);
        customizationPanel.SetActive(index == 2);
    }

    private void LoadItems()
    {
        // Загружаем косметику
        foreach (var item in GetCosmeticsList())
        {
            var obj = Instantiate(cosmeticsItemPrefab, cosmeticsGrid);
            obj.GetComponent<ShopItemUI>().Setup(item);
        }

        // Загружаем бустеры
        foreach (var item in GetBoostersList())
        {
            var obj = Instantiate(boosterItemPrefab, boostersGrid);
            obj.GetComponent<ShopItemUI>().Setup(item);
        }

        // Загружаем кастомизацию
        foreach (var item in GetCustomizationList())
        {
            var obj = Instantiate(customizationItemPrefab, customizationGrid);
            obj.GetComponent<ShopItemUI>().Setup(item);
        }
    }

    private List<ShopItem> GetCosmeticsList()
    {
        return new List<ShopItem>
        {
            new ShopItem("Аватар программиста", "Стильный образ", 100, ShopItemType.Cosmetic),
            new ShopItem("Дизайнерский костюм", "Для творческих", 150, ShopItemType.Cosmetic),
        };
    }

    private List<ShopItem> GetBoostersList()
    {
        return new List<ShopItem>
        {
            new ShopItem("Удвоить опыт", "30 минут", 50, ShopItemType.Booster),
            new ShopItem("Ускорить тест", "Мгновенно", 75, ShopItemType.Booster),
        };
    }

    private List<ShopItem> GetCustomizationList()
    {
        return new List<ShopItem>
        {
            new ShopItem("Тёмная тема", "Стиль интерфейса", 200, ShopItemType.Customization),
            new ShopItem("Анимированный фон", "Красиво", 300, ShopItemType.Customization),
        };
    }
}

public class ShopItem
{
    public string name;
    public string description;
    public int price;
    public ShopItemType type;

    public ShopItem(string name, string description, int price, ShopItemType type)
    {
        this.name = name;
        this.description = description;
        this.price = price;
        this.type = type;
    }
}

public enum ShopItemType
{
    Cosmetic,
    Booster,
    Customization
}