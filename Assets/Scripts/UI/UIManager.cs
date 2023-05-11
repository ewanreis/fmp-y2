using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //* Manages various parts of the UI
    [SerializeField] private GameObject soldierInspectionMenu;
    [SerializeField] private TMP_Text soldierInspectText;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fullscreenToggle;

    private bool isVsyncOn;
    private bool isFullscreen;

    private void Start()
    {
        GetSavedGraphics();
    }

    private void OnEnable() 
    {
        SoldierInspect.OnSoldierInspect += InspectSoldier;
    }

    private void OnDisable() 
    {
        SoldierInspect.OnSoldierInspect -= InspectSoldier;
    }

    private void GetSavedGraphics()
    {
        GraphicsData graphics = SaveData.GetGraphicsData();
        isVsyncOn = graphics.isVsync;
        isFullscreen = graphics.isFullscreen;
        vsyncToggle.isOn = isVsyncOn;
        fullscreenToggle.isOn = isFullscreen;
    }

    public void ToggleVsync()
    {
        isVsyncOn = !isVsyncOn;
        QualitySettings.vSyncCount = (isVsyncOn) ? 1 : 0;
        SaveGraphicsData();
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        SaveGraphicsData();
    }

    private void SaveGraphicsData()
    {
        GraphicsData data = new GraphicsData();
        data.isFullscreen = isFullscreen;
        data.isVsync = isVsyncOn;
        SaveData.SaveGraphics(data);
    }

    private void InspectSoldier(Soldier soldier)
    {
        soldierInspectionMenu.SetActive(true);
        soldierInspectText.text = $"Health: {soldier.currentHealth}\nSoldier Type: {soldier.soldierType}\n\nStrength: {soldier.soldierStats.strength}\nDefence: {soldier.soldierStats.defence}\nSpeed: {soldier.soldierStats.speed}\nVision: {soldier.soldierStats.vision}\nStealth: {soldier.soldierStats.stealth}";
    }
}
