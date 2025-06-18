using UnityEngine;
using Project.Utility.Attributes;
using SocketIO;
using UDPSocket;

namespace Project
{
    public class NetworkIdentity : MonoBehaviour
    {
        [GreyOut, SerializeField] private string _id;
        private SocketIOComponent _tcp;
        private UDPSocketComponent _udp;
        [GreyOut, SerializeField] private bool _isMine;

        public string id => _id;
        public bool isMine => _isMine;

        public void SetId(string i)
        {
            _id = i;
        }

        public void SetMine(bool b)
        {
            _isMine = b;
        }

        public void SetSocketTCP(SocketIOComponent s)
        {
            _tcp = s;
        }

        public void SetSocketUDP(UDPSocketComponent s)
        {
            _udp = s;
        }
        
        public SocketIOComponent GetSocket()
        {
            return _tcp;
        }

        public UDPSocketComponent GetSocketUDP()
        {
            return _udp;
        }
    }
}