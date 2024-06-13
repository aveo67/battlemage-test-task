namespace Battlemage.MainCharacter
{
	internal class DeathState : MotionStateBase
	{
		public DeathState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			//
		}

		public override void Process()
		{
			_context.Die();
		}

		public override void Push()
		{
			//
		}

		internal override void OpenFire()
		{
			//
		}

		public override void Stun()
		{
			//
		}
	}
}
