using System;
using UnityEngine;

namespace Environment.ScriptedEvents
{
	public class EvilTriggerCity : MonoBehaviour
	{
		[SerializeField] private Animator _evilAnimator;
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Player"))
			{
				_evilAnimator.SetTrigger("AppearCity");
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.CompareTag("Player"))
			{
				_evilAnimator.SetTrigger("HideCity");
			}
		}
	}
}