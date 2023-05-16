using UnityEngine;

public class SoldierUpgrade : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] componentsToToggle;

    private void Start()
    {
        SetUpgradeLevel(1);
    }

    public void SetUpgradeLevel(int level)
    {
        foreach (MonoBehaviour component in componentsToToggle)
    
            component.enabled = false;
    
        componentsToToggle[level].enabled = true;
    }

}
