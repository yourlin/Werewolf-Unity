using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace WorldTime {

    public class TimeWorldWatcher : MonoBehaviour {
        [SerializeField]
        private WorldTime worldTime;

        [SerializeField]
        private List<Schedule> scheduleList;

        // Start is called before the first frame update
        void Start () {
            worldTime.WorldTimeChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule = scheduleList.FirstOrDefault (s =>
            s.Hour == newTime.Hours &&
            s.Minute == newTime.Minutes);

            schedule?.action?.Invoke ();
        }

        [Serializable]
        private class Schedule {
            public int Hour;
            public int Minute;
            public UnityEvent action;
        }
    }
}