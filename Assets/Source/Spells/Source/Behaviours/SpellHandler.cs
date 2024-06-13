using Battlemage.Creatures;
using System;
using UnityEngine;
using Zenject;

namespace Battlemage.Spells
{
	public abstract class SpellHandler : MonoBehaviour
	{
		protected PrefabPool<BulletHandle> _pool;

		protected Transform _parent;

		private bool _activated = false;

		internal BulletModel BulletModel { get; set; }

		public float Deley { get; internal set; }

		public DamageDescriptor DamageModifier { get; internal set; }

		public bool AllowFriendlyFire { get; internal set; }

		public AttackType AttackType { get; internal set; }

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
			bullet.StopWhenCollided = BulletModel.StopWhenCollided;
			bullet.LifeTime = BulletModel.LifeTime;
			bullet.Owner = _parent;
			bullet.AllowFriendlyFire = AllowFriendlyFire;
			bullet.transform.SetParent(null);
			bullet.Dead += OnBulletDead;

			return bullet;
		}

		private void OnBulletDead(BulletHandle bullet)
		{
			bullet.Dead -= OnBulletDead;

			_pool.Push(bullet);
		}

		protected abstract void TakeAim();

		protected abstract void Stop();

		public void Activate()
		{
			_activated = true;

			if (CanCast)
			{
				TakeAim();
			}
		}

		public void Deactivate()
		{
			_activated = false;

			Stop();
		}

		public abstract void Release(Ray ray, Damage damage, int bulletNumber);

		public async Awaitable Cast(Ray ray, Damage damage)
		{
			if (!CanCast)
				return;

			damage = DamageModifier.Damage.Combine(damage);

			Release(ray, damage, BulletModel.Number);

			Stop();

			CanCast = false;

			try
			{
				await Awaitable.WaitForSecondsAsync(Deley, destroyCancellationToken);
			}

			catch (OperationCanceledException)
			{
				Debug.Log("Spell casting has stoped because game object was destroyed");

				return;
			}

			CanCast = true;

			if (_activated)
				TakeAim();
		}

		public override string ToString()
		{
			return $"Spell: {name}, Activation Status: {_activated}, Parent: {_parent.name}, {BulletModel}, Deley: {Deley}, {DamageModifier}, Allow Friendly Fire: {AllowFriendlyFire}, Attack Type: {AttackType}, Can Cast: {CanCast}";
		}
	}
}
