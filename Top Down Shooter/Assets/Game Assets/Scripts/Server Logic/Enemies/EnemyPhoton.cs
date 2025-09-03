using UnityEngine;
using Photon.Pun;

public class EnemyPhoton : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetTrigger(string name)
    {
        Debug.Log("settrigger");
        PhotonView.Get(this).RPC("AnimatorTriggerRPC", RpcTarget.All, name);
    }

    [PunRPC]
    public void AnimatorTriggerRPC(string name)
    {
        Debug.Log($"trigger name rpc {name}");
        _animator.SetTrigger(name);
    }
}
