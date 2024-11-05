using UnityEngine;
using System.Collections;
using System;

namespace UDEV
{
    public class TimerText : MonoBehaviour
    {
        public Color normal;
        public Color timeOut;

        public bool countUp = true;
        public bool runOnStart = false;
        public float timeValue = 0;
        float m_curTime = 0;
        public int timeOutNotice = 5;

        public bool showHour = false;
        public bool showMinute = true;
        public bool showSecond = true;

        public Action OnCountDownComplete;
        public Action OnCounting;

        private bool isRunning = false;

        Coroutine m_updateTimer;

        public float CurTime { get => m_curTime;}

        private void Start()
        {
            UpdateText();
            if (runOnStart)
            {
                Run(null);
            }
        }

        public void Run(MonoBehaviour behaviour)
        {
            if (behaviour == null) behaviour = this;

            if (!isRunning)
            {
                isRunning = true;
                m_updateTimer = behaviour.StartCoroutine(UpdateClockText());
            }
        }

        private IEnumerator UpdateClockText()
        {
            while (isRunning)
            {
                UpdateText();
                yield return new WaitForSeconds(1);
                if (countUp) timeValue++;
                else
                {
                    if (timeValue == 0)
                    {
                        isRunning = false;
                    }
                    else
                    {
                        if (OnCounting != null)
                            OnCounting.Invoke();
                        timeValue--;
                    }
                }
            }

            Stop();
            if (OnCountDownComplete != null) OnCountDownComplete();
        }

        public void SetTime(float value)
        {
            if (value < 0)
                value = 0;

            m_curTime = value;
            timeValue = value;
            UpdateText();
        }

        public void UpdateTime(float value)
        {
            timeValue = value;
        }

        public void AddTime(int value)
        {
            timeValue += value;
            UpdateText();
        }

        private void UpdateText()
        {
            TimeSpan t = TimeSpan.FromSeconds(timeValue);

            string text;
            if (showHour && showMinute && showSecond)
            {
                text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            }
            else if (showHour && showMinute)
            {
                text = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
            }
            else if(showMinute)
            {
                text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }
            else
            {
                text = string.Format("{0:D2}", t.Seconds);
            }

            gameObject?.SetText(text);

            if (timeValue <= timeOutNotice)
                gameObject?.SetTextColor(timeOut);
            else
                gameObject?.SetTextColor(normal);
        }

        public void Stop()
        {
            isRunning = false;
            if (m_updateTimer != null)
            {
                m_curTime = timeValue;
                StopCoroutine(m_updateTimer);
            }
        }

        public void Resume()
        {
            isRunning = true;
            m_updateTimer = StartCoroutine(UpdateClockText());
        }

        private void Update()
        {
            if (isRunning)
                m_curTime -= Time.deltaTime;
        }
    }
}
