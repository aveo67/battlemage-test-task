namespace Battlemage.Enemies
{
	internal class AttakState : EnemyState
	{
		public AttakState(Enemy context) : base(context)
		{
		}

		public override void Process()
		{
			_context.Bite();

			_context.SetState(new WaitingState(_context));
		}
	}
}
