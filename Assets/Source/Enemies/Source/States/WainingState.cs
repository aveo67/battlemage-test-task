using UnityEngine;

namespace Battlemage.Enemies
{
	internal class WainingState : EnemyState
	{
		public WainingState(Enemy context) : base(context)
		{
		}

		public override async void Process()
		{
			await Awaitable.WaitForSecondsAsync(3f);

			if (_context.TargetDead)
				_context.SetState(new IdleState(_context));

			else
				_context.SetState(new MovingState(_context));
		}
	}
}
