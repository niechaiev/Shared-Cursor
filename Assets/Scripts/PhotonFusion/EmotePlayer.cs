using System.Collections;
using Fusion;
using Radial;
using UnityEngine;
using Zenject;
using NetworkBehaviour = Fusion.NetworkBehaviour;

namespace PhotonFusion
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

        [Networked, OnChangedRender(nameof(OnEmoteChanged))]
        private int CurrentEmoteIndex { get; set; } = -1;

        [Inject]
        public void Construct(RadialMenu radialMenu)
        {
            Debug.Log("[Inject]");
            _radialMenu = radialMenu;

            _overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = _overrideController;
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

        public override void Spawned()
        {
            if (!HasStateAuthority) return;
            _radialMenu.emotePlayer = this;
        }

        public void SetEmote(int index)
        {
            if (HasStateAuthority) SetEmoteServerRpc(index);
        }

        [Rpc]
        private void SetEmoteServerRpc(int index)
        {
            CurrentEmoteIndex = index;
        }

        private void OnEmoteChanged()
        {
            if (CurrentEmoteIndex == -1)
            {
                secondHand.SetActive(false);
                return;
            }
            _overrideController[DefaultClipName] = _radialMenu.Emotes[CurrentEmoteIndex].clip;
            secondHand.SetActive(_radialMenu.Emotes[CurrentEmoteIndex].isTwoHanded);
        }
    }
}
