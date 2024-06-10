using Battlemage.Creatures;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Battlemage.Enemies
{
	[RequireComponent(typeof(Creature), typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour
	{
		[SerializeField]
		private GameObject _attackEffectHolder;

		private ITarget _target;

		private Creature _creature;

		private NavMeshAgent _agent;

		private EnemyState _state;

		public bool TargetReached => _agent.remainingDistance <= 2f;

		public bool TargetDead => _target.IsDead;

		[Inject]
		private void Construct(ITarget target)
		{
			_target = target;
		}

		private void Awake()
		{
			if (_attackEffectHolder != null)
				_attackEffectHolder.SetActive(false);

			_creature = GetComponent<Creature>();
			_agent = GetComponent<NavMeshAgent>();
			_agent.speed = _creature.Speed;			
		}

		private void Start()
		{
			SetState(new IdleState(this));
		}

		internal void SetState(EnemyState state)
		{
			_state = state;

			_state.Process();
		}

		internal void Stop()
		{
			_agent.isStopped = true;
		}

		internal void Move()
		{
			_agent.isStopped = false;

			_agent.destination = _target.Transform.position;
		}

		internal async void Bite()
		{
			_agent.Raycast(_target.Transform.position, out var hit);

			if (hit.distance < 2f)
			{
				var demage = _creature.GetDemage();

				_target.Hit(demage);

				if (_attackEffectHolder != null)
				{
					_attackEffectHolder.SetActive(true);

					await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);

					_attackEffectHolder.SetActive(false);
				}
			}
		}
	}
}
