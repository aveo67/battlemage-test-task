using System;
using UnityEngine;

namespace Battlemage.Creatures
{
	[CreateAssetMenu(fileName = "DemageModifier", menuName = "Modifiers/DemageModifier", order = 1)]
	internal class DemageDescriptor : ScriptableObject
	{
		[SerializeField, Range(1f, 100f)]
		private float _value;

		[SerializeField, Range(0f, 1f)]
		private float _resistanceIgnoring;

		public float Value => _value;

		public float Resistancegnoring => _resistanceIgnoring;
	}
}
