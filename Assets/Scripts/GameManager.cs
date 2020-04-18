using System.Collections;
using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CheeseTeam
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public static string[] Minigames = {"Empty Minigame",};

        private Minigame activeMinigame;

        private string activeSceneName;

        private List<string> bagOfGames;

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

        /// <summary>
        /// Pulls random scene from MinigameScenes and loads it
        /// </summary>
        public void LoadRandomScene()
        {
            var selectedGame = PopMinigame();

            if (activeSceneName == selectedGame)
            {
                var returningGame = selectedGame;

                selectedGame = PopMinigame();

                bagOfGames.Add(returningGame);
            }

            StartCoroutine(LoadSceneAsync(selectedGame));
        }

        IEnumerator LoadSceneAsync(string sceneName)
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
}
