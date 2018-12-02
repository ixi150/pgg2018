using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	[SerializeField] AudioClip[] clips;
	[SerializeField] RangedFloat volume;
	[MinMaxRange(0, 2)]
	[SerializeField] RangedFloat pitch;
	[SerializeField] float time;
	[SerializeField] bool loop;
	[SerializeField] AudioMixerGroup mixer;
	
	public override void Play(AudioSource source)
	{
		if (source==null || clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.loop = loop;
		source.outputAudioMixerGroup = mixer;
		source.Play();
		source.time = time;
	}
}