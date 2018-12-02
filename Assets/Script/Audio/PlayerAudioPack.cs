using UnityEngine;

namespace Script.Audio
{
    [CreateAssetMenu()]
    public class PlayerAudioPack : ScriptableObject
    {
        public AudioEvent
            biteHit,
            biteTry,
            burp,
            fart,
            hurt,
            laugh,
            step;
    }
}