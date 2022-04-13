using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInit : MonoBehaviour
{
    //static MapInit _uniqueInstance;

    public int _monCount = 3;
    public int _monlive = 0;
    public float _totalExp = 0;

    public int _getGoldCount = 0;
    [SerializeField] GameObject[] _pos;

    int[] _Rotate;
    int[] _posArr;
    int[] _rotArr;

    [SerializeField] Text _TimeTxt;
    float _time;
    [SerializeField] GameObject _mapInfoCavas;
    GameObject _resultPanel;

    [SerializeField] Text _resultExp;
    [SerializeField] Text _resultGold;
    [SerializeField] Text _resultAchive;
    [SerializeField] Text _resultTime;

    //캐릭터 영역
    //임시로
    [SerializeField] GameObject _playerObj;
    [SerializeField] Transform _playerPos;

    GameObject[] _MonsterArr;
    /*public static MapInit _instance
    {
        get { return _uniqueInstance; }
    }*/


    void Awake()
    {
        if (UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp
           && UserInfoClass._instance.CurrentQuest == "2")
        {
            UserInfoClass._instance._questVal = 0;
        }

        

        _MonsterArr = new GameObject[_monCount];

        _Rotate = new int[4];
        _Rotate[0] = 0;
        _Rotate[1] = 90;
        _Rotate[2] = 180;
        _Rotate[3] = 270;

        //_posArr = new int[_monCount];
        _posArr = getRandomInt(3, 0, _pos.Length);
        _rotArr = getRandomInt(3, 0, _Rotate.Length);



        for (int i = 0; i < _monCount; i++)
        {
            System.Random rd = new System.Random();

            GameObject go = Instantiate(Resources.Load<GameObject>("Monster/MonChick"), _pos[_posArr[i]].transform.position, Quaternion.Euler(0, -90.0f, 0));
            
            _MonsterArr[i] = go;
            //Canvas hpcavas = go.GetComponentInChildren<Canvas>();
            //Shpcavas.transform.localEulerAngles = new Vector3(hpcavas.transform.localEulerAngles.x, transform.rotation.y, hpcavas.transform.localEulerAngles.z);
        }



        _resultPanel = _mapInfoCavas.transform.Find("ResultPanel").gameObject;



        //나중에 userinfoclass 에서 current캐릭터 인덱스 불러와 로드 할것임
        Instantiate(_playerObj, _playerPos.position, Quaternion.identity);

    }

    void Update()
    {

        if(_monCount == _monlive)
        {
            
            Invoke("ResultPopup", 2.0f);
        }
        else
        {
            _time += Time.deltaTime;
            string str = _time.ToString("00.00");
            str = str.Replace(".",":");
            _TimeTxt.text = str;
        }

        if (UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp 
            && UserInfoClass._instance.CurrentQuest == "2")
        {
            UserInfoClass._instance.QuestManager(_monlive, 1);
        }
    }

    

    void ResultPopup()
    {
        _resultPanel.SetActive(true);
        _resultExp.text = _totalExp.ToString();
        _resultTime.text = _TimeTxt.text;
        _resultGold.text = _getGoldCount.ToString();
        _resultAchive.text = _monlive.ToString();
    }


    public int[] getRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for(int i = 0; i < length; i++)
        {
            while (true)
            {
                System.Random rd = new System.Random();
                randArray[i] = rd.Next(min, max);
                isSame = false;

                for(int j = 0; j < i; ++j)
                {
                    if(randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }


    public void LoadVillageScene()
    {
        TcpClient._instance.ExpUpdate(int.Parse(_resultExp.text), UserInfoClass._instance.CharacterIdx);
        TcpClient._instance._isExpRecive = true;//true를 하면 리시브 받을수 있음

        if (UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp 
            && (UserInfoClass._instance.CurrentQuest == "-1" || UserInfoClass._instance.CurrentQuest == "3"))
        {
            UserInfoClass._instance.QuestManager(1,1);
        }

        UserInfoClass._instance._isBattle = false;
        LoadingManager.LoadScene("VillageScene");
    }
}
