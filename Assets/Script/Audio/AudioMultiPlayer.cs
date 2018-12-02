using System.Collections.Generic;
using UnityEngine;

namespace Script.Audio
{
    public class AudioMultiPlayer : AudioPlayer
    {
        readonly Dictionary<AudioEvent, AudioSource> audioSources
            = new Dictionary<AudioEvent, AudioSource>();

        public override void Play()
        {
            if (audioEvent)
                audioEvent.Play(GetOrCreateSource(audioEvent));
        }

        AudioSource GetOrCreateSource(AudioEvent ae)
        {
            AudioSource source;
            if (!audioSources.TryGetValue(ae, out source))
            {
                source = gameObject.AddComponent<AudioSource>();
                source.spatialBlend = audioSource.spatialBlend;
                source.minDistance = audioSource.minDistance;
                source.maxDistance = audioSource.maxDistance;
                source.SetCustomCurve(AudioSourceCurveType.CustomRolloff,
                    audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
                audioSources.Add(ae, source);
            }

            return source;
        }
    }
}