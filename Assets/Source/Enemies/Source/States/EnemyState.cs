namespace Battlemage.Enemies
{
	internal abstract class EnemyState
	{
		protected readonly Enemy _context;

		protected bool _terminated = false;

		public EnemyState(Enemy context)
		{
			_context = context;
		}

		public abstract void Process();

		public void Stop()
		{
			_terminated = true;

			_context.Stop();
		}

		public void Die()
		{
			_context.Stop();
		}

		public virtual void Reset()
		{
			Stop();

			_context.SetState(new IdleState(_context));
		}

		public virtual void Dead()
		{
			Stop();

			_context.SetState(new DeadState(_context));
		}
	}
}
