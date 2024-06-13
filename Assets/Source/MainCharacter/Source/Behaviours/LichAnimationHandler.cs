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
		_stateCompletionSource.SetResult(null);
		_stateCompletionSource = null;
	}

	private async Task PlayAnimationState(int id)
	{
        if (_stateCompletionSource != null)
			await _stateCompletionSource.Task;

		_stateCompletionSource = new TaskCompletionSource<object>();

		_animator.SetTrigger(id);

		await _stateCompletionSource.Task;
	}

	public async Task PlayShortAttack()
	{
		//Debug.Log("PlayShortAttack");

		await PlayAnimationState(_shortAttackTriggerId);

		//Debug.Log("Stop PlayShortAttack");
	}

	public async Task PlayLongAttack()
	{
		//Debug.Log("PlayLongAttack");

		await PlayAnimationState(_longAttackTriggerId);

		//Debug.Log("Stop PlayLongAttack");
	}

	public async Task PlayGettingHit()
	{
		//Debug.Log("PlayGettingHit");

		await PlayAnimationState(_hitTriggerId);

		//Debug.Log("Stop PlayGettingHit");
	}

	public async Task PlayWinning()
	{
		//Debug.Log("PlayWinning");

		await PlayAnimationState(_winTriggerId);

		//Debug.Log("Stop PlayWinning");
	}

	public async Task PlayDieing()
	{
		//Debug.Log("PlayDieing");

		await PlayAnimationState(_dieTriggerId);

		//Debug.Log("Stop PlayDieing");
	}

	public void SetSpeed(float speed, float maxSpeed)
	{
		speed = Mathf.InverseLerp(0f, maxSpeed, speed);

		_animator.SetFloat(_speedParamId, speed);
	}
}
