using System;
using System.Collections;
using UnityEngine;

namespace WorldTime {
    public class WorldTime : MonoBehaviour {
        [SerializeField]
        private bool AutoTiming = true;

        [SerializeField]
        private int dayTime = 10;
        [SerializeField]
        private int nightTime = 23;
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField]
        private float dayLength;

        private TimeSpan currentTime;

        private bool isDay;

        private float minuteLength => dayLength / WorldTimeConstants.MinutesInDay;

        // Start is called before the first frame update
        void Start () {
            if (AutoTiming) {
                StartCoroutine (AddMinute ());
            } else {
                SetDayTime ();
            }
        }

        private IEnumerator AddMinute () {
            currentTime += TimeSpan.FromMinutes (1);
            WorldTimeChanged?.Invoke (this, currentTime);
            yield return new WaitForSeconds (minuteLength);
            StartCoroutine (AddMinute ());
        }

        public void SetTime(TimeSpan newTime) {
            currentTime = newTime;
            WorldTimeChanged?.Invoke (this, currentTime);
        }

        public void SetDayTime () {
            isDay = true;
            currentTime = new TimeSpan (dayTime, 0, 0);
            WorldTimeChanged?.Invoke (this, currentTime);
        }

        public void SetNightTime () {
            isDay = false;
            currentTime = new TimeSpan (nightTime, 0, 0);
            WorldTimeChanged?.Invoke (this, currentTime);
        }

        public bool IsDay() {
            return isDay;
        }
    }
}