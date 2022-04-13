using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TcpClient : MonoBehaviour
{
    const string _ip = "127.0.0.1";
    const short _port = 80;
    Socket _server;
    static TcpClient _uniqueInstance;

    public bool _isLoginTry = false;
    public bool _isLogin = false;

    public bool _isJoinIDChk = false;
    public bool _isJoinNickNameChk = false;


    //최종 으로 가입 버튼을 누를때까지 검사하여
    //아이디 치크버튼을 누르면 계속 true로 초기화 됨
    //false일때 가입 시켜여함
    public bool _isFinalIDChk = true;
    public string _FinalId;
    public bool _isFinalNNameChk = true;
    public string _FinalName;

    public bool _userInit = false;

    public bool _isExpRecive = false;

    //
   

    public static TcpClient _instance
    {
        get { return _uniqueInstance; }
    }

    public Socket server
    {
        get { return _server; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
        Connect();
    }

    void Update()
    {
        if (_isLoginTry && !_isLogin)//로그인 요청시에만 실행 해야 함
        {
            if (_server.Connected && _server.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[8];
                int recvLength = _server.Receive(buffer);
                object obj = ClientPacket.RecivePacketInit(buffer, typeof(UserInfocheck), recvLength);
                UserInfocheck LoginPacket = (UserInfocheck)obj;

                if (LoginPacket._chkType != 0)
                    return;

                //커넥트 후 리시브로 uuid받아오는지 확인
                _isLogin = LoginPacket._isUserInfo;
                Debug.Log("Login -> " + _isLogin);

                LoginNoicePopupCreate();
            }    
        }

        if (_isLogin && !_userInit)//로그인 시에 이제 정보 불러오기
        {
            if (_server.Connected && _server.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[936];
                int recvLength = _server.Receive(buffer);

                object obj = ClientPacket.RecivePacketInit(buffer, typeof(LoginInfoPacket), recvLength);
                LoginInfoPacket LoginInfoPacket = (LoginInfoPacket)obj;

                UserInfoClass._instance.UserID = LoginInfoPacket._ID.ToString();
                UserInfoClass._instance.NickName = LoginInfoPacket._NICKNAME.ToString();
                UserInfoClass._instance.CharacterIdx = LoginInfoPacket._AVTIDX;
                //AbilityClass._instance.InitPlayer(LoginInfoPacket);
                UserInfoClass._instance.InitPlayer(LoginInfoPacket);

                if (recvLength > 0)
                    _userInit = true;

            }
        }



        if (_isJoinIDChk || _isJoinNickNameChk)//id check
        {
            if (_server.Connected && _server.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[8];
                int recvLength = _server.Receive(buffer);
                object obj = ClientPacket.RecivePacketInit(buffer, typeof(UserInfocheck), recvLength);
                UserInfocheck Packet = (UserInfocheck)obj;

                if (Packet._chkType == 1)
                {
                 //   Debug.Log("ID Check -> " + Packet._isUserInfo);

                    _isFinalIDChk = Packet._isUserInfo;

                    JoinCheckPopupCreate(0, Packet._isUserInfo);


                    _isJoinIDChk = false;
                }
                else if (Packet._chkType == 2)
                {
                   // Debug.Log("NickName Check -> " + Packet._isUserInfo);

                    _isFinalNNameChk = Packet._isUserInfo;

                    JoinCheckPopupCreate(1, Packet._isUserInfo);


                    _isJoinNickNameChk = false;
                }
                else
                    return;
            }
        }
        if(_isExpRecive)
        {
            if (_server.Connected && _server.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[24];
                int recvLength = _server.Receive(buffer);
                //레벨 관련
                object obj = ClientPacket.RecivePacketInit(buffer, typeof(ReciveExpPacket), recvLength);
                ReciveExpPacket expPacket = (ReciveExpPacket)obj;
                UserInfoClass._instance.LevelUpFun(expPacket._CurrentExp, expPacket._Level);
                _isExpRecive = false;
            }

        }


    }


    void  Connect()
    {
        try
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Connect(_ip, _port); // -> 커넥트 

            while (true)
            {
                //커넥트시 uuid 받아옴
                if (_server.Connected && _server.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buffer = new byte[8];
                    int recvLength = _server.Receive(buffer);
                    object obj = ClientPacket.RecivePacketInit(buffer, typeof(ConnectPacket), recvLength);
                    ConnectPacket ConnectPacket = (ConnectPacket)obj;

                    //커넥트 후 리시브로 uuid받아오는지 확인
                    UserInfoClass._instance.Uuid = ConnectPacket._UUID;
                    Debug.Log(UserInfoClass._instance.Uuid.ToString());
                    //_server.Accept();
                    if (recvLength > 0)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    public void JoinInfoSend(string id, string pwd, string nickName)
    {
        ClientPacket cliPack = new ClientPacket();
        cliPack.JoinRequest(id, pwd, nickName, _server);
    }

    public void LoginInfoSend(string id, string pwd)
    {
        ClientPacket cliPack = new ClientPacket();
        cliPack.LoginRequest(id, pwd, _server);
    }

    public void JoinIdCheck(string id)
    {
        ClientPacket cliPack = new ClientPacket();
        cliPack.JoinCheckRequest(id, _server, 0);
    }

    public void JoinNNameCheck(string name)
    {
        ClientPacket cliPack = new ClientPacket();
        cliPack.JoinCheckRequest(name, _server, 1);
    }

    public void GetGold(int Gold)
    {
        ClientPacket cliPack = new ClientPacket();
        cliPack.GetGold(Gold,_server);
    }

    public void LoginNoicePopupCreate()
    {
        if (_isLogin)
        {//Login Sucess
            LoadingManager.LoadScene("VillageScene");
        }
        else
        {
            GameObject frame = GameObject.FindWithTag("Frame");
            RectTransform FrameRect;
            GameObject go = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
            go.transform.SetParent(frame.transform);
            FrameRect = go.GetComponent<RectTransform>();


            FrameRect.sizeDelta = frame.transform.GetComponent<RectTransform>().sizeDelta;
            FrameRect.anchoredPosition = frame.transform.GetComponent<RectTransform>().anchoredPosition;
            FrameRect.anchorMin = frame.transform.GetComponent<RectTransform>().anchorMin;
            FrameRect.anchorMax = frame.transform.GetComponent<RectTransform>().anchorMax;

            NoicePopup _notiPop;
            _notiPop = go.GetComponent<NoicePopup>();
            _notiPop.InitNoicePopup("아이디를 찾을 수없습니다", 1);
        }

    }

    public void ExpUpdate(int Exp, int CharacIdx)
    {
        ClientPacket ExpPack = new ClientPacket();
        ExpPack.EXPUpdate(Exp, CharacIdx, _server);
    }

    public void QuestData(string data)
    {
        ClientPacket QuDataPack = new ClientPacket();
        QuDataPack.SendQuestdata(data, _server);
    }

    public void ComplteQuestUpdate()
    {
        ClientPacket QuDataPack = new ClientPacket();
        string data = "Complte";
        if (UserInfoClass._instance.CurrentQuest == "-1")
        {
            UserInfoClass._instance._Quest_ing = 0;
            UserInfoClass._instance.CurrentQuest = "1";
            UserInfoClass._instance._QuestCurrentData = "0";
            UserInfoClass._instance._isQuestComp = false;
            data += "M1";
        }
        else if (UserInfoClass._instance.CurrentQuest == "1")
        {
            UserInfoClass._instance._Quest_ing = 0;
            UserInfoClass._instance.CurrentQuest = "2";
            UserInfoClass._instance._QuestCurrentData = "0";
            UserInfoClass._instance._isQuestComp = false;
            data += "1";
        }
        else if (UserInfoClass._instance.CurrentQuest == "2")
        {
            UserInfoClass._instance._Quest_ing = 0;
            UserInfoClass._instance.CurrentQuest = "3";
            UserInfoClass._instance._QuestCurrentData = "0";
            UserInfoClass._instance._isQuestComp = false;
            data += "2";
        }
        else if (UserInfoClass._instance.CurrentQuest == "3")
        {
            UserInfoClass._instance._Quest_ing = 0;
            UserInfoClass._instance.CurrentQuest = "999";
            UserInfoClass._instance._QuestCurrentData = "0";
            UserInfoClass._instance._isQuestComp = false;
            data += "3";
        }

        

        QuDataPack.SendQuestdata(data, _server);
    }

    public void JoinCheckPopupCreate(int type, bool check)
    {
        if (type == 0)
        {
            if (!check)
            {//아이디 없음

                GameObject frame = GameObject.FindWithTag("Frame");
                RectTransform FrameRect;
                GameObject go = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
                go.transform.SetParent(frame.transform);
                FrameRect = go.GetComponent<RectTransform>();


                FrameRect.sizeDelta = frame.transform.GetComponent<RectTransform>().sizeDelta;
                FrameRect.anchoredPosition = frame.transform.GetComponent<RectTransform>().anchoredPosition;
                FrameRect.anchorMin = frame.transform.GetComponent<RectTransform>().anchorMin;
                FrameRect.anchorMax = frame.transform.GetComponent<RectTransform>().anchorMax;


                NoicePopup _notiPop;
                _notiPop = go.GetComponent<NoicePopup>();
                _notiPop.InitNoicePopup("아이디를 사용 하시겠습니까??", 0);
                if (_notiPop._isConfirm)
                    _isFinalIDChk = false;
                else
                    _isFinalIDChk = true;
            }
            else
            {//아이디 있음
                GameObject frame = GameObject.FindWithTag("Frame");
                RectTransform FrameRect;
                GameObject go = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
                go.transform.SetParent(frame.transform);
                FrameRect = go.GetComponent<RectTransform>();


                FrameRect.sizeDelta = frame.transform.GetComponent<RectTransform>().sizeDelta;
                FrameRect.anchoredPosition = frame.transform.GetComponent<RectTransform>().anchoredPosition;
                FrameRect.anchorMin = frame.transform.GetComponent<RectTransform>().anchorMin;
                FrameRect.anchorMax = frame.transform.GetComponent<RectTransform>().anchorMax;


                NoicePopup _notiPop;
                _notiPop = go.GetComponent<NoicePopup>();
                _notiPop.InitNoicePopup("이미 사용중인 아이디 입니다", 1);
            }
            _isJoinIDChk = false;
        }
        else if (type == 1)
        {
            if (!check)
            {//닉네임 없음
                GameObject frame = GameObject.FindWithTag("Frame");
                RectTransform FrameRect;
                GameObject go = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
                go.transform.SetParent(frame.transform);
                FrameRect = go.GetComponent<RectTransform>();


                FrameRect.sizeDelta = frame.transform.GetComponent<RectTransform>().sizeDelta;
                FrameRect.anchoredPosition = frame.transform.GetComponent<RectTransform>().anchoredPosition;
                FrameRect.anchorMin = frame.transform.GetComponent<RectTransform>().anchorMin;
                FrameRect.anchorMax = frame.transform.GetComponent<RectTransform>().anchorMax;


                NoicePopup _notiPop;
                _notiPop = go.GetComponent<NoicePopup>();
                _notiPop.InitNoicePopup("해당 닉네임을 사용 하시겠습니까??", 0);
                if (_notiPop._isConfirm)
                    _isFinalNNameChk = false;
                else
                    _isFinalNNameChk = true;
            }
            else
            {//닉네임 있음
                GameObject frame = GameObject.FindWithTag("Frame");
                RectTransform FrameRect;
                GameObject go = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
                go.transform.SetParent(frame.transform);
                FrameRect = go.GetComponent<RectTransform>();


                FrameRect.sizeDelta = frame.transform.GetComponent<RectTransform>().sizeDelta;
                FrameRect.anchoredPosition = frame.transform.GetComponent<RectTransform>().anchoredPosition;
                FrameRect.anchorMin = frame.transform.GetComponent<RectTransform>().anchorMin;
                FrameRect.anchorMax = frame.transform.GetComponent<RectTransform>().anchorMax;


                NoicePopup _notiPop;
                _notiPop = go.GetComponent<NoicePopup>();
                _notiPop.InitNoicePopup("이미 사용중인 이름 입니다", 1);
            }
            _isJoinNickNameChk = false;
        }
    }

}
