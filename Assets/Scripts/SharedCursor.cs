using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharedCursor : NetworkBehaviour
{
    [SerializeField] private InputActionReference _pointAction;

    private Camera _cam;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _cam = Camera.main;
            _pointAction.action.Enable();
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            _pointAction.action.Disable();
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            var screenPos = _pointAction.action.ReadValue<Vector2>();
            if (screenPos == Vector2.zero) return;

            var ray = _cam.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out var hit, 10f, 1 << 0))
                gameObject.transform.position = hit.point;
        }
    }
}
