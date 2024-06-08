using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class BlockedState : MotionStateBase
	{
		private readonly Awaitable _awaitHandle;

		public BlockedState(LichHandler context, Awaitable awaitHandle) : base(context)
		{
			_awaitHandle = awaitHandle;
		}

		public override void Brake()
		{
			//
		}

		public override async void Process()
		{
			await _awaitHandle;

			_context.SetNextState(new IdleState(_context));
		}

		public override void Push()
		{
			//
		}
	}
}
