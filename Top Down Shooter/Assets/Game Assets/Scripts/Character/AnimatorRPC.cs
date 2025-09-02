using UnityEngine;
using Photon.Pun;

public class AnimatorRPC : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetTrigger(string triggerName)
    {
        if (PhotonNetwork.CurrentRoom != null)
            PhotonView.Get(this).RPC("SetTriggerRPC", RpcTarget.All, triggerName);
        else
            _animator.SetTrigger(triggerName);
    }

    public Animator GetAnimator()
    { 
        return _animator;
    }

    [PunRPC]
    public void SetTriggerRPC(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}
