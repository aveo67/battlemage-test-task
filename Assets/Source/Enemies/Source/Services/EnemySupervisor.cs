using Battlemage.Enemies;
using System;

namespace Battlemage.Domain
{
	public class EnemySupervisor
	{
		public event Action EnemyDead;

		public event Action<Enemy> NewEnemy;

		public int EnemyCount { get; private set; }

		public EnemySupervisor(Enemy[] enemies)
		{
			foreach (Enemy enemy in enemies)
			{
				RegisterEnemy(enemy);
			}
		}

		public void RegisterEnemy(Enemy enemy)
		{
			enemy.Dead += OnEnemyDead;
			EnemyCount++;
			NewEnemy?.Invoke(enemy);
		}

		private void OnEnemyDead(Enemy enemy)
		{
			EnemyCount--;
			EnemyDead?.Invoke();
			enemy.Dead -= OnEnemyDead;
		}
	}
}
