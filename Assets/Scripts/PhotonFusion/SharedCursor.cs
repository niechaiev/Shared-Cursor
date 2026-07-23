using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using NetworkBehaviour = Fusion.NetworkBehaviour;

namespace PhotonFusion
{
    public class SharedCursor : NetworkBehaviour
    {
        [SerializeField] private InputActionReference pointAction;

        private Camera _cam;
        private Quaternion _targetRotation;

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                _cam = Camera.main;
                pointAction.action.Enable();
            }
            else
            {
                SetLayerRecursively(transform.GetChild(0), 0);
            }
        }

        private static void SetLayerRecursively(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            foreach (Transform child in t)
            {
                SetLayerRecursively(child, layer);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (HasStateAuthority)
            {
                pointAction.action.Disable();
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority)
            {
                var screenPos = pointAction.action.ReadValue<Vector2>();
                if (screenPos == Vector2.zero || float.IsNaN(screenPos.x)) return;

                var ray = _cam.ScreenPointToRay(screenPos);

                if (Physics.Raycast(ray, out var hit, 10f, 1 << 0))
                {
                    gameObject.transform.position = hit.point;
                    _targetRotation = Quaternion.LookRotation(-hit.normal);
                    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, _targetRotation, Runner.DeltaTime * 5f);
                }
            }
        }
    }
}
