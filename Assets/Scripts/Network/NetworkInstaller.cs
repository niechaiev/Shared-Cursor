using Network.Zenject;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Network
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private NetworkPrefabsList[] networkPrefabsLists;

        public override void InstallBindings()
        {
            Container.Bind<NetworkManager>().FromInstance(networkManager).AsSingle();
            Container.Bind<NetworkPrefabsList[]>().FromInstance(networkPrefabsLists).AsSingle();
            Container.BindInterfacesTo<NetworkPrefabInstaller>().AsSingle();
        }
    }
}
