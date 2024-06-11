using Battlemage.Creatures;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	[CreateAssetMenu(fileName = "Artifact", menuName = "Artifacts/Artifact", order = 1)]
	internal class Artifact : ScriptableObject
	{
		[SerializeField]
		private DamageDescriptor _damageModifier;

		[SerializeField]
		private ResistanceModifierDescriptor _resistanceModifier;

		public Damage Damage => _damageModifier?.Damage ?? default;

		public float Resistance => _resistanceModifier?.Value ?? 0f;
	}
}
