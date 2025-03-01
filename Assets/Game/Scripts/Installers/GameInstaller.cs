using Game.Scripts.Controllers;
using Game.Scripts.Interfaces;
using Zenject;

namespace Game.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Matching IClusterChecker interface with ClusterChecker class
            Container.Bind<IClusterChecker>().To<ClusterChecker>().AsSingle();
        }
    }
}
