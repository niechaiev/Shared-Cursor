using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using NetworkBehaviour = Fusion.NetworkBehaviour;

namespace PhotonFusion
{
    public class ClientPlayerMove : NetworkBehaviour
    {
        [SerializeField] private InputActionReference rightClickAction;
        [SerializeField] private InputActionReference lookAction;

        [SerializeField] private GameObject phantomParent;
        [SerializeField] private MeshRenderer[] phantomMeshRenderers;
        [SerializeField] private Collider phantomPlaneCollider;

        private CinemachineCamera _cinemachineCamera;
        private CinemachineInputAxisController _cinemachineInputAxisController;

        private void LateUpdate()
        {
            if (HasStateAuthority)
            {
                phantomParent.transform.position = _cinemachineCamera.transform.position;
                phantomParent.transform.rotation = _cinemachineCamera.transform.rotation;
            }
        }

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                _cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
                _cinemachineCamera.Target.TrackingTarget = transform;
                _cinemachineCamera.Follow = transform;


                _cinemachineInputAxisController = _cinemachineCamera.GetComponent<CinemachineInputAxisController>();
                rightClickAction.action.started += _ =>
                {
                    _cinemachineInputAxisController.Controllers[0].Input.InputAction = lookAction;
                    _cinemachineInputAxisController.Controllers[1].Input.InputAction = lookAction;
                };
                rightClickAction.action.canceled += _ =>
                {
                    _cinemachineInputAxisController.Controllers[0].Input.InputAction = null;
                    _cinemachineInputAxisController.Controllers[1].Input.InputAction = null;
                };
                rightClickAction.action.Enable();

                foreach (var meshRenderer in phantomMeshRenderers)
                {
                    meshRenderer.enabled = false;
                }
            }
            else
            {
                phantomPlaneCollider.enabled = false;
            }
        }
    }
}
