using PhotonFusion;
using UnityEngine;

public class StateEnteredBehaviour : StateMachineBehaviour
{
    private EmotePlayer _emotePlayer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _emotePlayer ??= animator.GetComponent<EmotePlayer>();
        _emotePlayer.SetEmote(-1);
    }
}
