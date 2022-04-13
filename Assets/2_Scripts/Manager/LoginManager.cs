using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] InputField _id;
    [SerializeField] InputField _pwd;
    List<List<string>> _questSernario;

    void Start()
    {

    }

    public void Login()
    {
        TcpClient._instance.LoginInfoSend(_id.text, _pwd.text);
    }

    public void Join()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("UI/JoinObject"), transform.position, Quaternion.identity);
        go.transform.parent = transform;
        go.transform.position = new Vector3(-8, 100, 0);
    }

    /*public void LoginFailPopup()
    {
        RectTransform rect = GameObject.Find("BeginFrame").GetComponent<RectTransform>();
        GameObject go = Instantiate(Resources.Load<GameObject>("UI/JoinObject"), transform.position, Quaternion.identity);
        Debug.Log(go.name);
        go.transform.parent = this.transform;
        rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
        NoicePopup _notiPop;
        _notiPop = go.GetComponent<NoicePopup>();
        _notiPop.InitNoicePopup("아이디를 찾을 수없습니다", 1);
    }*/


}
