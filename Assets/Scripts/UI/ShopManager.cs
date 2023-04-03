using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private List<ShopItem> items;
    [SerializeField] private int currentPoints;
    [SerializeField] private TMP_Text currentPointTextShop;
    [SerializeField] private TMP_Text currentPointText;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private Color lockedButtonColour;
    [SerializeField] private Color lockedButtonColourSelected;
    [SerializeField] private Color lockedButtonColourPressed;
    [SerializeField] private Color buttonColour;
    [SerializeField] private Color buttonColourSelected;
    [SerializeField] private Color buttonColourPressed;

    private void Start()
    {
        UpdateShop();
        ShopButton.OnItemBuy += BuyItem;
        PointsPerMinute.OnGainPoints += UpdateShop;
    }

    public void UpdateShop()
    {
        currentPoints = scoreManager.GetCurrentPoints();
        List<ShopItem> modifiedItems = new List<ShopItem>();

        foreach (ShopItem item in items)
        {
            ShopItem modifiedItem = new ShopItem();
            modifiedItem = item;
            modifiedItem.locked = (currentPoints >= item.cost) ? false : true;
            var colors = modifiedItem.shopButton.colors;
            ShopButton shopButton = modifiedItem.shopButton.GetComponent<ShopButton>();
            shopButton.SetItemID(item.id);

            if(modifiedItem.locked)
            {
                colors.normalColor = lockedButtonColour;
                colors.highlightedColor = lockedButtonColourSelected;
                colors.selectedColor = lockedButtonColourSelected;
                colors.pressedColor = lockedButtonColourPressed;
                shopButton.isLocked = true;
            }

            else
            {
                colors.normalColor = buttonColour;
                colors.highlightedColor = buttonColourSelected;
                colors.selectedColor = buttonColourSelected;
                colors.pressedColor = buttonColourPressed;
                shopButton.isLocked = false;
            }

            modifiedItem.shopButton.colors = colors;
            modifiedItems.Add(modifiedItem);
        }

        items = modifiedItems;

        // map value from 1 to 10000 to a normalized value between 0 and 1
        float normalizedValue = Mathf.InverseLerp(1, 10000, currentPoints);

        // get colour for current value from colour gradient
        Color color = colorGradient.Evaluate(normalizedValue);

        currentPointTextShop.color = color;
        currentPointTextShop.text = $"{currentPoints}";

        currentPointText.color = color;
        currentPointText.text = $"{currentPoints}";
    }

    private void BuyItem(int itemID)
    {
        scoreManager.SubtractPoints(items[itemID].cost);
        UpdateShop();
    }
}

[System.Serializable]
public struct ShopItem
{
    public Button shopButton;
    public int cost;
    public int id;
    public bool locked;
}