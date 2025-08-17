using UnityEngine;

public class PlayerCameraRenderer : MonoBehaviour
{
    public GameObject CameraPrefab;
    public void Initialize()
    {
        GameObject Camera = Instantiate(CameraPrefab, this.gameObject.transform);
    }
}
