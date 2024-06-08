using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LichAnimationHandler : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	/// <summary>
	/// Идентификатор параметра анимации Move
	/// </summary>
	private int _speedParamId;

	/// <summary>
	/// Идентификатор триггера ShortAttack
	/// </summary>
	private int _shortAttackTriggerId;

	/// <summary>
	/// Идентификатор триннера LongAttack
	/// </summary>
	private int _longAttackTriggerId;

	private int _winTriggerId;

	private int _dieTriggerId;

	private int _hitTriggerId;

	private TaskCompletionSource<object> _stateCompletionSource;



	private void Awake()
	{
		if (_animator == null)
		{
			var message = "Animator not set";

			Debug.LogError(message, this);

			throw new NullReferenceException(message);
		}

		InitAnimatorParameters();
	}

	private void InitAnimatorParameters()
	{
		var parameters = _animator.parameters.Select(n => n.nameHash).ToHashSet();

		_speedParamId = ValidateParamId("Speed", parameters);
		_shortAttackTriggerId = ValidateParamId("ShortAttack", parameters);
		_longAttackTriggerId = ValidateParamId("LongAttack", parameters);
		_hitTriggerId = ValidateParamId("Hit", parameters);
		_winTriggerId = ValidateParamId("Win", parameters);
		_dieTriggerId = ValidateParamId("Die", parameters);
	}

	private int ValidateParamId(string paramName, HashSet<int> parameters)
	{
		var id = Animator.StringToHash(paramName);

		if (!parameters.Contains(id))
		{
			var message = $"There is no parameter with name {paramName} in the animator";

			Debug.LogError(message, this);

			throw new ArgumentException(message);
		}

		return id;
	}

	internal void EndCurrentState() 
	{
		_stateCompletionSource.SetResult(null);
		_stateCompletionSource = null;
	}

	private async Awaitable PlayAnimationState(int id)
	{
		_stateCompletionSource = new TaskCompletionSource<object>();

		_animator.SetTrigger(id);

		await _stateCompletionSource.Task;
	}

	public async Awaitable PlayShortAttack()
	{
		await PlayAnimationState(_shortAttackTriggerId);
	}

	public async Awaitable PlayLongAttack()
	{
		await PlayAnimationState(_longAttackTriggerId);
	}

	public async Awaitable PlayGettingHit()
	{
		await PlayAnimationState(_hitTriggerId);
	}

	public async Awaitable PlayWinning()
	{
		await PlayAnimationState(_winTriggerId);
	}

	public async Awaitable PlayDieing()
	{
		await PlayAnimationState(_dieTriggerId);
	}

	public void SetSpeed(float speed, float maxSpeed)
	{
		speed = Mathf.InverseLerp(0f, maxSpeed, speed);

		_animator.SetFloat(_speedParamId, speed);
	}
}
