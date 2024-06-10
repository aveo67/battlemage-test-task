using Battlemage.Creatures;
using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	[RequireComponent(typeof(Creature))]
	public class LichHandler : MonoBehaviour, ITarget
	{
		public Action Dead;

		[SerializeField]
		private CharacterController _characterController;

		[SerializeField]
		private LichAnimationHandler _animationHandler;

		[SerializeField, Range(1f, 20f)]
		private float _speed = 10f;

		[SerializeField, Range(0.1f, 10f)]
		private float _accelerationDuration = 3f;

		private Creature _creature;

		private float _speedInterpolationTime;

		private Vector3 _direction;

		private MotionStateBase _currentState;

		private Transform _cameraTransform;

		public Transform Transform => transform;

		internal bool AchivedMaxSpeed => _speedInterpolationTime >= 1f;

		internal bool IsIdle => _speedInterpolationTime <= 0f;

		public bool IsDead => _creature.Health <= 0f;



		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			_creature = GetComponent<Creature>();
			
			if (_animationHandler == null)
				throw new NullReferenceException("Animation handler not set to main character asset");

			_cameraTransform = Camera.main.transform;

			_currentState = new IdleState(this);
		}

		internal void Accelerate()
		{
			var time = _speedInterpolationTime + Time.deltaTime * _speed / _accelerationDuration;

			_speedInterpolationTime = Mathf.Clamp01(time);
		}

		internal void Deccelerate()
		{
			var time = _speedInterpolationTime - Time.deltaTime * _speed / _accelerationDuration;

			_speedInterpolationTime = Mathf.Clamp01(time);
		}

		internal void Move()
		{
			var cameraDirection = Vector3.Scale(_cameraTransform.forward, new Vector3(1f, 0f, 1f));

			var cameraRotation = Quaternion.LookRotation(cameraDirection, Vector3.up);

			var movementDirection = new Vector3(_direction.x, 0f, _direction.y);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraRotation * Quaternion.LookRotation(movementDirection), 360f * Time.deltaTime);

			var speed = Mathf.Lerp(0f, _speed, _speedInterpolationTime);

			_characterController.Move(speed * Time.deltaTime * transform.forward);

			_animationHandler.SetSpeed(speed, _speed);
		}

		internal void SetNextState(MotionStateBase nextState)
		{
			_currentState = nextState;

			_currentState.Process();
		}

		public void Push(Vector2 direction)
		{
			_direction = direction;

			_currentState.Push();
		}

		public void Brake()
		{
			_currentState.Brake();
		}

		public void Hit(Demage demage)
		{
			_creature.Hit(demage);

			if (_creature.Health > 0f)
			{
				Stun();
			}

			else
			{
				_currentState.Die();
			}
		}

		private void Stun()
		{
			var chance = UnityEngine.Random.Range(0, 5);

            if (chance == 0)
            {
				Awaitable delay = _animationHandler.PlayGettingHit();

				_currentState.Block(delay);
            }
        }

		internal async void Die()
		{
			await _animationHandler.PlayDieing();

			Dead?.Invoke();
		}
	}
}