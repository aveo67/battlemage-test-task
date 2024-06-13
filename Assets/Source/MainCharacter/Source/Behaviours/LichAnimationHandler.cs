using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LichAnimationHandler : MonoBehaviour
{
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
		_animator = GetComponent<Animator>();

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
		if (_stateCompletionSource == null)
			Debug.Log("Bip");

		_stateCompletionSource.SetResult(null);
		_stateCompletionSource = null;
	}

	private async Task PlayAnimationState(int id)
	{
		_stateCompletionSource = new TaskCompletionSource<object>();

		_animator.SetTrigger(id);

		await _stateCompletionSource.Task;
	}

	public async Task PlayShortAttack()
	{
		await PlayAnimationState(_shortAttackTriggerId);
	}

	public async Task PlayLongAttack()
	{
		await PlayAnimationState(_longAttackTriggerId);
	}

	public async Task PlayGettingHit()
	{
		await PlayAnimationState(_hitTriggerId);
	}

	public async Task PlayWinning()
	{
		await PlayAnimationState(_winTriggerId);
	}

	public async Task PlayDieing()
	{
		await PlayAnimationState(_dieTriggerId);
	}

	public void SetSpeed(float speed, float maxSpeed)
	{
		speed = Mathf.InverseLerp(0f, maxSpeed, speed);

		_animator.SetFloat(_speedParamId, speed);
	}
}
