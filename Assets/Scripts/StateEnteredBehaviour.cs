using UnityEngine;

public class StateEnteredBehaviour : StateMachineBehaviour
{
    private EmotePlayer _emotePlayer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(stateInfo + "StateEnteredBehaviour OnStateEnter");
        _emotePlayer ??= animator.GetComponent<EmotePlayer>();
        _emotePlayer.OnIdle();
    }
}
