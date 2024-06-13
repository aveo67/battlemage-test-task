using Battlemage.MainCharacter;
using Battlemage.Enemies;
using EasyInputHandling.ZenjectExtensions;
using Zenject;

namespace Battlemage.Domain
{
	public class DomainInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.UseMainCharacter()
				.UseEnemies()
				.BindInputHandlerFactoryFromProfile<MainCharacterInputProfile, LichHandler>();
		}
	}
}
