using Battlemage.Spells;
using Zenject;

namespace Battlemage.MainCharacter
{
	internal class MainCharacterDiTag { }

	public static class MainCharacterDiExtensions
	{
		public static DiContainer UseMainCharacter(this DiContainer container)
		{
			if (!container.HasBinding<MainCharacterDiTag>())
			{
				container.Bind<MainCharacterDiTag>().AsSingle().NonLazy();

				container.UseSpells();
			}

			return container;
		}
	}
}
