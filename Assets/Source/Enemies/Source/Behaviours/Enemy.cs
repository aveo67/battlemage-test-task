using Battlemage.Creatures;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Battlemage.Enemies
{
	[RequireComponent(typeof(Creature), typeof(NavMeshAgent), typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class Enemy : MonoBehaviour
	{
		public event Action<Enemy> Dead;

		[SerializeField]
		private GameObject _attackEffectHolder;

		[SerializeField]
		private GameObject _preservedTarget;

		private ITarget _target;

		private Creature _creature;

		private NavMeshAgent _agent;

		private Rigidbody _rigidBody;

		private Collider _collider;

		private EnemyState _state;

		public bool TargetReached => _agent.remainingDistance <= 2f;

		public bool TargetDead => _target != null ? _target.IsDead : true;

		public bool HasTarget => _target != null;

		public bool IsDead => _creature.Health <= 0f;



		private void OnEnable()
		{
			_creature.Dead += OnDead;
		}

		public void GetAlive()
		{
			_state?.Reset();
		}

		private void OnDisable()
		{
			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

			_rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
			_agent.enabled = true;
			_collider.enabled = true;

			_creature.Dead -= OnDead;
			Dead = null;
		}

		private void OnDestroy()
		{
			_creature.Dead -= OnDead;
			Dead = null;
		}

		private void Awake()
		{
			if (_preservedTarget != null && _preservedTarget.TryGetComponent<ITarget>(out var t))
			{
				_target = t;
			}

			if (_attackEffectHolder != null)
				_attackEffectHolder.SetActive(false);

			_creature = GetComponent<Creature>();
			_agent = GetComponent<NavMeshAgent>();
			_rigidBody = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();
			_agent.speed = _creature.Speed;

		}

		private void Start()
		{
			SetState(new IdleState(this));
		}

		public void SetTarget(ITarget target)
		{
			if (target != null && _target != target)
			{
				_target = target;
				_state?.Reset();
			}
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
				var damage = _creature.GetDamage();

				_target.Hit(damage);

				if (_attackEffectHolder != null)
				{
					_attackEffectHolder.SetActive(true);

					await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);

					_attackEffectHolder.SetActive(false);
				}
			}
		}

		private void OnDead()
		{
			_creature.Dead -= OnDead;

			_state.Dead();
		}

		public override string ToString()
		{
			return $"Enemy. {_creature}, Curent State: {_state.GetType().Name}";
		}

		internal async void AnimateDead()
		{
			_rigidBody.constraints = RigidbodyConstraints.None;
			_agent.enabled = false;

			await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);

			_collider.enabled = false;

			await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);

			Dead?.Invoke(this);
		}
	}
}
