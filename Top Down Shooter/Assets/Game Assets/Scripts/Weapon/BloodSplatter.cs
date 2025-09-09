using System.Collections;
using Photon.Pun;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyBloodSplatter());
    }

    private IEnumerator DestroyBloodSplatter()
    {
        yield return new WaitForSeconds(1f);

        PhotonNetwork.Destroy(this.gameObject);

        yield break;
    }
}
