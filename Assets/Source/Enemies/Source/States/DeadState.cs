using System;
using UnityEngine;

namespace Battlemage.Enemies
{
	internal class DeadState : EnemyState
	{
		public DeadState(Enemy context) : base(context)
		{
		}

		public override async void Process()
		{
			try
			{
				await _context.AnimateDead();
			}

			catch (OperationCanceledException)
			{
				Debug.Log("Enemy death animation was terminated");
			}
		}

		public override void Dead()
		{
			//
		}
	}
}
