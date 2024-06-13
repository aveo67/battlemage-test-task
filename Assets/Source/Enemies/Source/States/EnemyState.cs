using System;
using UnityEngine;

namespace Battlemage.Enemies
{
	internal abstract class EnemyState
	{
		protected readonly Enemy _context;

		public EnemyState(Enemy context)
		{
			_context = context;
		}

		public abstract void Process();

		public void Die()
		{
			_context.Stop();
		}

		public virtual void Reset()
		{
			_context.SetState(new IdleState(_context));
		}

		public virtual async void Dead()
		{
			_context.SetState(new DeadState(_context));

			try
			{
				await _context.AnimateDead();
			}

			catch (OperationCanceledException)
			{
				Debug.Log("Enemy death animation was terminated");
			}			
		}
	}
}
