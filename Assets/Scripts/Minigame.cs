using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CheeseTeam
{
    public abstract class Minigame : MonoBehaviour
    {

        public enum MinigameState { Start, Gameplay, Paused, End }

        protected int difficulty;
        protected float timeLimit;

        public virtual void Start() {
            // If testing the minigame in isolation, simulate events from GameManager to init
            if (GameManager.instance == null) {
                Setup(0);
                StartGame();
            }
        }

        public virtual void Setup(int difficulty) => this.difficulty = difficulty;

        public virtual void StartGame() { }

        public virtual void Pause() { }

        public virtual void Cleanup() { }

        public Action OnGameWin { get; set; }

        public Action OnGameLose { get; set; }
    } 
}