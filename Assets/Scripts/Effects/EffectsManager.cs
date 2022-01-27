using UnityEngine;

namespace Effects
{
	public class EffectsManager : MonoBehaviour
	{
		[SerializeField] private HitEffect _hitEffectPrefab;

		public HitEffect GetHitEffect()
		{
			return Instantiate(_hitEffectPrefab, transform);
		}
	}
}