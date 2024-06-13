namespace Battlemage.MainCharacter
{
	internal class IdleState : MotionStateBase
	{
		public IdleState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			//
		}

		public override void Process()
		{
			if (_context.IsDead && !_terminated)
				_context.SetNextState(new DeathState(_context));

		}

		public override void Push()
		{
			if (!_context.IsDead && !_terminated)
				_context.SetNextState(new AccelerationState(_context));
		}
	}
}
