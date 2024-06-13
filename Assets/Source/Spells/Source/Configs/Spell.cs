using Battlemage.Creatures;
using System;
using UnityEngine;

namespace Battlemage.Spells
{
	public enum AttackType
	{
		None,
		Short,
		Long,
	}

	[CreateAssetMenu(fileName = "Spell", menuName = "Artifacts/Spells", order = 1)]
	public class Spell : ScriptableObject
	{
		[SerializeField]
		protected internal DamageDescriptor _damageModifier;

		[SerializeField]
		protected internal SpellHandler _spellHandler;

		[SerializeField, Range(1, 100)]
		protected internal int _bulletNumber;

		[SerializeField]
		protected BulletHandle _bulletPrefab;

		[SerializeField, Range(0.5f, 5f)]
		protected float _bulletLifeTime;

		[SerializeField]
		protected bool _stopBulletWhenCollided;

		[SerializeField, Range(0f, 5f)]
		protected internal float _deley;

		[SerializeField]
		protected AttackType _attackType;

		[SerializeField]
		protected internal bool _allowFriendlyFire;

		[SerializeField]
		public AttackType AttackType => _attackType;



		private void Awake()
		{
			if (_damageModifier == null)
			{
				var message = $"Damage Config not set to Spell Config {name}";

				throw new NullReferenceException(message);
			}

			if (_spellHandler == null)
			{
				var message = $"Spell Handler not set to Spell Config {name}";

				throw new NullReferenceException(message);
			}

			if (_bulletPrefab == null)
			{
				var message = $"Bullet Prefab not set to Spell Config {name}";

				throw new NullReferenceException(message);
			}
		}

		public BulletModel GetBulletModel()
		{
			return new BulletModel
			{
				LifeTime = _bulletLifeTime,
				Number = _bulletNumber,
				Prefab = _bulletPrefab,
				StopWhenCollided = _stopBulletWhenCollided
			};
		}
		public override string ToString()
		{
			return $"Spell: {name}, {GetBulletModel()}, Deley: {_deley}, {_damageModifier}, Allow Friendly Fire: {_allowFriendlyFire}, Attack Type: {_attackType}";
		}
	}
}
