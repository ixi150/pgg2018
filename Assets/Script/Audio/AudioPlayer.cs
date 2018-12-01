using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xunity.ScriptableEvents;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
	[SerializeField] bool playOnEnable;
	[SerializeField] AudioEvent audioEvent;

	AudioSource audioSource;
	
	public void Play()
	{
		audioEvent.Play(audioSource);
	}

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void OnEnable()
	{
		if (playOnEnable)
			Play();
	}

	void OnDisable()
	{
		audioSource.Stop();
	}
}
