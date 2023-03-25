using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopButtonSound : MonoBehaviour
{
    [SerializeField] private Button button;

    public bool isLocked = false;

    public static event Action OnLockedButtonClick;
    public static event Action OnShopButtonClick;

    private void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void OnButtonClick()
    {
        if (isLocked)
        {
            OnLockedButtonClick.Invoke();
        }
        else if (!isLocked)
        {
            OnShopButtonClick.Invoke();
        }
    }
}
