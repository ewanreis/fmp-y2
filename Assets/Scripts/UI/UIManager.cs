using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool isVsyncOn;
    private bool isFullscreen;

    [SerializeField]
    private Toggle vsyncToggle;
    [SerializeField]
    private Toggle fullscreenToggle;

    private void Start()
    {
        GetSavedGraphics();
        
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
}
