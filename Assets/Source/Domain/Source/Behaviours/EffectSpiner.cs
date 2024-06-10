using UnityEngine;

namespace Battlemage.Domain
{
	public class EffectSpiner : MonoBehaviour
	{
		[SerializeField, Range(0f, 10f)]
		private float _radius;

		[SerializeField, Range(1f, 360f)]
		private float _speed;

		private void Update()
		{
			transform.Rotate(0f, 2f * _speed * Time.deltaTime, 0f);
		}
	}
}
