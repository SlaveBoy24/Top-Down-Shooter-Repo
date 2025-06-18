using System;

namespace UDPSocket
{
	// Token: 0x02000004 RID: 4
	public class UDPSocketEvent
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002622 File Offset: 0x00000822
		public UDPSocketEvent(string name) : this(name, null)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000262C File Offset: 0x0000082C
		public UDPSocketEvent(string name, string data)
		{
			this.name = name;
			this.pack = data.Split(UDPSocketEvent.Delimiter);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000264C File Offset: 0x0000084C
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002654 File Offset: 0x00000854
		public string name { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000265D File Offset: 0x0000085D
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002665 File Offset: 0x00000865
		public string[] pack { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x0000266E File Offset: 0x0000086E
		public override string ToString()
		{
			return string.Format("[UDPEvent: name={0}, data={1}]", this.name, this.pack.ToString());
		}

		// Token: 0x04000015 RID: 21
		private static readonly char[] Delimiter = new char[]
		{
			'|'
		};
	}
}
