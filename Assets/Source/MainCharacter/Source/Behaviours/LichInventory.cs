using Battlemage.Creatures;
using Battlemage.Spells;
using UnityEngine;
using Zenject;

namespace Battlemage.MainCharacter
{
	[RequireComponent(typeof(Creature))]
	internal class LichInventory : MonoBehaviour
	{
		[SerializeField]
		private Artifact[] _artifacts;

		[SerializeField]
		private Spell[] _spells;

		private SpellHandler[] _handlers;

		private SpellResolver _spellResolver;

		private Creature _creature;

		[Inject]
		private void Construct(SpellResolver spellResolver)
		{
			_spellResolver = spellResolver;
			_handlers = new SpellHandler[_spells.Length];
		}

		private void Awake()
		{
			_creature = GetComponent<Creature>();
			foreach (var a in _artifacts)
			{
				_creature.SetArtifact(a);
			}
		}

		public Damage GetTotalDamage()
		{
			var res = new Damage();

			for (int i = 0; i < _artifacts.Length; ++i)
			{
				var damage = _artifacts[i].Damage;

				res = res.Combine(damage);
			}

			return res;
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

		public SpellHandler GetSpell(int index) 
		{
			if (index >= 0 &&  index < _spells.Length)
			{
				var s = _handlers[index];

				if (s == null)
				{
					s = _spellResolver.Resolve(_spells[index], transform);

					_handlers[index] = s;
				}

				return s;
			}

			return null;
		}

		public override string ToString()
		{
			return $"Inventory. Spells Count: {_spells.Length}, Artifact Count: {_artifacts.Length}, Total Damage: {GetTotalDamage()}, TotalResistance: {GetTotalResistance()}";
		}
	}
}
