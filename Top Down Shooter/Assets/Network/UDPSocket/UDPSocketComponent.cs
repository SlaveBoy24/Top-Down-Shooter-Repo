using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UDPSocket
{
	
	public class UDPSocketComponent : MonoBehaviour
	{
		
		public void Awake()
		{
			this.handlers = new Dictionary<string, List<Action<UDPSocketEvent>>>();
			this.eventQueueLock = new object();
			this.eventQueue = new Queue<UDPSocketEvent>();
			this.udpSocketState = UDPSocketComponent.UDPSocketState.DISCONNECTED;
		}

		
		public void connect(string _serverURL, int _serverPort)
		{
			if (this.tListenner != null && this.tListenner.IsAlive)
			{
				this.disconnect();
				while (this.tListenner != null && this.tListenner.IsAlive)
				{
				}
			}
			this.serverURL = _serverURL;
			this.serverPort = _serverPort;
			this.tListenner = new Thread(new ThreadStart(this.OnListeningServer));
			this.tListenner.IsBackground = true;
			this.tListenner.Start();
		}

		
		public void OnListeningServer()
		{
			try
			{
				object obj = this.udpClientLock;
				lock (obj)
				{
					this.udpClient = new UdpClient();
					this.udpClient.ExclusiveAddressUse = false;
					this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					IPEndPoint local_end = new IPEndPoint(IPAddress.Any, 0);
					this.udpClient.Client.Bind(local_end);
					this.udpSocketState = UDPSocketComponent.UDPSocketState.CONNECTED;
					this.udpClient.BeginReceive(new AsyncCallback(this.OnWaitPacketsCallback), null);
				}
			}
			catch
			{
				throw;
			}
		}

	
		public void OnWaitPacketsCallback(IAsyncResult res)
		{
			object obj = this.udpClientLock;
			lock (obj)
			{
				byte[] array = this.udpClient.EndReceive(res, ref this.endPoint);
				this.MessageReceived(array, this.endPoint.Address.ToString(), this.endPoint.Port);
				if (array != null && array.Length > 0)
				{
					object obj2 = this.eventQueueLock;
					lock (obj2)
					{
						this.receivedMsg = Encoding.UTF8.GetString(array);
						string[] array2 = this.receivedMsg.Split(UDPSocketComponent.Delimiter);
						this.eventQueue.Enqueue(new UDPSocketEvent(array2[0], this.receivedMsg));
						this.receivedMsg = string.Empty;
					}
				}
				this.udpClient.BeginReceive(new AsyncCallback(this.OnWaitPacketsCallback), null);
			}
		}

		private void InvokEvent(UDPSocketEvent ev)
		{
			if (!this.handlers.ContainsKey(ev.name))
			{
				return;
			}
			foreach (Action<UDPSocketEvent> action in this.handlers[ev.name])
			{
				try
				{
					action(ev);
				}
				catch (Exception ex)
				{
				}
			}
		}

	
		public void MessageReceived(byte[] data, string ipHost, int portHost)
		{
		}

	
		public void On(string ev, Action<UDPSocketEvent> callback)
		{
			if (!this.handlers.ContainsKey(ev))
			{				
				this.handlers[ev] = new List<Action<UDPSocketEvent>>();
			}
			this.handlers[ev].Add(callback);
			
		}

	
		public void Emit(string callbackID, string _pack)
		{
			try
			{
				if (this.udpSocketState == UDPSocketComponent.UDPSocketState.CONNECTED)
				{
					object obj = this.udpClientLock;
					lock (obj)
					{
						if (this.udpClient == null)
						{
							this.udpClient = new UdpClient();
							this.udpClient.ExclusiveAddressUse = false;
							this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
							IPEndPoint local_end = new IPEndPoint(IPAddress.Any, 0);
							this.udpClient.Client.Bind(local_end);
						}
						this.udpSocketState = UDPSocketComponent.UDPSocketState.SENDING_MESSAGE;
						string text = callbackID + "|" + _pack;
						byte[] bytes = Encoding.UTF8.GetBytes(text.ToString());
						IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse(this.serverURL), this.serverPort);
						this.udpClient.Send(bytes, bytes.Length, ipendPoint);
						this.udpSocketState = UDPSocketComponent.UDPSocketState.CONNECTED;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
			}
		}

	
		private void OnDestroy()
		{
			object obj = this.udpClientLock;
			lock (obj)
			{
				if (this.udpClient != null)
				{
					this.udpClient.Close();
				}
			}

		}
	
		public void Update()
		{
			object obj = this.eventQueueLock;
			lock (obj)
			{
				while (this.eventQueue.Count > 0)
				{					
					this.InvokEvent(this.eventQueue.Dequeue());
				}
			}
		}


		private void OnApplicationQuit()
		{
			object obj = this.udpClientLock;
			lock (obj)
			{
				if (this.udpClient != null)
				{
					this.udpClient.Close();
				}
			}
		}


		public void disconnect()
		{
			object obj = this.udpClientLock;
			lock (obj)
			{
				if (this.udpClient != null)
				{
					this.udpClient.Close();
				}
			}
		}


		private string serverURL;


		private int serverPort;

		
		public UDPSocketComponent.UDPSocketState udpSocketState;

	
		private UdpClient udpClient;

	
		private readonly object udpClientLock = new object();

	
		private static readonly char[] Delimiter = new char[]
		{
			'|'
		};

	
		private string receivedMsg = string.Empty;

	
		private Dictionary<string, List<Action<UDPSocketEvent>>> handlers;

		
		private Queue<UDPSocketEvent> eventQueue;

	
		private object eventQueueLock;


		private IPEndPoint endPoint;

	
		private string listenerInput = string.Empty;

	
		private Thread tListenner;

	
		public enum UDPSocketState
		{
		
			DISCONNECTED,
		
			CONNECTED,
	
			ERROR,
			
			SENDING_MESSAGE
		}
	}
}
