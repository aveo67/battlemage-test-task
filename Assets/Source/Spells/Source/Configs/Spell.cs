using Battlemage.Creatures;
using System;
using UnityEngine;
using Zenject;

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
		protected DamageDescriptor _damageModifier;

		[SerializeField]
		protected SpellHandler _spellHandler;

		[SerializeField]
		protected int _bulletNumber;

		[SerializeField]
		protected BulletHandle _bulletPrefab;

		[SerializeField]
		protected float _bulletLifeTime;

		[SerializeField]
		protected bool _stopBulletWhenCollided;

		[SerializeField, Range(0f, 5f)]
		protected float _deley;

		[SerializeField]
		protected AttackType _attackType;

		private SpellHandler _instance;

		public AttackType AttackType => _attackType;


		public SpellHandler GetSpellHandler(DiContainer container, Transform owner) 
		{
			if (_instance == null)
			{
				_instance = container.InstantiatePrefabForComponent<SpellHandler>(_spellHandler, owner);
				_instance.BulletModel = GetBulletModel();
				_instance.DamageModifier = _damageModifier;
				_instance.Deley = _deley;
			}			

			return _instance;
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
	}
}
