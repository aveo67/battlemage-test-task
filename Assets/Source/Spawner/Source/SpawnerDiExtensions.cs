using Zenject;

namespace Battlemage.Spawner
{
	internal class SpawnerDiTag { }

	public static class SpawnerDiExtensions
	{
		public static DiContainer UseSpawners(this DiContainer container)
		{
			if (!container.HasBinding<SpawnerDiTag>())
			{
				container.Bind<SpawnerDiTag>().AsSingle().NonLazy();

				//container.Bind<SpawnService>().AsSingle().NonLazy();
			}

			return container;
		}
	}
}
