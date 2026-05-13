using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public Button buyButton;
    public GameObject ownedBadge; // Галочка "Уже куплено"
    public Image iconImage;

    private ShopItem currentItem;
    private bool isOwned = false;

    public void Setup(ShopItem item)
    {
        currentItem = item;

        // Проверяем, куплен ли предмет
        isOwned = CheckIfOwned(item);

        UpdateUI();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void UpdateUI()
    {
        nameText.text = currentItem.name;
        descriptionText.text = currentItem.description;

        if (isOwned)
        {
            priceText.text = "✓ Владелец";
            priceText.color = Color.green;
            buyButton.interactable = false;
            if (ownedBadge != null) ownedBadge.SetActive(true);
        }
        else
        {
            priceText.text = $"{currentItem.price} 💎";
            priceText.color = Color.white;
            buyButton.interactable = true;
            if (ownedBadge != null) ownedBadge.SetActive(false);
        }
    }

    private bool CheckIfOwned(ShopItem item)
    {
        string key = $"shop_{item.name}_{item.type}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    private void OnBuyClicked()
    {
        if (isOwned) return;

        if (!ShopAdapter.HasEnoughGems(currentItem.price))
        {
            Debug.Log($"Недостаточно монет! Нужно: {currentItem.price}");
            return;
        }

        if (ShopAdapter.SpendGems(currentItem.price))
        {
            MarkAsOwned();
            UpdateUI();
            Debug.Log($"✅ Куплено: {currentItem.name}");
        }
    }

    private void MarkAsOwned()
    {
        string key = $"shop_{currentItem.name}_{currentItem.type}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        isOwned = true;
    }
}