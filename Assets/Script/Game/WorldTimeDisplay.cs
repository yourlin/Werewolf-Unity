using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WorldTime {
    public class WorldTimeDisplay : MonoBehaviour {
        [SerializeField]
        private WorldTime worldTime;

        private TMP_Text text;

        // Start is called before the first frame update
        void Awake () {
            text = GetComponent<TMP_Text> ();
            worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }
        private void OnWorldTimeChanged (object sender, TimeSpan newTime) {
            text.SetText (newTime.ToString (@"hh\:mm"));
        }
    }

}
