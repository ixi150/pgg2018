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
                audioSources.Add(ae, source);
            }

            return source;
        }
    }
}