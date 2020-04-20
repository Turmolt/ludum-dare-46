using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    private CanvasGroup CanvasGroup;

    void Reset()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (CanvasGroup == null) CanvasGroup = GetComponent<CanvasGroup>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void FadeScreen(bool endValue, float duration, Action OnComplete)
    {
        CanvasGroup.DOFade(endValue ? 1f:0f, duration).OnComplete(()=>OnComplete());
    }
}

