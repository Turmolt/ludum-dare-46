﻿using System.Collections;
using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Minigame activeMinigame;

    private MinigameBag minigameBag = new MinigameBag();

    private string activeSceneName;

    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetActiveMinigame(Minigame game)
    {
        this.activeMinigame = game;
    }

    /// <summary>
    /// Pulls random scene from MinigameScenes and loads it
    /// </summary>
    public void LoadRandomScene()
    {
        var selectedGame = minigameBag.PopMinigame();
        LoadScene(selectedGame);
    }

    public void LoadScene(string minigameName)
    {
        StartCoroutine(LoadSceneAsync(minigameName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        //unload previous scene if there is one
        if (!string.IsNullOrEmpty(activeSceneName))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(activeSceneName);
            yield return new WaitUntil(()=>asyncUnload.isDone);
        }

        //load next scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(()=>asyncLoad.isDone);

        //set active scene name
        activeSceneName = sceneName;
    }
}

public class MinigameBag 
{
    public static string[] Minigames = {
        "Empty Minigame",
    };

    private List<string> bagOfGames = new List<string>();

    public string PopMinigame()
    {
        if (bagOfGames.Count == 0)
        {
            RefillBag();
            if (bagOfGames.Count == 0) {
                Debug.LogError("There are no minigames in the list!");
                return;
            }
        }
        Debug.Log(bagOfGames);
        var index = Random.Range(0, bagOfGames.Count - 1);
        var selected = bagOfGames[index];
        bagOfGames.Remove(selected);
        return selected;
    }

    private void RefillBag()
    {
        bagOfGames.AddRange(MinigameBag.Minigames);
    }
}
