using System.Collections;
using Radial;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class EmotePlayer : NetworkBehaviour
{
    private static readonly int _idleEmote = Animator.StringToHash("Idle");
    private static readonly int _playEmote = Animator.StringToHash("PlayEmote");
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject secondHand;

    private RadialMenu _radialMenu;
    private AnimatorOverrideController _overrideController;

    [Inject]
    public void Construct(RadialMenu radialMenu)
    {
        Debug.Log("EmotePlayer Construct");
        _radialMenu = radialMenu;
        _overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }

    public IEnumerator PlayEmote(AnimationClip animationClip, bool isTwoHanded)
    {
        animator.SetTrigger(_idleEmote);
        OnIdle();

        yield return null;

        animator.runtimeAnimatorController = _overrideController;
        _overrideController["SK_ElKemikleri|SK_arıfelayık"] = animationClip ;

        animator.ResetTrigger(_idleEmote);
        animator.SetTrigger(_playEmote);
        secondHand.SetActive(isTwoHanded);

        yield return null;
        animator.ResetTrigger(_playEmote);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _radialMenu.emotePlayer = this;
    }

    public void OnIdle()
    {
        secondHand.SetActive(false);
    }
}
