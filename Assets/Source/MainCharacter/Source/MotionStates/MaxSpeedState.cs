using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class MaxSpeedState : MotionStateBase
	{
		public MaxSpeedState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			if (_terminated)
				return;

			_terminated = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override async void Process()
		{
			while (!_terminated)
			{
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
		}

		public override void Push()
		{
			//
		}
	}
}
