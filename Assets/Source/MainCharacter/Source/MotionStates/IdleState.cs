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
			//
		}

		public override void Push()
		{
			_context.SetNextState(new AccelerationState(_context));
		}
	}
}
