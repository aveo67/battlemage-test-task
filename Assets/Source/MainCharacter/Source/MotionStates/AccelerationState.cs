using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class AccelerationState : MotionStateBase
	{
		private bool _stopped = false;

		public AccelerationState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			_stopped = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override async void Process()
		{
			while (!_context.AchivedMaxSpeed)
			{
				if (_stopped)
					return;

				_context.Move();

				await Awaitable.NextFrameAsync(_context.destroyCancellationToken);
			}

			if (!_stopped)
				_context.SetNextState(new MaxSpeedState(_context));
		}

		public override void Push()
		{
			_context.Accelerate();
		}
	}
}
