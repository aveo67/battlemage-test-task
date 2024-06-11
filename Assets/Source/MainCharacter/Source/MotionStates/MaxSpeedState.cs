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
			_stopped = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override async void Process()
		{
			while (!_stopped)
			{
				_context.Move();

				await Awaitable.NextFrameAsync(_context.destroyCancellationToken);
			}
		}

		public override void Push()
		{

		}

		public override void Die()
		{
			_stopped = true;

			base.Die();
		}
	}
}
