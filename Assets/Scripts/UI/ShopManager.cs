using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private List<ShopItem> items;

    [SerializeField]
    private int currentPoints;

    [SerializeField]
    private TMP_Text currentPointTextShop;

    [SerializeField]
    private TMP_Text currentPointText;

    [SerializeField]
    private Gradient colorGradient;

    private void Start()
    {
        UpdateShop();
    }

    public void UpdateShop()
    {
        foreach(ShopItem item in items)
        {
            item.shopButton.interactable = (currentPoints >= item.cost) ? true : false;
        }

        // map value from 1 to 10000 to a normalized value between 0 and 1
        float normalizedValue = Mathf.InverseLerp(1, 10000, currentPoints);

        // get colour for current value from colour gradient
        Color color = colorGradient.Evaluate(normalizedValue);

        currentPointTextShop.color = color;
        currentPointTextShop.text = $"{currentPoints}";

        currentPointText.color = color;
        currentPointText.text = $"{currentPoints}";
    }
}

[System.Serializable]
public struct ShopItem
{
    public Button shopButton;
    public int cost;
    public int id;
}