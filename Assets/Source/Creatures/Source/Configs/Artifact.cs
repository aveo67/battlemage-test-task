using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Battlemage.Creatures
{
	[CreateAssetMenu(fileName = "Artifact", menuName = "Artifacts/Artifact", order = 1)]
	public class Artifact : ScriptableObject
	{
		[SerializeField]
		private DamageDescriptor _damageModifier;

		[SerializeField]
		private ResistanceModifierDescriptor _resistanceModifier;

		public Damage Damage => _damageModifier != null ? _damageModifier.Damage : default;

		public float Resistance => _resistanceModifier != null ? _resistanceModifier.Value : 0f;

		public override string ToString()
		{
			return $"{Damage}, {_resistanceModifier}";
		}
	}
}
