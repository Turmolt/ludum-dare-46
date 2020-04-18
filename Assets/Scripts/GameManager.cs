using System.Collections;
using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CheeseTeam
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

<<<<<<< HEAD
        public static string[] Minigames = {"Empty Minigame",};

        private Minigame activeMinigame;

        private string activeSceneName;

        private List<string> bagOfGames;
=======
    private Minigame activeMinigame;

    private MinigameBag minigameBag = new MinigameBag();

    private string activeSceneName;
>>>>>>> 79690ade151302ee1946f7e37e32b4ea3914881d

        void Start()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }

            bagOfGames = new List<string>();
        }

        public void SetActiveMinigame(Minigame game)
        {
            this.activeMinigame = game;
        }
<<<<<<< HEAD

        /// <summary>
        /// Pulls random scene from MinigameScenes and loads it
        /// </summary>
        public void LoadRandomScene()
        {
            var selectedGame = PopMinigame();
=======
    }
>>>>>>> 79690ade151302ee1946f7e37e32b4ea3914881d

            if (activeSceneName == selectedGame)
            {
                var returningGame = selectedGame;

<<<<<<< HEAD
                selectedGame = PopMinigame();

                bagOfGames.Add(returningGame);
            }

            StartCoroutine(LoadSceneAsync(selectedGame));
        }

        IEnumerator LoadSceneAsync(string sceneName)
=======
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
>>>>>>> 79690ade151302ee1946f7e37e32b4ea3914881d
        {
            //unload previous scene if there is one
            if (!string.IsNullOrEmpty(activeSceneName))
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(activeSceneName);
                yield return new WaitUntil(() => asyncUnload.isDone);
            }

            //load next scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => asyncLoad.isDone);

            //set active scene name
            activeSceneName = sceneName;
        }

<<<<<<< HEAD
        string PopMinigame()
        {
            if (bagOfGames.Count == 0)
            {
                LoadBag();
            }

            var index = Random.Range(0, bagOfGames.Count);
            var selected = bagOfGames[index];
            bagOfGames.Remove(selected);
            return selected;
        }

        void LoadBag()
        {
            bagOfGames.AddRange(Minigames);
        }
    } 
=======
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
>>>>>>> 79690ade151302ee1946f7e37e32b4ea3914881d
}
