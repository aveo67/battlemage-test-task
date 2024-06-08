using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	[RequireComponent(typeof(LichAnimationHandler))]
	public class LichHandler : MonoBehaviour
	{
		[SerializeField]
		private CharacterController _characterController;

		[SerializeField, Range(1f, 20f)]
		private float _speed = 10f;

		[SerializeField, Range(0.1f, 10f)]
		private float _accelerationDuration = 3f;

		private LichAnimationHandler _animationHandler;

		private float _speedInterpolationTime;

		private Vector3 _direction;

		private MotionStateBase _currentState;

		private Transform _cameraTransform;

		internal bool AchivedMaxSpeed => _speedInterpolationTime >= 1f;

		internal bool IsIdle => _speedInterpolationTime <= 0f;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			
			if (!TryGetComponent(out _animationHandler))
				throw new NullReferenceException("Animation handler not set to main character game object");

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
	}
}