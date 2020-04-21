using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public Material FadeMaterial;

    public Texture[] FadeTextures;

    public Texture LoseLogo;
    public Texture Logo;

    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            FadeMaterial.SetFloat("_Fade", 0f);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }


    public void FadeScreen(bool endValue, float duration, Action OnComplete, bool endScreen=false)
    {
        FadeMaterial.SetVector("_Resolution",new Vector4(Screen.width,Screen.height,0,0));
        FadeMaterial.SetTexture("_Logo",endScreen?LoseLogo:Logo);
        FadeMaterial.SetTexture("_FadeTexture",FadeTextures[Random.Range(0,FadeTextures.Length)]);
        FadeMaterial.DOFloat(endValue ? 1.01f:-.01f, "_Fade" ,duration).SetEase(Ease.Linear).OnComplete(()=>OnComplete());
    }
}

