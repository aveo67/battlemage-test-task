using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class DeccelerationState : MotionStateBase
	{
		private bool _stopped = false;

		public DeccelerationState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{

		}

		public override async void Process()
		{
			while (!_context.IsIdle && !_stopped)
			{
				if (_stopped)
					throw new OperationCanceledException();

				_context.Deccelerate();

				_context.Move();

				await Awaitable.NextFrameAsync(_context.destroyCancellationToken);
			}

			if (!_stopped)
				_context.SetNextState(new IdleState(_context));
		}

		public override void Push()
		{
			_stopped = true;

			_context.SetNextState(new AccelerationState(_context));
		}

		public override void Die()
		{
			_stopped = true;

			base.Die();
		}
	}
}
