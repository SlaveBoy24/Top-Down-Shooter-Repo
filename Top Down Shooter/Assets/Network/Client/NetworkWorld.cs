using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project
{
    public class NetworkWorld : MonoBehaviour
    {
        public static NetworkWorld instance;
        private Dictionary<string, NetworkTransform> players = new Dictionary<string, NetworkTransform>();
        private float _nextRate = 0.25f;
        private float _rateTime;  
        public event Action sendRateTime;

        void Awake()
        {
            NetworkWorld.instance = this;
            SetNextRate(15);
        }

        private void SetNextRate(int rate)
        {
            var time = 60f/rate;
            _nextRate = 1f/time;
        }

        void FixedUpdate()
        {
            _rateTime += _nextRate;
            if(_rateTime >= 1f)
            {
                _rateTime = 0;
                sendRateTime?.Invoke();
            }
        }

        public NetworkTransform GetPlayer(string id)
        {
            if (players.ContainsKey(id))
            {
                return players[id];
            }

            return null;
        }

        public void UpdatePlayer(PlayerData data)
    {
        if (players.ContainsKey(data.id))
            {
               players[data.id].NextTransform(data);  
            }
    }

        public void AddPlayer(string id, NetworkTransform s)
        {
            if (!players.ContainsKey(id))
            {
                players.Add(id, s);
            }
        }

        public void DeletePlayer(string id)
        {
            if (players.ContainsKey(id))
            {
                Destroy(players[id].gameObject);
                players.Remove(id);
            }
        }

        public void ClearWorld()
        {                      

            foreach (var n in players)
            {
                Destroy(players[n.Key].gameObject);
            }

            players.Clear();
        }


    }
}