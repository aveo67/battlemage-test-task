using Battlemage.MainCharacter;
using EasyInputHandling.ZenjectExtensions;
using Zenject;

namespace Battlemage.Domain
{
	public class DomainInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.UsePrefabPools()
				.BindInputHandlerFactoryFromProfile<MainCharacterInputProfile, LichHandler>();
		}
	}
}
