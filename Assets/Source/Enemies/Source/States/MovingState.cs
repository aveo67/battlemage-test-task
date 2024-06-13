using UnityEngine;

namespace Battlemage.Enemies
{
	internal class MovingState : EnemyState
	{
		private bool _terminated = false;

		public MovingState(Enemy context) : base(context)
		{
		}

		public override async void Process()
		{
			do
			{
				if (!_terminated)
					_context.Move();

				await Awaitable.WaitForSecondsAsync(0.5f, _context.destroyCancellationToken);

			} while (!_terminated && !_context.TargetReached);

			if (!_terminated)
				_context.SetState(new AttakState(_context));
		}

		public override void Reset()
		{
			_terminated = true;
			_context.Stop();

			base.Reset();
		}

		public override void Dead()
		{
			_terminated = true;
			_context.Stop();

			base.Dead();
		}
	}
}
