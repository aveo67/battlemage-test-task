using System.Threading.Tasks;

namespace Battlemage.MainCharacter
{
	internal abstract class MotionStateBase
	{
		protected readonly LichHandler _context;

		protected bool _terminated;

		public MotionStateBase(LichHandler context)
		{
			_context = context;
		}

		public void Stop()
		{
			_terminated = true;
		}

		public virtual void Block(Task awaitHandle)
		{
			if (_terminated)
				return;

			_terminated = true;

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
			if (_terminated)
				return;

			_terminated = true;

			_context.SetNextState(new DeathState(_context));
		}

		internal virtual async void OpenFire()
		{
			if (_terminated)
				return;

			if (!_context.IsDead)
			{
				_context.Stop();

				await _context.CastSpell();
			}
		}
	}
}
