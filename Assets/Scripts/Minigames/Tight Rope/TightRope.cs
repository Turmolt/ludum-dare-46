using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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

        private float runtime = 0f;
        private float timeToSwitch = 1f;
        private float minTimeToSwitch = .5f;
        private float maxTimeToSwitch = 1.5f;

        private bool isPlaying = false;

        public override void Setup(int difficulty)
        {
            base.difficulty = difficulty;
            BalanceSlider.value = .5f;
            RandomizeDelta();
            base.Setup(difficulty);
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space)&&!isPlaying)
            {
                Setup(1);
                StartGame();
            }

            if (!isPlaying) return;

            BalanceSlider.value = Mathf.Clamp01(BalanceSlider.value + delta);

            if (BalanceSlider.value == 0f || BalanceSlider.value == 1f) GameOver(); 

            //runtime += Time.deltaTime;

            if (runtime >= timeToSwitch)
            {
                runtime = 0f;
                timeToSwitch = Random.Range(minTimeToSwitch, maxTimeToSwitch);
                RandomizeDelta();
            }

            if (Input.GetMouseButton(0))
            {
                delta = Mathf.Clamp(delta - minDelta,-maxDelta,maxDelta);
            }

            if (Input.GetMouseButton(1))
            {
                delta = Mathf.Clamp(delta + minDelta, -maxDelta, maxDelta);
            }

            var pullMultiplier = 2.5f;

            if (BalanceSlider.value < .5f)
            {
                var d = (.5f - BalanceSlider.value)/1.5f;
                SetDelta(delta - d * minDelta * pullMultiplier);
            }
            else
            {
                var d = (BalanceSlider.value - .5f) / 1.5f;
                SetDelta(delta + d * minDelta * pullMultiplier);
            }

        }

        void SetDelta(float newDelta)
        {
            delta = Mathf.Clamp(newDelta, -maxDelta, maxDelta);
        }

        void GameOver()
        {
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

        public void SwitchSign() => sign *= -1f;
    } 
}
