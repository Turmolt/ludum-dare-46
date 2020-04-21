using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CheeseTeam
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private Minigame activeMinigame;

        private MinigameBag minigameBag = new MinigameBag();

        private string activeSceneName;

        private int difficulty = 1;

        //Life Gauge
        private int life;
        public GameObject HeartParent;
        public Image[] HeartImages;

        //Timer Variables
        public TextMeshProUGUI Timer;
        private float maxTime = 10f;
        private float currentTimer;

        private bool isPlaying;

        private Minigame minigame;

        

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

            ResetLife();
            ToggleUI(false);
            activeSceneName = string.Empty;
        }

        /// <summary>
        /// Pulls random scene from MinigameScenes and loads it
        /// </summary>
        public void LoadRandomScene()
        {
            LoadingScreen.instance.FadeScreen(true,1f,()=>
            {
                //if we are loading our first scene, enable the UI components
                if (string.IsNullOrEmpty(activeSceneName))
                {
                    ToggleUI(true);
                }

                var selectedGame = minigameBag.PopMinigame();
                LoadScene(selectedGame);
            });
        }

        void Update()
        {
            if (isPlaying)
            {
                currentTimer -= Time.deltaTime;
                Timer.text = Mathf.Clamp(currentTimer,0,maxTime).ToString("00");
                if (currentTimer <=0)
                {
                    TimerEnd();
                }
            }
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
            minigame = (Minigame)FindObjectOfType(typeof(Minigame));
            if (minigame)
            {
                activeMinigame = minigame;
                // Subscribe to game events
                minigame.OnGameWin += OnMinigameWon;
                minigame.OnGameLose += OnMinigameLost;

                yield return new WaitUntil(()=>minigame.Setup(difficulty++));
                LoadingScreen.instance.FadeScreen(false,1f,()=>
                {
                    StartTimer();
                    minigame.StartGame();
                });
            }
            else 
            {
                Debug.LogError("No minigame was found!!");
            }

        }

        void OnMinigameWon()
        {
            isPlaying = false;
            LoadRandomScene();
        }

        void OnMinigameLost()
        {
            isPlaying = false;
            ReduceLife();
            if (life > 0)
            {
                LoadRandomScene();
            }
            else
            {
                Debug.Log("Game Over");
                GameOver();
            }
        }

        void ToggleUI(bool endState)
        {
            HeartParent.SetActive(endState);
            Timer.gameObject.SetActive(endState);
        }

        public void ResetLife()
        {
            life = HeartImages.Length;
            for (int i = 0; i < HeartImages.Length; i++)
            {
                HeartImages[i].enabled = true;
            }
        }

        void ReduceLife()
        {
            life = Mathf.Clamp(life - 1, 0, HeartImages.Length);
            HeartImages[life].enabled = false;
        }

        void GameOver()
        {
            isPlaying = false;
            LoadingScreen.instance.FadeScreen(true,1.0f, () =>
            {
                Invoke("ResetGame",5.0f);
            }, true);
        }

        void ResetGame()
        {
            ToggleUI(false);
            ResetLife();
            SceneManager.LoadScene("Menu");
            LoadingScreen.instance.FadeScreen(false, 1.0f, () => { }, true);
        }

        void StartTimer()
        {
            isPlaying = true;
            currentTimer = maxTime;
            Timer.text = currentTimer.ToString("00");
        }

        void TimerEnd()
        {
            isPlaying = false;
            minigame.TimerEnd();
        }
    }

    public class MinigameBag
    {
        public static string[] Minigames = {
        "Clear The Way",
        "Operation",
        "Tight Rope"
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