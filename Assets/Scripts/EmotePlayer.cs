using Radial;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class EmotePlayer : NetworkBehaviour
{
    private static readonly int _playEmote = Animator.StringToHash("PlayEmote");
    [SerializeField] private Animator animator;
    [SerializeField] private EmoteSO emoteSo;

    private RadialMenu _radialMenu;

    [Inject]
    public void Construct(RadialMenu radialMenu)
    {
        Debug.Log("EmotePlayer Construct");
        _radialMenu = radialMenu;
    }

    public void PlayEmote(int emoteIndex)
    {
        animator.SetInteger(_playEmote, emoteIndex);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _radialMenu.emotePlayer = this;
    }
}
