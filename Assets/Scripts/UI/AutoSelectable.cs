using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSelectable : MonoBehaviour
{
    [SerializeField] private Button selectableButton;

    public void Select()
    {
        selectableButton.Select ();
    }
}
