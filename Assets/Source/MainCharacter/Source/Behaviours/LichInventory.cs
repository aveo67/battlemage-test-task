using Battlemage.Creatures;
using Battlemage.Spells;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class LichInventory : MonoBehaviour
	{
		[SerializeField]
		private Artifact[] _artifacts;

		[SerializeField]
		private Spell[] _spells;

		public Damage GetTotalDamage()
		{
			float totalDamage = 0f;
			float totalIgnoring = 0f;

			for (int i = 0; i < _artifacts.Length; ++i)
			{
				var damage = _artifacts[i].Damage;

				totalDamage += damage.Value;
				totalIgnoring += damage.ResistanceIgnoring;
			}

			return new Damage(totalDamage, totalIgnoring);
		}

		public float GetTotalResistance()
		{
			float totalResistance = 0f;

			for (int i = 0; i < _artifacts.Length; ++i)
			{
				totalResistance += _artifacts[i].Resistance;
			}

			return totalResistance;
		}

		public Spell GetSpell(int index) 
		{
			if (index >= 0 &&  index < _spells.Length)
			{
				return _spells[index];
			}

			return null;
		}
	}
}
