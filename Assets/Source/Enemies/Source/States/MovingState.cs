using System;
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
				if (!_terminated)
					_context.Move();

				try
				{
					await Awaitable.WaitForSecondsAsync(0.5f, _context.destroyCancellationToken);
				}

				catch (OperationCanceledException) 
				{
					Debug.Log("Enemy running was terminated");

					return;
				}

			} while (!_terminated && !_context.TargetReached);

			if (!_terminated)
				_context.SetState(new AttakState(_context));
		}
	}
}
