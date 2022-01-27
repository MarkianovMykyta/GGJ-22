using UnityEngine;

namespace Effects
{
	public class HitEffect : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _particleSystem;

		public void PlayEffect(Color color)
		{
			var main = _particleSystem.main;
			main.startColor = color;

			gameObject.SetActive(true);
			
			Destroy(gameObject, main.startLifetime.constant);
		}
	}
}