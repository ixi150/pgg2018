using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Xunity.ScriptableEvents;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : GameEventListenerBase
{
    [SerializeField] AudioEvent audioEvent;
    [SerializeField] bool playOnEnable;

    AudioSource audioSource;

    public void Play()
    {
        if (audioEvent)
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
        Play();
    }

    void OnDisable()
    {
        audioSource.Stop();
    }
}