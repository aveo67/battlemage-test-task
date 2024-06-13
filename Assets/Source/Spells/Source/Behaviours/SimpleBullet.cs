using UnityEngine;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(Rigidbody))]
	internal class SimpleBullet : MonoBehaviour
	{
		[SerializeField, Range(1f, 100f)]
		private float _speed;

		private Rigidbody _rigidBody;

		private void Awake()
		{
			_rigidBody = GetComponent<Rigidbody>();			
		}

		private void OnEnable()
		{
			_rigidBody.velocity = transform.forward * _speed;
		}

		public override string ToString()
		{
			return $"Simple Bullet. Speed: {_speed}";
		}
	}
}
