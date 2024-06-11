using Battlemage.Creatures;
using UnityEngine;
using Zenject;

namespace Battlemage.Spells
{
	public abstract class SpellHandler : MonoBehaviour
	{
		protected PrefabPool<BulletHandle> _pool;

		protected Transform _parent;

		internal BulletModel BulletModel { get; set; }

		public float Deley { get; internal set; }

		public DamageDescriptor DamageModifier { get; internal set; }

		public bool CanCast { get; protected set; } = true;

		[Inject]
		private void Construct(PrefabPool<BulletHandle> pool)
		{
			_pool = pool;
		}

		protected virtual void Awake()
		{
			_parent = transform.parent;
		}

		protected BulletHandle GetBullet(Damage damage)
		{
			var bullet = _pool.Get(BulletModel.Prefab);
			bullet.Damage = damage;
			bullet.StopWhenCollide = BulletModel.StopWhenCollided;
			bullet.LifeTime = BulletModel.LifeTime;
			bullet.transform.SetParent(null);
			bullet.Dead += (b) =>
			{
				_pool.Push(bullet, BulletModel.Prefab.gameObject);
			};

			return bullet;
		}

		//private void OnBulletDead(BulletHandle bullet)
		//{
		//	bullet.Dead -= OnBulletDead;

		//	_pool.Push(bullet, );
		//}

		public abstract void TakeAim();

		public abstract void Stop();

		public abstract void Release(Damage damage, int bulletNumber);

		public async void Cast(Damage damage)
		{
			if (!CanCast)
				return;

			damage = DamageModifier.Damage.Combine(damage);

			Release(damage, BulletModel.Number);

			Stop();

			CanCast = false;

			await Awaitable.WaitForSecondsAsync(Deley, destroyCancellationToken);

			CanCast = true;

			TakeAim();
		}
	}
}
