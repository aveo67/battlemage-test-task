using System;
using UnityEngine;

namespace Battlemage.Creatures
{
	[CreateAssetMenu(fileName = "DamageModifier", menuName = "Modifiers/DamageModifier", order = 1)]
	public class DamageDescriptor : ScriptableObject
	{
		[SerializeField, Range(1f, 100f)]
		private float _value;

		[SerializeField, Range(0f, 1f)]
		private float _resistanceIgnoring;

		public float Value => _value;

		public float Resistancegnoring => _resistanceIgnoring;

		public Damage Damage => new Damage(Value, Resistancegnoring);
	}
}
