using System;
using UnityEngine;

namespace Battlemage.Enemies
{
	internal class WaitingState : EnemyState
	{
		public WaitingState(Enemy context) : base(context)
		{
		}

		public override async void Process()
		{
			try
			{
				await Awaitable.WaitForSecondsAsync(3f, _context.destroyCancellationToken);
			}

			catch (OperationCanceledException)
			{
				Debug.Log("Enemy running was terminated");

				return;
			}

			if (_context.IsDead)
			{
				return;
			}

			//if (_context.TargetDead)
				_context.SetState(new IdleState(_context));

			//else
			//	_context.SetState(new MovingState(_context));
		}
	}
}
