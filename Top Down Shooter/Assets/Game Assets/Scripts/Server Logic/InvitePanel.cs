using UnityEngine;

public class InvitePanel : MonoBehaviour
{
    [SerializeField] private string _roomName;

    public void SetRoomName(string roomName)
    { 
        _roomName = roomName;
    }

    public void Accept()
    {
        FindFirstObjectByType<PhotonRoom>().JoinRoom(_roomName);
        Destroy(gameObject);
    }

    public void Decline()
    {
        // to do back message to requested user
        Destroy(gameObject);
    }
}
