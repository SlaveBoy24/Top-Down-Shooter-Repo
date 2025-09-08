using UnityEngine;

public class BulletController : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
        {
            Debug.Log($"{other.name}");
            Destroy(this.gameObject);
        }
    }
}
