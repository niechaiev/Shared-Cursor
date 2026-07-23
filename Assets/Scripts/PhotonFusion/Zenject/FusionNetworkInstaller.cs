using Zenject;

namespace PhotonFusion.Zenject
{
    public class FusionNetworkInstaller : MonoInstaller
    {
        public static DiContainer NetworkContainer { get; private set; }

        public override void InstallBindings()
        {
            NetworkContainer = Container;
        }

        private void OnDestroy()
        {
            if (NetworkContainer == Container)
                NetworkContainer = null;
        }
    }
}
