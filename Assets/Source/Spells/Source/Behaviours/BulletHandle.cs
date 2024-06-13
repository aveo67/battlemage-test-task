using Battlemage.Creatures;
using System;
using System.Threading;
using UnityEngine;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
	public class BulletHandle : MonoBehaviour
	{
		public event Action<BulletHandle> Dead;

		[SerializeField]
		private bool _ignorGroung = false;

		private CancellationTokenSource _tokenSource;

		public Transform Owner { get; internal set; }

		public Damage Damage { get; internal set; }

		public float LifeTime { get; internal set; }

		public bool StopWhenCollided { get; internal set; }

		public bool AllowFriendlyFire { get; internal set; }

		private void OnCollisionEnter(Collision collision)
		{
			Debug.Log(collision.gameObject.name);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Ground") && !_ignorGroung)
			{
				Die();
			}

			else
			{
				if (other.TryGetComponent<Creature>(out var creature))
				{
					if (creature.transform == Owner && !AllowFriendlyFire)
						return;

					creature.Hit(Damage);

					if (StopWhenCollided)
						Die();
				}
			}
		}

		private void Die()
		{
			_tokenSource.Cancel();
			_tokenSource.Dispose();
			_tokenSource = null;

			Dead?.Invoke(this);
		}

		public async void Release()
		{
			_tokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

			await Awaitable.WaitForSecondsAsync(LifeTime, _tokenSource.Token);

			Die();
		}

		private void OnDisable()
		{
			Damage = default;
			LifeTime = 0f;
			Dead = null;
		}

		public override string ToString()
		{
			return $"Bullet. Owner: {Owner.name}, Ignore Ground: {_ignorGroung}, Life Time: {LifeTime}, Stop When Collided: {StopWhenCollided}, Allow Friendly Fire: {AllowFriendlyFire}";
		}
	}
}
