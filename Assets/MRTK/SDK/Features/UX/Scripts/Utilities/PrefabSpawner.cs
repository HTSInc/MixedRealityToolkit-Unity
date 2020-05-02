﻿using Microsoft.MixedReality.Toolkit.Input;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Microsoft.MixedReality.Toolkit.Utilities;

public class PrefabSpawner :
    BaseFocusHandler,
    IMixedRealityInputHandler,
    IMixedRealityInputHandler<float>
{
    private enum VanishType
    {
        VanishOnFocusExit = 0,
        VanishOnTap,
    }

    private enum AppearType
    {
        AppearOnFocusEnter = 0,
        AppearOnTap,
    }

    public enum RemainType
    {
        Indefinite = 0,
        Timeout,
    }

    [SerializeField, FormerlySerializedAs("toolTipPrefab")]
    protected GameObject prefab = null;

    [Header("Input Settings")]
    [SerializeField]
    [Tooltip("The action that will be used for when to spawn or toggle the tooltip.")]
    private MixedRealityInputAction tooltipToggleAction = MixedRealityInputAction.None;

    [Header("Appear / Vanish Behavior Settings")]
    [SerializeField]
    private AppearType appearType = AppearType.AppearOnFocusEnter;
    [SerializeField]
    private VanishType vanishType = VanishType.VanishOnFocusExit;
    [SerializeField]
    private RemainType remainType = RemainType.Timeout;
    [SerializeField]
    [Range(0f, 5f)]
    private float appearDelay = 0.0f;
    [SerializeField]
    [Range(0f, 5f)]
    private float vanishDelay = 2.0f;
    [SerializeField]
    [Range(0.5f, 10.0f)]
    private float lifetime = 1.0f;

    private float focusEnterTime = 0f;
    private float focusExitTime = 0f;
    private float tappedTime = 0f;

    private GameObject spawnable;

    private async void ShowSpawnable()
    {
        await UpdateSpawnable(focusEnterTime, tappedTime);
    }

    protected void TryCreateSpawnable()
    {
        if (spawnable == null)
        {
            spawnable = Instantiate(prefab);
            spawnable.gameObject.SetActive(false);
            spawnable.transform.position = transform.position;
            spawnable.transform.parent = transform;
        }
    }

    private async Task UpdateSpawnable(float focusEnterTimeOnStart, float tappedTimeOnStart)
    {
        if (appearType == AppearType.AppearOnFocusEnter)
        {
            if (spawnable == null)
            {
                spawnable = Instantiate(prefab);
                spawnable.gameObject.SetActive(false);
                spawnable.transform.position = transform.position;
                spawnable.transform.parent = transform;
            }
            // Wait for the appear delay
            await new WaitForSeconds(appearDelay);
            // If we don't have focus any more, get out of here

            if (!HasFocus)
            {
                return;
            }
        }

        spawnable.gameObject.SetActive(true);

        SpawnableActivated(spawnable);

        while (spawnable.gameObject.activeSelf)
        {
            if (remainType == RemainType.Timeout)
            {
                switch (appearType)
                {
                    case AppearType.AppearOnTap:
                        if (Time.unscaledTime - tappedTime >= lifetime)
                        {
                            spawnable.gameObject.SetActive(false);
                            return;
                        }

                        break;
                    case AppearType.AppearOnFocusEnter:
                        if (Time.unscaledTime - focusEnterTime >= lifetime)
                        {
                            spawnable.gameObject.SetActive(false);
                            return;
                        }

                        break;
                }
            }

            //check whether we're suppose to disappear
            switch (vanishType)
            {
                case VanishType.VanishOnFocusExit:
                    if (!HasFocus)
                    {
                        spawnable.gameObject.SetActive(false);
                    }

                    break;

                case VanishType.VanishOnTap:
                    if (!tappedTime.Equals(tappedTimeOnStart))
                    {
                        spawnable.gameObject.SetActive(false);
                    }

                    break;

                default:
                    if (!HasFocus)
                    {
                        if (Time.time - focusExitTime > vanishDelay)
                        {
                            spawnable.gameObject.SetActive(false);
                        }
                    }
                    break;
            }

            await new WaitForUpdate();
        }
    }

    protected virtual void SpawnableActivated(GameObject spawnable) { }

    /// <inheritdoc />
    public override void OnFocusEnter(FocusEventData eventData)
    {
        base.OnFocusEnter(eventData);

        HandleFocusEnter();
    }

    /// <inheritdoc />
    public override void OnFocusExit(FocusEventData eventData)
    {
        base.OnFocusExit(eventData);

        HandleFocusExit();
    }

    /// <inheritdoc />
    void IMixedRealityInputHandler<float>.OnInputChanged(InputEventData<float> eventData)
    {
        if (eventData.InputData > .95f)
        {
            HandleTap();
        }
    }

    /// <inheritdoc />
    void IMixedRealityInputHandler.OnInputDown(InputEventData eventData)
    {
        if (tooltipToggleAction.Id == eventData.MixedRealityInputAction.Id)
        {
            HandleTap();
        }
    }

    /// <inheritdoc />
    void IMixedRealityInputHandler.OnInputUp(InputEventData eventData) { }

    protected virtual void HandleTap()
    {
        tappedTime = Time.unscaledTime;

        if (spawnable == null || !spawnable.gameObject.activeSelf)
        {
            switch (appearType)
            {
                case AppearType.AppearOnTap:
                    ShowSpawnable();
                    break;
            }
        }
        else
        {
            switch (vanishType)
            {
                case VanishType.VanishOnTap:
                    spawnable.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void HandleFocusEnter()
    {
        focusEnterTime = Time.unscaledTime;

        if (spawnable == null || !spawnable.gameObject.activeSelf)
        {
            switch (appearType)
            {
                case AppearType.AppearOnFocusEnter:
                    ShowSpawnable();
                    break;
            }
        }
    }

    private void HandleFocusExit()
    {
        focusExitTime = Time.unscaledTime;
    }
}
