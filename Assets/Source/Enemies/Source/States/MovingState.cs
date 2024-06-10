using UnityEngine;

namespace Battlemage.Enemies
{
	internal class MovingState : EnemyState
	{
		public MovingState(Enemy context) : base(context)
		{
		}

		public override async void Process()
		{
			do
			{
				_context.Move();

				await Awaitable.WaitForSecondsAsync(0.5f, _context.destroyCancellationToken);

			} while (!_context.TargetReached);

			_context.SetState(new AttakState(_context));
		}
	}
}
