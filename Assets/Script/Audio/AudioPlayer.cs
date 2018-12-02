using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xunity.ScriptableEvents;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : GameEventListener
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

    protected override void OnEnable()
    {
        base.OnEnable();

        if (playOnEnable)
            Play();
    }

    public override void OnEventRaised()
    {
        base.OnEventRaised();
        Play();
    }

    void OnDisable()
    {
        audioSource.Stop();
    }
}