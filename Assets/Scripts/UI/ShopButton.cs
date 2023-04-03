using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopButton : MonoBehaviour
{
    public static event Action OnLockedButtonClick;
    public static event Action OnShopButtonClick;
    public static event Action<int> OnItemBuy;

    public bool isLocked = false;

    [SerializeField] private Button button;
    [SerializeField] private int itemID;

    private void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void SetItemID(int id) => itemID = id;

    public void OnButtonClick()
    {
        if (isLocked)
        {
            OnLockedButtonClick.Invoke();
        }
        else if (!isLocked)
        {
            OnShopButtonClick.Invoke();
            OnItemBuy.Invoke(itemID);
        }
    }
}
