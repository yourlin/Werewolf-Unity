using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using System;

namespace WorldTime
{
    [RequireComponent (typeof (Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private Light2D light;

        [SerializeField]
        private WorldTime worldTime;
        [SerializeField]
        private Gradient gradient;

        private void Awake()
        {
            light = GetComponent<Light2D> ();
            worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            light.color = gradient.Evaluate(PercentOfDay(newTime));
        }

        private float PercentOfDay(TimeSpan timeSpan) {
            return (float)timeSpan.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
        }
    }
}

