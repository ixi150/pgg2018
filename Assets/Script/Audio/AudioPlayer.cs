using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Xunity.ScriptableEvents;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : GameEventListenerBase
{
    [SerializeField] protected AudioEvent audioEvent;
    [SerializeField] protected bool playOnEnable;

    protected AudioSource audioSource;

    public AudioEvent AudioEvent
    {
        get { return audioEvent; }
        set { audioEvent = value; }
    }
    
    public virtual void Play()
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