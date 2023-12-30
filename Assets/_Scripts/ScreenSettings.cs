using GravityPong;
using GravityPong.Menu.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] private bool EnableDebug;

    private ISettingsData _settingsData;
    private Vector2 _mainRes;

    // Use this for initialization
    private void Awake()
    {
        if (!Application.isMobilePlatform && !EnableDebug)
        {
            this.enabled = false;
            return;
        }

        _settingsData = Services.Instance.Get<ISettingsData>();

        _mainRes = new Vector2(Screen.width, Screen.height);

        SubscribeToEvents();
    }

    private void OnDestroy()
        => UnsubscribeFromEvents();

    private void OnScreenScaleChanged(float value)
    {
        Screen.SetResolution((int)(_mainRes.x * value), (int)(_mainRes.y * value), Screen.fullScreenMode);

        if (EnableDebug)
            Debug.Log("X " + Screen.width + " / Y " + Screen.height + " / scale " + value);
    }
    private void OnVSyncChanged(int value)
    {
        QualitySettings.vSyncCount = value;

        if (EnableDebug)
            Debug.Log($"Vsync: {QualitySettings.vSyncCount}");
    }
    private void OnTargetFramerateChanged(int value)
    {
        Application.targetFrameRate = value;

        if (EnableDebug)
            Debug.Log($"Max FPS: {Application.targetFrameRate}");
    }

    private void SubscribeToEvents()
    {
        _settingsData.OnScreenScaleSettingsChanged += OnScreenScaleChanged;
        _settingsData.OnVSyncSettingsChanged += OnVSyncChanged;
        _settingsData.OnTargetFramerateSettingsChanged += OnTargetFramerateChanged;

        OnScreenScaleChanged(_settingsData.ScreenScale);
        OnVSyncChanged(_settingsData.VSync);
        OnTargetFramerateChanged(_settingsData.TargetFramerate);
    }
    private void UnsubscribeFromEvents()
    {
        if (_settingsData == null)
            return;

        _settingsData.OnScreenScaleSettingsChanged -= OnScreenScaleChanged;
        _settingsData.OnVSyncSettingsChanged -= OnVSyncChanged;
        _settingsData.OnTargetFramerateSettingsChanged -= OnTargetFramerateChanged;
    }
}