using System.Collections;
using Radial;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace NGO
{
    public class EmotePlayer : NetworkBehaviour
    {
        private const string DefaultClipName = "SK_ElKemikleri|SK_arıfelayık";
        private static readonly int _idleEmote = Animator.StringToHash("Idle");
        private static readonly int _playEmote = Animator.StringToHash("PlayEmote");
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject secondHand;

        private RadialMenu _radialMenu;
        private AnimatorOverrideController _overrideController;
        private readonly NetworkVariable<int> _currentEmoteIndex = new(-1);

        [Inject]
        public void Construct(RadialMenu radialMenu)
        {
            _radialMenu = radialMenu;

            _overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = _overrideController;
            _currentEmoteIndex.OnValueChanged -= OnEmoteChanged;
            _currentEmoteIndex.OnValueChanged += OnEmoteChanged;
        }

        public IEnumerator PlayEmote(int index)
        {
            animator.SetTrigger(_idleEmote);
            SetEmote(-1);

            yield return null;

            SetEmote(index);
            animator.ResetTrigger(_idleEmote);
            animator.SetTrigger(_playEmote);

            yield return null;

            animator.ResetTrigger(_playEmote);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            //_radialMenu.emotePlayer = this;
        }

        public void SetEmote(int index)
        {
            if (IsOwner) SetEmoteServerRpc(index);
        }

        [ServerRpc]
        private void SetEmoteServerRpc(int index)
        {
            _currentEmoteIndex.Value = index;
        }

        private void OnEmoteChanged(int oldIndex, int newIndex)
        {
            if (newIndex == -1)
            {
                secondHand.SetActive(false);
                return;
            }
            _overrideController[DefaultClipName] = _radialMenu.Emotes[newIndex].clip;
            secondHand.SetActive(_radialMenu.Emotes[newIndex].isTwoHanded);
        }
    }
}
