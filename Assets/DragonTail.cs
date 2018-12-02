using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTail : MonoBehaviour
{
	public static DragonTail Ref;
	
	public AudioPlayer attackSound;
	
	Animator animator;

	public void PlayBorn()
	{
		animator.SetTrigger("born");
	}
	
	public void PlayAttack()
	{
		animator.SetTrigger("attack");
	}

	void SoundAttack()
	{
		attackSound.Play();
	}
	
	void Awake()
	{
		Ref = this;
		animator = GetComponent<Animator>();
	}
}
