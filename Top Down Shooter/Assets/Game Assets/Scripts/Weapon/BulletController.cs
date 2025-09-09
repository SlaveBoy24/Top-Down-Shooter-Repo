using UnityEngine;
using Photon.Pun;
using System.Collections;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private float _timeToDestroy;
    public GameObject BloodSplatter;

    private void Start()
    {
        StartCoroutine(DestoyIfNotHit());
    }

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
                StartCoroutine(Hit(other, true));
            }
            else if (other.tag == "Enemy" && !other.isTrigger)
            {
                StartCoroutine(Hit(other, false));
            }
        }
    }

    private IEnumerator Hit(Collider hitCollider, bool isEnemyHitBox)
    {
        yield return StartCoroutine(CreateBloodSplatter());

        if (isEnemyHitBox)
        {
            hitCollider.transform.parent.GetComponent<Enemy>().GetDamage(_damage);
            PhotonNetwork.Destroy(_photonView);
        }
        else
        {
            hitCollider.gameObject.GetComponent<Enemy>().GetDamage(_damage);
            PhotonNetwork.Destroy(this.gameObject);
        }

        yield break;
    }

    private IEnumerator DestoyIfNotHit()
    {
        yield return new WaitForSeconds(_timeToDestroy);

        PhotonNetwork.Destroy(this.gameObject);

        yield break;
    }

    private IEnumerator CreateBloodSplatter()
    {
        Vector3 position = this.gameObject.transform.position;

        Quaternion halfTurn = Quaternion.Euler(0f, 180f, 0f);

        Quaternion newRotation = this.gameObject.transform.rotation * halfTurn;

        GameObject bloodSplatter = PhotonNetwork.Instantiate(BloodSplatter.name, position, newRotation);

        yield break;
    }
}
