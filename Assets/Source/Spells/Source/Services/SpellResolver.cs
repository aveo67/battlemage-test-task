using UnityEngine;
using Zenject;

namespace Battlemage.Spells
{
	internal class SpellDiTag { }

	public static class SpellDiExtensions
	{
		public static DiContainer UseSpells(this DiContainer container)
		{
			if (!container.HasBinding<SpellDiTag>())
			{
				container.Bind<SpellDiTag>().AsSingle().NonLazy();

				container
					.UsePrefabPools()
					.Bind<SpellResolver>().AsSingle();
			}

			return container;
		}
	}

	public class SpellResolver
	{
		private readonly DiContainer _container;

		public SpellResolver(DiContainer container)
		{
			_container = container;
		}

		public SpellHandler Resolve(Spell spell, Transform owner)
		{
			var instance = _container.InstantiatePrefabForComponent<SpellHandler>(spell._spellHandler, owner);
			instance.BulletModel = spell.GetBulletModel();
			instance.DamageModifier = spell._damageModifier;
			instance.AllowFriendlyFire = spell._allowFriendlyFire;
			instance.Deley = spell._deley;
			instance.AttackType = spell.AttackType;

			return instance;
		}
	}
}
