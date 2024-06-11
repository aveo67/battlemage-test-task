using Battlemage.Creatures;
using System;
using System.Threading;
using UnityEngine;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
	public class BulletHandle : MonoBehaviour
	{
		public Action<BulletHandle> Dead;

		private CancellationTokenSource _tokenSource;

		public Damage Damage { get; internal set; }

		public float LifeTime { get; internal set; }

		public bool StopWhenCollide { get; internal set; }

		private void OnCollisionEnter(Collision collision)
		{
			Debug.Log(collision.gameObject.name);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Ground"))
			{
				Die();
			}

			else
			{
				if (other.TryGetComponent<Creature>(out var creature))
				{
					creature.Hit(Damage);

					if (StopWhenCollide)
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
	}
}
