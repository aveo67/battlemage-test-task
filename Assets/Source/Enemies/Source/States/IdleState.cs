namespace Battlemage.Enemies
{
	internal class IdleState : EnemyState
	{
		public IdleState(Enemy context) : base(context)
		{
		}

		public override void Process()
		{
			if (!_context.IsDead && _context.HasTarget && !_context.TargetDead)
				_context.SetState(new MovingState(_context));
		}
	}
}
