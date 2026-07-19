using Unity.Netcode;
using Zenject;

namespace Network.Zenject
{
    public class NetworkPrefabInstaller : IInitializable
    {
        private readonly DiContainer _container;
        private readonly NetworkManager _networkManager;
        private readonly NetworkPrefabsList[] _prefabLists;

        public NetworkPrefabInstaller(DiContainer container, NetworkManager networkManager, NetworkPrefabsList[] prefabLists)
        {
            _container = container;
            _networkManager = networkManager;
            _prefabLists = prefabLists;
        }

        public void Initialize()
        {
            foreach (var list in _prefabLists)
            {
                foreach (var prefab in list.PrefabList)
                {
                    var handler = new ZenjectNetworkPrefabHandler(_container, prefab.Prefab);
                    _networkManager.PrefabHandler.AddHandler(prefab.Prefab, handler);
                }
            }
        }
    }
}
