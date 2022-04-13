using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoicePopup : MonoBehaviour
{
    [SerializeField] Text _noticeMsg;
    [SerializeField] GameObject _closeBtn;
    string _msg = "";
    int _type = 0; // 0일때는 버튼 2개, 1일떄는 버튼 1개
    public bool _isConfirm;


   /* void Awake()
    {
        
    }*/

    public void InitNoicePopup(string msg, int type)//생성자로 메세지와 팝업 타입을 받아온다
    {
        _msg = msg;
        _type = type;

        _noticeMsg.text = _msg;

        if (_type == 1)
            _closeBtn.SetActive(false);

    }

    
    
    public void Closebtn()
    {
        _isConfirm = false;
        Destroy(this.gameObject);
    }

    public void Confirmbtn()
    {
        if(_type == 1)
            Destroy(this.gameObject);
        else
        {
            _isConfirm = true;
            Destroy(this.gameObject);
        }
    }
    
}
