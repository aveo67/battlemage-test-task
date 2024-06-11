using System;
using System.Linq;
using UnityEngine;

namespace Battlemage.Creatures
{
	public class Creature : MonoBehaviour
	{
		public Action Dead;

		[SerializeField]
		private CreatureDescriptor _descriptor;

		[SerializeField]
		private ResistanceModifierDescriptor[] _resistanceModifiers;

		[SerializeField]
		private DamageDescriptor _damageModifier;

		private float _health;

		public float Speed => _descriptor.BaseMovementSpeed;

		public float Health => _health;



		private void OnEnable()
		{
			_health = _descriptor.BaseHealth;
		}

		public void Hit(Damage damage)
		{
			if (_health <= 0f)
				return;

			var totalResistance = Mathf.Clamp(_resistanceModifiers.Sum(n => n.Value) + _descriptor.BaseResistance, -0.85f, 0.85f) * Mathf.Clamp(damage.ResistanceIgnoring, 0f, 1f);

			var totalDamage = Mathf.Clamp(damage.Value * (1f - totalResistance), 0f, float.MaxValue);

			_health -= totalDamage;

			if (_health <= 0f)
				Dead?.Invoke();
		}

		public Damage GetDamage()
		{
			return new Damage(_damageModifier.Value, _damageModifier.Resistancegnoring);
		}
	}
}
