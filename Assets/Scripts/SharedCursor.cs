using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharedCursor : NetworkBehaviour
{
    [SerializeField] private InputActionReference _pointAction;

    private Camera _cam;
    private Quaternion _targetRotation;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _cam = Camera.main;
            _pointAction.action.Enable();
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
            {
                gameObject.transform.position = hit.point;
                _targetRotation = Quaternion.LookRotation(-hit.normal);
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, _targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
