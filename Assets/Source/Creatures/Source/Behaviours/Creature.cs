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
		private DemageDescriptor _demageModifier;

		private float _health;

		public float Speed => _descriptor.BaseMovementSpeed;

		public float Health => _health;



		private void Awake()
		{
			_health = _descriptor.BaseHealth;
		}

		public void Hit(Demage demage)
		{
			if (_health <= 0f)
				return;

			var totalResistance = Mathf.Clamp(_resistanceModifiers.Sum(n => n.Value) + _descriptor.BaseResistance, -0.85f, 0.85f) * Mathf.Clamp(demage.ResistanceIgnoring, 0f, 1f);

			var totalDemage = Mathf.Clamp(demage.Value * (1f - totalResistance), 0f, float.MaxValue);

			_health -= totalDemage;

			if (_health <= 0f)
				Dead?.Invoke();
		}

		public Demage GetDemage()
		{
			return new Demage(_demageModifier.Value, _demageModifier.Resistancegnoring);
		}
	}
}
