using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private PhotonView _photonView;

    public void SetDamage(float damage)
    { 
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (PhotonNetwork.LocalPlayer == _photonView.Owner)
        { 
            if (other.tag == "EnemyHitBox")
            {
                Debug.Log($"{other.name}");

                other.transform.parent.GetComponent<Enemy>().GetDamage(_damage);

                PhotonNetwork.Destroy(_photonView);
            }
            else if (other.tag == "Enemy" && !other.isTrigger)
            {
                other.gameObject.GetComponent<Enemy>().GetDamage(_damage);

                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
