using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;


public struct ConnectPacket // -> 커넥트 됬을시 uuid 받아오는 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
}

public struct UserInfocheck
{//0:login, 1:joinid, 2:joinNName
    [MarshalAs(UnmanagedType.Bool, SizeConst = 4)]
    public bool _isUserInfo;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _chkType;
}

public struct LoginInfoPacket
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _ID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _NICKNAME;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _AVTIDX;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _LEVEL;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _GOLD;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _EXP;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _CURRENT_EXP;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
    public string _COMPLE_QUEST;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
    public string _CURRENT_QUEST;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
    public string _QUESTDATA;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _QUESTING;
}

public struct LoginPacket// -> 로그인용 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _ID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _PWD;
}

public struct JoinPacket// -> 가입용 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 12)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _ID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _PWD;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _NICKNAME;
}


public struct JoinCheckPacket// -> 가입 아이디 확인용 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _TargetString;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _chkType;
}


public struct GoldPacket
{//보낼때와 받을때 둘다 사용
    //Type골드를 얻을시에는 true, 사용 핼때는 false
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
    public string _ID;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _GOLD;
    [MarshalAs(UnmanagedType.Bool, SizeConst = 4)]
    public bool _GTYPE;
}

public struct ExpPacket// -> 경험치 보냄 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] _ID;
    [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
    public short _AVTIDX;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _Exp;

    public ExpPacket(long uuid, string id, short avtidx, int exp)
    {
        _UUID = uuid;
        _ID = new byte[20];

        byte[] _idStr = new UTF8Encoding(true, true).GetBytes(id);
        Array.Copy(_idStr, 0, _ID, 0, _idStr.Length);

        _AVTIDX = avtidx;
        _Exp = exp;
    }
}



public struct ReciveExpPacket// -> 경험치 받음 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
    public short _AVTIDX;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _Level;
    [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
    public int _CurrentExp;
}

public struct QuestProgressPacket// -> 퀘스트 저장 받음 구조체
{
    [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
    public long _UUID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] _ID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] _QUDATE;

    public QuestProgressPacket(long uuid, string id, string data)
    {
        _UUID = uuid;

        _ID = new byte[20];
        _QUDATE = new byte[20];

        byte[] _idStr = new UTF8Encoding(true, true).GetBytes(id);
        Array.Copy(_idStr, 0, _ID, 0, _idStr.Length);
        byte[] _qdStr = new UTF8Encoding(true, true).GetBytes(data);
        Array.Copy(_qdStr, 0, _QUDATE, 0, _qdStr.Length);
    }
}


public class ClientPacket
{
    //컨버트 하는것과 주고 체가 있어야함
    /*[MarshalAs(UnmanagedType.U4)]
    long _packetProtocolId; //패킷 프로토콜 아이디
    [MarshalAs(UnmanagedType.U4)]
    int _uniqueUserIdx;
    [MarshalAs(UnmanagedType.U4)]
    int _packetDataLength; //패킷 데이터 크기
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1004)]
    byte[] _packetData;*/


    /*public ClientPacket()
    { }*/

    public void LoginRequest(string id, string pwd, Socket client)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        { 
            byte[] buffer;
            LoginPacket loginPacket;
            loginPacket._ID = id;
            loginPacket._PWD = pwd;
            loginPacket._UUID = UserInfoClass._instance.Uuid;
            buffer = SendConvertPacketInit(loginPacket);
            client.Send(buffer);
            TcpClient._instance._isLoginTry = true;
        }
    }


    public void JoinRequest(string id, string pwd, string nickName, Socket client)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        {
            byte[] buffer;
            JoinPacket joinPack;
            joinPack._ID = id;
            joinPack._PWD = pwd;
            joinPack._NICKNAME = nickName;
            joinPack._UUID = UserInfoClass._instance.Uuid;
            buffer = SendConvertPacketInit(joinPack);
            client.Send(buffer);
        }
    }

    public void JoinCheckRequest(string str, Socket client, int type)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        {
            byte[] buffer;
            JoinCheckPacket IDChkPack;
            IDChkPack._UUID = UserInfoClass._instance.Uuid;
            IDChkPack._TargetString = str;
            IDChkPack._chkType = type;
            buffer = SendConvertPacketInit(IDChkPack);
            client.Send(buffer);
            if(type == 0)
            {
                TcpClient._instance._isJoinIDChk = true;
                TcpClient._instance._isFinalIDChk = true;
            }

            if (type == 1)
            {
                TcpClient._instance._isJoinNickNameChk = true;
                TcpClient._instance._isFinalNNameChk = true;
            }
        }
    }

    public void GetGold(int Gold, Socket client)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        {
            byte[] buffer;
            GoldPacket GoldPack;
            GoldPack._GOLD = Gold;
            GoldPack._ID = UserInfoClass._instance.UserID;
            GoldPack._UUID = UserInfoClass._instance.Uuid;
            GoldPack._GTYPE = true;
            buffer = SendConvertPacketInit(GoldPack);
            client.Send(buffer);
        }
    }

    public void EXPUpdate(int Exp, int CharacIdx, Socket client)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        {//long uuid, string id, short avtidx, int exp
            byte[] buffer;
            ExpPacket expPack = new ExpPacket(UserInfoClass._instance.Uuid, UserInfoClass._instance.UserID.ToString()
                , (short)CharacIdx, Exp);
            buffer = SendConvertPacketInit(expPack);
            client.Send(buffer);
        }
    }

    public void SendQuestdata(string data, Socket client)
    {
        if (client.Connected && client.Poll(0, SelectMode.SelectWrite))
        {//long uuid, string id, short avtidx, intt exp
            byte[] buffer;
            QuestProgressPacket questpack = new QuestProgressPacket(UserInfoClass._instance.Uuid, UserInfoClass._instance.UserID.ToString()
                , data);
            buffer = SendConvertPacketInit(questpack);
            client.Send(buffer);
        }
    }



    public static object RecivePacketInit(byte[] data, Type type, int size)
    {
        IntPtr buff = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, buff, data.Length);
        object obj = Marshal.PtrToStructure(buff, type);
        Marshal.FreeHGlobal(buff);
        if (Marshal.SizeOf(obj) != data.Length)
        {
            return null;
        }
        return obj;
    }

    static byte[] SendConvertPacketInit(object obj)
    {
        int datasize = Marshal.SizeOf(obj);
        IntPtr buff = Marshal.AllocHGlobal(datasize);
        Marshal.StructureToPtr(obj, buff, false);
        byte[] data = new byte[datasize];
        Marshal.Copy(buff, data, 0, datasize);
        Marshal.FreeHGlobal(buff);
        return data;
    }

   
}

