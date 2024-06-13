using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class MaxSpeedState : MotionStateBase
	{
		private bool _stopped = false;

		public MaxSpeedState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			if (_stopped)
				return;

			_stopped = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override async void Process()
		{
			while (!_stopped)
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
			if ( _stopped) 
				return;

			_stopped = true;

			_context.Stop();

			base.OpenFire();
		}
	}
}
