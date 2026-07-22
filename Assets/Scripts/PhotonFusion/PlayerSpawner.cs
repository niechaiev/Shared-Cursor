using Fusion;
using UnityEngine;

namespace PhotonFusion
{
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
    {
        public GameObject PlayerPrefab;

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
    }
}
