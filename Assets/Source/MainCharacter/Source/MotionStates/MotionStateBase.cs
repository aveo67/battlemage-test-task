using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal abstract class MotionStateBase
	{
		protected readonly LichHandler _context;

		public MotionStateBase(LichHandler context)
		{
			_context = context;
		}

		public void Block(Awaitable awaitHandle)
		{
			_context.SetNextState(new BlockedState(_context, awaitHandle));
		}

		public abstract void Brake();

		public abstract void Process();

		public abstract void Push();

		public void Die()
		{
			_context.SetNextState(new DeathState(_context));
		}
	}
}
