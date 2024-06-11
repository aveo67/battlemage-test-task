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
	}
}
