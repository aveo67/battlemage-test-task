using Battlemage.Domain;
using Battlemage.Spawner;
using Zenject;

namespace Battlemage.Enemies
{
	internal class EnemyDiTag { }


	public static class EnemyDiExtensions
	{
		public static DiContainer UseEnemies(this DiContainer container)
		{
			if (!container.HasBinding<EnemyDiTag>())
			{
				container.Bind<EnemyDiTag>().AsSingle().NonLazy();

				container.UseSpawners();

				container.Bind<EnemySupervisor>().AsSingle().NonLazy();
			}

			return container;
		}
	}
}
