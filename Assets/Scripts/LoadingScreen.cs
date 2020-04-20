using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public Material FadeMaterial;


    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }else if (instance != this)
        {
            Destroy(this);
        }
        FadeMaterial.SetFloat("_Fade",0f);
    }


    public void FadeScreen(bool endValue, float duration, Action OnComplete)
    {
        FadeMaterial.DOFloat(endValue ? 1.01f:-.01f, "_Fade" ,duration).OnComplete(()=>OnComplete());
    }
}

