using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinInfoManager : MonoBehaviour
{
    [SerializeField] InputField _idInput;
    string _tempId;
    [SerializeField] InputField _pwdInput;
    string _tempPwd;
    [SerializeField] InputField _pwd1Input;
    [SerializeField] InputField _nickNameInput;
    string _tempName;

    
    void Start()
    {

    }

    void Update()
    {
        _tempId = _idInput.text;
        _tempName = _nickNameInput.text;
        _tempPwd = _pwdInput.text;
    }


    public void FinalCheck()
    {

    }

    public void Checkid()
    {
        if(_idInput.text.Length <= 5 || _idInput.text.Length >= 10)
        {// 자리수 안맞음
            string msg = "길이는 6 ~ 10 글자여야 합니다";
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
            _notiPop.InitNoicePopup(msg, 1);
        }
        else
        {//아이디 중복 체크 해야함
            TcpClient._instance._FinalId = _idInput.text;
            TcpClient._instance.JoinIdCheck(_idInput.text);
        }
    }

    public void CheckNName()
    {
        if (_nickNameInput.text.Length <= 2 || _nickNameInput.text.Length >= 9)
        {// 자리수 안맞음
            string msg = "길이는 3 ~ 10 글자여야 합니다";
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
            _notiPop.InitNoicePopup(msg, 1);
        }
        else
        {//닉네임 중복 체크 해야함
            TcpClient._instance._FinalName = _nickNameInput.text;
            TcpClient._instance.JoinNNameCheck(_nickNameInput.text);
        }
    }

    public void JoinProcess()
    {
        if (!TcpClient._instance._isFinalIDChk || TcpClient._instance._FinalId != _tempId)
        {
            string msg = "아이디 체크를 해주세";
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
            _notiPop.InitNoicePopup(msg, 1);
            return;
        }
        
        if (!TcpClient._instance._isFinalNNameChk || TcpClient._instance._FinalName != _tempName)
        {
            string msg = "닉네임 체크를 해주세요";
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
            _notiPop.InitNoicePopup(msg, 1);
            return;
        }

        if (_pwdInput.text.Length <= 5 || _pwdInput.text.Length >= 9)
        {
            string msg = "패스워드의 길이는 6 ~ 10여야 합니다";
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
            _notiPop.InitNoicePopup(msg, 1);
            return;
        }

        if (_tempPwd != _pwd1Input.text)
        {
            string msg = "패스워드가 일치 하지 않습니다";

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
            _notiPop.InitNoicePopup(msg, 1);
            return;
        }


        //하기전에 유효성 체크 해야함
        TcpClient._instance.JoinInfoSend(_idInput.text, _pwdInput.text, _nickNameInput.text);

        string msg1 = "가입이 성공적으로 이루어졌습니다";

        GameObject frame1 = GameObject.FindWithTag("Frame");
        RectTransform FrameRect1;
        GameObject go1 = Instantiate(Resources.Load<GameObject>("UI/NoticeObject"), transform.position, Quaternion.identity);
        go1.transform.SetParent(frame1.transform);
        FrameRect1 = go1.GetComponent<RectTransform>();


        FrameRect1.sizeDelta = frame1.transform.GetComponent<RectTransform>().sizeDelta;
        FrameRect1.anchoredPosition = frame1.transform.GetComponent<RectTransform>().anchoredPosition;
        FrameRect1.anchorMin = frame1.transform.GetComponent<RectTransform>().anchorMin;
        FrameRect1.anchorMax = frame1.transform.GetComponent<RectTransform>().anchorMax;



        NoicePopup _notiPop1;
        _notiPop1 = go1.GetComponent<NoicePopup>();
        _notiPop1.InitNoicePopup(msg1, 1);

        CloseJoinPopup();
    }

    public void CloseJoinPopup()
    {
        Destroy(this.gameObject);
    }
}
