using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
public class NetworkUtility
{
    static public string specifier = "F";
    static public CultureInfo culture = CultureInfo.CreateSpecificCulture("en-CA");

    static public string ToFloat(float value)
    {
        string f = value.ToString(specifier, culture);
        return f;
    }

    static public int ToInt(bool b)
    {
        int i = 0;

        if(b)
        {
            i = 1;
        }
        
        return i;
	}
    static public int ToInt(string s)
    {
        int m = 0;

        int.TryParse(s, out m);
        return m;
    }

    static public string ToInt(int i)
    {        
        return i.ToString();
    }
    static public bool ToBool(int i)
    {
        bool b = false;

        if(i == 1)
        {
            b = true;
        }
        
        return b;
	}

    static public float ToFloat(string value)
    {
        var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.NumberDecimalSeparator = ".";
        float f = float.Parse(value, ci);
        return f;
    }
    static public V3 ToV3(Vector3 position)
    {
        V3 v3 = new V3();        
        v3.x = (position.x).ToString(specifier, culture);
        v3.y = (position.y).ToString(specifier, culture);
        v3.z = (position.z).ToString(specifier, culture);
        return v3;
	}

    static public V4 ToV4(Quaternion rotation)
    {
        V4 v4 = new V4();
        v4.x = (rotation.x).ToString(specifier, culture);
        v4.y = (rotation.y).ToString(specifier, culture);
        v4.z = (rotation.z).ToString(specifier, culture);
        v4.w = (rotation.w).ToString(specifier, culture);
        return v4;
	}

    static public Vector3 ToVector3(V3 position)
    {   

        var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.NumberDecimalSeparator = ".";

        Vector3 pos = Vector3.zero;
        pos.x = float.Parse(position.x, ci);
        pos.y = float.Parse(position.y, ci);
        pos.z = float.Parse(position.z, ci);
        return pos;
	}

    static public Quaternion ToVector4(V4 rotation)
    {       
        var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.NumberDecimalSeparator = ".";

        Quaternion rot = Quaternion.identity;
        rot.x = float.Parse(rotation.x, ci);
        rot.y = float.Parse(rotation.y, ci);
        rot.z = float.Parse(rotation.z, ci);
        rot.w = float.Parse(rotation.w, ci);
        return rot;
	}


}

[System.Serializable]
public class V3 // Vector3
{
    public string x = "0";
    public string y = "0";  
    public string z = "0";  
}
[System.Serializable]
public class V4 // Quaternion
{
    public string x = "0";
    public string y = "0";  
    public string z = "0";
    public string w = "0";  
    
}

[Serializable]
public class PlayerData
{
    public string id;
    public int player_id;
    public string mail;
    [HideInInspector]public string status;
    public string username;  
}

[Serializable]
public class Friends
{
    public List<Friend> list;
}

[Serializable]
public class Friend
{
    public string id;
    public string user_id;
    public string username;
    public string status;
}

[Serializable]
public class Invite
{
    public string where_username;
    public string to_username;
    public string photon_room;

    public Invite(Friend friend, string photon_room)
    {
        where_username = NetworkClient.Instance.NetworkIdentity.Player.username;
        to_username = friend.username;
        this.photon_room = photon_room;
    }
}

[Serializable]
public class PingFromFriend
{
    public string username;
    public string status;
}

[Serializable]
public class SearchFriend
{
    public string username;
}