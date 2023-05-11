using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSelectable : MonoBehaviour
{
    //* Used for auto-selecting buttons on events, useful for controller menu navigation
    [SerializeField] private Button selectableButton;

    public void Select()
    {
        selectableButton.Select ();
    }
}
