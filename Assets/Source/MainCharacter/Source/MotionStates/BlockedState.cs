using System.Threading.Tasks;

namespace Battlemage.MainCharacter
{
	internal class BlockedState : MotionStateBase
	{
		private readonly Task _awaitHandle;

		public BlockedState(LichHandler context, Task awaitHandle) : base(context)
		{
			_awaitHandle = awaitHandle;
		}

		public override void Block(Task awaitHandle)
		{
			//
		}

		public override void Brake()
		{
			//
		}

		public override async void Process()
		{
			_context.Stop();

			await _awaitHandle;

			_context.SetNextState(new IdleState(_context));
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

		public override void Die()
		{
			//
		}
	}
}
