using System.Threading.Tasks;

namespace Battlemage.MainCharacter
{
	internal abstract class MotionStateBase
	{
		protected readonly LichHandler _context;

		public MotionStateBase(LichHandler context)
		{
			_context = context;
		}

		public void Block(Task awaitHandle)
		{
			_context.SetNextState(new BlockedState(_context, awaitHandle));
		}

		public abstract void Brake();

		public abstract void Process();

		public abstract void Push();

		public virtual void Stun()
		{
			_context.Stun();
		}

		public virtual void Die()
		{
			_context.SetNextState(new DeathState(_context));
		}

		internal virtual async void OpenFire()
		{
			if (!_context.IsDead)
			{
				await _context.CastSpell();
			}

			_context.SetNextState(new IdleState(_context));
		}
	}
}
