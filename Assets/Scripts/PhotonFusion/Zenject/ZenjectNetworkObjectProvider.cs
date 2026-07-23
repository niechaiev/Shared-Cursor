using Fusion;
using Zenject;

namespace PhotonFusion.Zenject
{
    public class ZenjectNetworkObjectProvider : NetworkObjectProviderDefault
    {
        protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
        {
            var instance = Instantiate(prefab);
            FusionNetworkInstaller.NetworkContainer?.InjectGameObject(instance.gameObject);
            return instance;
        }
    }
}
