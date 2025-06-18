using UnityEngine;

namespace Project
{
    public class NetworkTransform : MonoBehaviour
    {
        private NetworkIdentity _networkIdentity;
        private Vector3 _position;

        void Start()
        {

            _networkIdentity = GetComponent<NetworkIdentity>();

            if (_networkIdentity.isMine)
            {
                NetworkWorld.instance.sendRateTime += SendTransform;
            }

            _position = gameObject.transform.position;            
        }

        void Update()
        {
            if (!_networkIdentity.isMine)
            {
                Transform t = gameObject.transform;
                t.position = Vector3.Lerp(t.position, _position, Time.deltaTime * 15f);
            }else{
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                transform.Translate(Vector3.forward * Time.deltaTime * 5f * verticalInput);
                transform.Translate(-Vector3.right * Time.deltaTime * 5f * horizontalInput);
            }
        }

        void SendTransform()
        {
            if (_networkIdentity.isMine)
            {
                PlayerData data = new PlayerData();
                data.id = _networkIdentity.id;
                data.position = NetworkUtility.ToV3(gameObject.transform.position);
                
                _networkIdentity.GetSocketUDP().Emit("pl", JsonUtility.ToJson(data).ToString());
            }
        }

        public void NextTransform(PlayerData data)
        {
            _position = NetworkUtility.ToVector3(data.position);
        }

        void OnDestroy()
        {
            if(_networkIdentity == null)
            {
                return;
            }

            if (_networkIdentity.isMine)
            {
                NetworkWorld.instance.sendRateTime -= SendTransform;
            }
        }
    }
}
