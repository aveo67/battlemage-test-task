using Battlemage.Domain;
using Battlemage.Spawner;
using System;
using UnityEngine;
using Zenject;

namespace Battlemage.Enemies
{
	internal class EnemySpawner : MonoBehaviour, ISpawner
	{
		[SerializeField]
		private Enemy[] _prefabs;

		private PrefabPool<Enemy> _pool;

		private EnemySupervisor _supervisor;

		[Inject]
		private void Construct(PrefabPool<Enemy> pool, EnemySupervisor supervisor)
		{
			_pool = pool;
			_supervisor = supervisor;
		}

		private void Awake()
		{
			if (_prefabs.Length == 0)
			{
				var message = "There is no any enemy prefab";

				Debug.LogError(message, this);

				throw new ArgumentNullException(message);
			}
		}

		public void Spawn()
		{
			var index = UnityEngine.Random.Range(0, _prefabs.Length);

			var enemy = _pool.Get(_prefabs[index]);
			enemy.transform.position = transform.position;
			_supervisor.RegisterEnemy(enemy);
			enemy.Dead += OnEnemyDead;
			enemy.gameObject.SetActive(true);
			enemy.GetAlive();
		}

		private void OnEnemyDead(Enemy enemy)
		{
			enemy.Dead -= OnEnemyDead;
			_pool.Push(enemy);
		}
	}
}
