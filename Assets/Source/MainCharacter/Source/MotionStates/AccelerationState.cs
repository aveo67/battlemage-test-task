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
			if (_stopped)
				return;

			_stopped = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override void Die()
		{
			if (_stopped)
				return;

			_stopped = true;

			base.Die();
		}

		public override async void Process()
		{
			while (!_context.AchivedMaxSpeed)
			{
				if (_stopped)
					return;

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

			if (!_stopped)
				_context.SetNextState(new MaxSpeedState(_context));
		}

		public override void Push()
		{
			if (!_stopped)
				_context.Accelerate();
		}

		internal override void OpenFire()
		{
			if (_stopped)
				return;

			_stopped = true;

			_context.Stop();

			base.OpenFire();

		}
	}
}
