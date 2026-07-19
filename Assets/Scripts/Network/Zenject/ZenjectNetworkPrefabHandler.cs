using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Network.Zenject
{
    public class ZenjectNetworkPrefabHandler : INetworkPrefabInstanceHandler
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        public ZenjectNetworkPrefabHandler(DiContainer container, GameObject prefab)
        {
            _container = container;
            _prefab = prefab;
        }

        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            var instance = Object.Instantiate(_prefab, position, rotation);
            _container.InjectGameObject(instance);
            return instance.GetComponent<NetworkObject>();
        }

        public void Destroy(NetworkObject networkObject)
        {
            Object.Destroy(networkObject.gameObject);
        }
    }
}
