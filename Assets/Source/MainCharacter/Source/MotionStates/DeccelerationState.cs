using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class DeccelerationState : MotionStateBase
	{
		public DeccelerationState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			//
		}

		public override async void Process()
		{
			while (!_context.IsIdle)
			{
				if (_terminated)
					return;

				_context.Deccelerate();

				_context.Move();

				try
				{
					await Awaitable.NextFrameAsync(_context.destroyCancellationToken);
				}

				catch (OperationCanceledException)
				{
					Debug.Log("Main Character Game object was destroyed and moution has been terminated");

					return;
				}
			}

			if (!_terminated)
				_context.SetNextState(new IdleState(_context));
		}

		public override void Push()
		{
			if (_terminated)
				return;

			_terminated = true;

			_context.SetNextState(new AccelerationState(_context));

		}
	}
}
