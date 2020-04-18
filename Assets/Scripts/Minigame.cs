using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Minigame : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.SetActiveMinigame(this);
    }

    protected int difficulty;

    public virtual void Setup(int difficulty) => this.difficulty = difficulty;

    public virtual void StartGame(){}

    public virtual void Pause() { }

    public virtual void Cleanup() { }

    public Action OnGameWin { get; set; }

    public Action OnGameLose { get; set; }
}