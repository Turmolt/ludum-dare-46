using System.Collections;
using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CheeseTeam
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private Minigame activeMinigame;

        private MinigameBag minigameBag = new MinigameBag();

        private string activeSceneName;

        private int difficulty = 1;

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

            activeSceneName = string.Empty;
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
            
            //load next scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncLoad.isDone);

            //set active scene name
            activeSceneName = sceneName;

            // Extract minigame class from loaded objects
            Minigame minigame = (Minigame)FindObjectOfType(typeof(Minigame));
            if (minigame)
            {
                activeMinigame = minigame;
                // Subscribe to game events
                minigame.OnGameWin += OnMinigameWon;
                minigame.OnGameLose += OnMinigameLost;

                minigame.Setup(difficulty++);
                //TODO: Fade from intermittent screen then
                minigame.StartGame();
            }
            else 
            {
                Debug.LogError("No minigame was found!!");
            }

        }

        void OnMinigameWon() {
            
        }

        void OnMinigameLost() {
            LoadRandomScene();
        }
    }

    public class MinigameBag
    {
        public static string[] Minigames = {
        "Clear The Way",
        };

        private List<string> bagOfGames = new List<string>();

        public string PopMinigame()
        {
            if (bagOfGames.Count == 0)
            {
                RefillBag();
                if (bagOfGames.Count == 0)
                {
                    Debug.LogError("There are no minigames in the list!");
                    return null;
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

}