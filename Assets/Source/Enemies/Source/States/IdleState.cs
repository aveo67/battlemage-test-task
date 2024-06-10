namespace Battlemage.Enemies
{
	internal class IdleState : EnemyState
	{
		public IdleState(Enemy context) : base(context)
		{
		}

		public override void Process()
		{
			if (!_context.TargetDead)
				_context.SetState(new MovingState(_context));
		}
	}
}
