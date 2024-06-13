using Battlemage.Enemies;
using Battlemage.MainCharacter;
using Battlemage.Spawner;
using EasyInputHandling;
using UnityEngine;
using Zenject;

namespace Battlemage.Domain
{
	public class BattleSceneRoot : MonoBehaviour, IInitializable
	{
		[SerializeField]
		private int _enemyCount;

		[SerializeField]
		private int _goal;

		private LichHandler _mainCharacter;

		private IInput _input;

		private EnemySupervisor _supervisor;

		private int _kills;

		private int _totalEnemyCount;

		private SpawnService _spawnService;

		[Inject]
		private void Construct(LichHandler lichHandler, IInputFactory<LichHandler> inputFactory, EnemySupervisor supervisor, SpawnService spawnService)
		{
			_mainCharacter = lichHandler;
			_input = inputFactory.Create(lichHandler);
			_supervisor = supervisor;
			_supervisor.EnemyDead += OnEnemyDead;
			_supervisor.NewEnemy += OnEnemyAdded;
			_spawnService = spawnService;
			lichHandler.Dead += OnMainCharacterDead;
		}

		private async void Start()
		{
			while (_supervisor.EnemyCount < _enemyCount)
			{
				_spawnService.Spawn(_mainCharacter.transform.position);

				await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);
			}
		}

		private void OnEnemyAdded(Enemy enemy)
		{
			_totalEnemyCount++;
			enemy.SetTarget(_mainCharacter);
		}

		private void OnEnemyDead()
		{
			_kills++;

			if (_kills == _goal)
			{
				//Win

				Debug.Log("You Won!");

				return;
			}

			if (_totalEnemyCount < _goal && _supervisor.EnemyCount < _enemyCount)
			{
				Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);

				_spawnService.Spawn(_mainCharacter.transform.position);
			}
		}

		private void OnMainCharacterDead()
		{
			//Lose

			Debug.Log("You Dead!");
		}

		public void Initialize()
		{
			_input.Enable();
		}

		private void OnDestroy()
		{
			_input.Dispose();
			_mainCharacter.Dead -= OnMainCharacterDead;
			_supervisor.EnemyDead -= OnEnemyDead;
			_supervisor.NewEnemy -= OnEnemyAdded;

		}
	}
}
