using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

namespace CheeseTeam
{
    public class TightRope : Minigame
    {
        private float delta;
        private float maxDelta = .003f;
        private float minDelta = 0.0001f;
        private float sign = 1f;

        public Slider BalanceSlider;
        public Slider LifeSlider;

        public Image DarkRed;
        public Image Red;
        public Image Yellow;
        public Image Green;

        private bool isPlaying = false;

        float maxRed = .95f;
        private float minRed = .5f;
        float maxYellow = .5f;
        private float minYellow = .2f;
        float maxGreen = .1f;
        float minGreen = .05f;

        private float redSize;
        private float yellowSize;
        private float greenSize;

        private float greenIncreaseStrength = .0001f;
        private float redDrainStrength = .0025f;
        private float darkRedDrainStrength = .01f;

        public RectTransform Skater;

        public override bool Setup(int difficulty)
        {
            base.Setup(difficulty);
            LifeSlider.value = 1f;
            BalanceSlider.value = .5f;
            SetDifficulty();
            RandomizeDelta();
            return true;
        }

        void Update()
        {
            if (!isPlaying) return;

            if (LifeSlider.value == 0f) GameOver();

            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow))
            {
                SetDelta(delta-minDelta);
            }

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow))
            {
                SetDelta(delta + minDelta);

            }

            var pullMultiplier = 2.0f;

            if (BalanceSlider.value < .5f)
            {
                var d = (.5f - BalanceSlider.value)/(1.75f - difficulty / 30f);
                SetDelta(delta - d * minDelta * pullMultiplier);
            }
            else
            {
                var d = (BalanceSlider.value - .5f) / (1.75f - difficulty/30f);
                SetDelta(delta + d * minDelta * pullMultiplier);
            }

            BalanceSlider.value = Mathf.Clamp01(BalanceSlider.value + delta);
            Skater.eulerAngles = Skater.eulerAngles.xy(90-(180*BalanceSlider.value));
            UpdateLife();
        }

        void SetDifficulty()
        {
            Vector2 backgroundSize = DarkRed.rectTransform.rect.size;

            redSize = Mathf.Clamp(maxRed - (difficulty / 30f),minRed,maxRed);
            yellowSize = Mathf.Clamp(maxYellow - (difficulty / 25f),minYellow,maxYellow);
            greenSize = Mathf.Clamp(maxGreen - (difficulty / 20f), minGreen, maxGreen);

            Red.rectTransform.sizeDelta = new Vector2(backgroundSize.x*redSize,backgroundSize.y);
            Yellow.rectTransform.sizeDelta = new Vector2(backgroundSize.x*yellowSize,backgroundSize.y);
            Green.rectTransform.sizeDelta = new Vector2(backgroundSize.x*greenSize,backgroundSize.y);
        }

        float CurrentValue() => Mathf.Abs(BalanceSlider.value - .5f);

        public override void TimerEnd()
        {
            isPlaying = false;
            OnGameWin?.Invoke();
        }

        void UpdateLife()
        {
            var currentValue = CurrentValue();
            var sliderValue = LifeSlider.value;

            if (currentValue < greenSize / 2f) sliderValue += greenIncreaseStrength;
            else if (currentValue >= yellowSize/2f)
            {
                if (currentValue <= redSize / 2f) sliderValue -= redDrainStrength;
                else sliderValue -= darkRedDrainStrength;
            }
            LifeSlider.value = Mathf.Clamp01(sliderValue);
        }

        void SetDelta(float newDelta)
        {
            delta = Mathf.Clamp(newDelta, -maxDelta, maxDelta);
        }

        void GameOver()
        {
            if (!isPlaying) return;
            isPlaying = false;
            OnGameLose?.Invoke();
        }

        public override void StartGame()
        {
            isPlaying = true;
            base.StartGame();
        }

        public void RandomizeDelta()
        {
            sign = Mathf.Sign(Random.Range(-10, 10));
            delta = Random.Range(minDelta, maxDelta)*(1.0f+difficulty/20.0f) * sign;
        }
    } 
}
