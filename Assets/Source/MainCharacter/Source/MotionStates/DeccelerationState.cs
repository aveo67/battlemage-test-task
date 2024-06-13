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

			if (!_stopped)
				_context.SetNextState(new IdleState(_context));
		}

		public override void Push()
		{
			if (_stopped)
				return;

			_stopped = true;

			_context.SetNextState(new AccelerationState(_context));

		}

		public override void Die()
		{
			if (_stopped)
				return;

			_stopped = true;

			base.Die();

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
