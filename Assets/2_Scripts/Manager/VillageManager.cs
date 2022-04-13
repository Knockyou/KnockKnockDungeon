using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class VillageManager : MonoBehaviour
{
    //임시로
    [SerializeField] GameObject _playerObj;
    [SerializeField] Canvas _optionCavas;

    //Npc
    /*[SerializeField] GameObject _MrKim;
    [SerializeField] Image _MrKimStatusMark;
    [SerializeField] GameObject _Zombie;
    [SerializeField] Image _ZombieStatusMark;
    [SerializeField] GameObject _Rabbit;
    [SerializeField] Image _RabbitStatusMark;*/

    //퀘스트 관련
    List<List<string>> _questlt;
    [SerializeField] GameObject _Questmanual;
    List<List<string>> _QuestAnounce;

    //optionpanel
    GameObject _optionPanel;


    //void Awake()
    void Start()
    {
        //퀘스트 파일 읽기
        QuestSenario();
        //초기 로그인시에는 접오가 없으니 새로고침용
        //UserInfoClass._instance.QuestManager(0, 2);

        //현재 진행중 퀘스트 인잇
        //QuestNpcInit(UserInfoClass._instance.CurrentQuest);



        //나중에 userinfoclass 에서 current캐릭터 인덱스 불러와 로드 할것임
        Instantiate(_playerObj, transform.position, Quaternion.identity);
        _optionPanel = _optionCavas.transform.Find("OptionPanel").gameObject;
        if(UserInfoClass._instance._isLevelUp)
        {
            Invoke("LevelUp", 0.2f);
        }

        
        
    }

    void Update()
    {
        if(UserInfoClass._instance.CurrentQuest == "1" && UserInfoClass._instance._isQuestIng)
        {
            if(UserInfoClass._instance.Level == 2)
            {
                UserInfoClass._instance.QuestManager(0, 0);
                QuestSenario();
            }
        }




    }

    void LevelUp()
    {
        if (UserInfoClass._instance.CurrentQuest == "1" && UserInfoClass._instance._isQuestIng)
        {
            UserInfoClass._instance.QuestManager(0, 1);
            
            //NPC 대화와 마크 인잇
            QuestSenario();
        }


        GameObject go = Instantiate(Resources.Load<GameObject>("HitEffect/LevelUp"), transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        go.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        UserInfoClass._instance._isLevelUp = false;
        UserInfoClass._instance.Level += 1;
    }

    public void OptionPopup()
    {
        _optionPanel.SetActive(true);
    }

    public void OptionPopupClose()
    {
        _optionPanel.SetActive(false);
    }


    public void ExitGame()
    {
        //게임종료
        ProgramOff.Poweroff();
    }

    public void QuestSenario()
    {
        string path = "Assets/Resources/Scenario/QuestInfo.txt";

        _questlt = new List<List<string>>();

        if (File.Exists(path))
        {
            StreamReader fs = new StreamReader(path);
            while (fs.EndOfStream == false)
            {
                List<string> templt = new List<string>();
                string[] tempStr;
                string str = fs.ReadLine();
                tempStr = str.Split('\t');
                for (int i = 0; i < tempStr.Length; i++)
                {
                    templt.Add(tempStr[i]);
                }
                _questlt.Add(templt);

            }
            /*for(int i = 0; i < _questlt.Count; i++)
            {
                for(int j = 0; j < _questlt[i].Count; j++)
                {
                    Debug.Log(i + " : " + _questlt[i][j]);
                }
            }*/

        }

        QuestNpcInit(UserInfoClass._instance.CurrentQuest);

    }

    public void QuestNpcInit(string idx)
    {

        Sprite MarkNotice = Resources.Load<Sprite>("Image/NoticeMark");
        Sprite MarkComp = Resources.Load<Sprite>("Image/CompleteMark");
        Sprite MarkThink = Resources.Load<Sprite>("Image/ThinkMark");

        /*_NpcAnounce = new List<List<string>>();
        _PlayerAnounce = new List<List<string>>();*/

        _QuestAnounce = new List<List<string>>();

        GameObject _MrKim;
        Image _MrKimStatusMark;
        _MrKim = GameObject.Find("NPC").transform.GetChild(0).gameObject;
        _MrKimStatusMark = _MrKim.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Image>();

        GameObject _Zombie;
        Image _ZombieStatusMark;
        _Zombie = GameObject.Find("NPC").transform.GetChild(1).gameObject;
        _ZombieStatusMark = _Zombie.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Image>();
        GameObject _Rabbit;
        Image _RabbitStatusMark;
        _Rabbit = GameObject.Find("NPC").transform.GetChild(2).gameObject;
        _RabbitStatusMark = _Rabbit.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Image>();

        _MrKimStatusMark.sprite = null;
        _ZombieStatusMark.sprite = null;
        _RabbitStatusMark.sprite = null;
        _MrKimStatusMark.color = new Color(1, 1, 1, 0);
        _ZombieStatusMark.color = new Color(1, 1, 1, 0);
        _RabbitStatusMark.color = new Color(1, 1, 1, 0);


        switch (idx)
        {
            case "-1"://초심자 교육1
                //퀘스트 대사 인잇
                for(int i = 0; i < _questlt.Count; i++)
                {
                    if(_questlt[i][_questlt[i].Count - 1] == eQuestTitle.초심자교육1.ToString())
                    {
                        List<string> temp = new List<string>();

                        temp.Add(_questlt[i][1].ToString());
                        temp.Add(_questlt[i][4].ToString());
                        temp.Add(_questlt[i][5].ToString());
                        temp.Add(_questlt[i][6].ToString());
                        temp.Add(_questlt[i][7].ToString());

                        _QuestAnounce.Add(temp);

                    }    
                }

                //퀘스트 상태 표시
                if (UserInfoClass._instance._Quest_ing == 0 && !UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkNotice;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);

                }
                else if (UserInfoClass._instance._Quest_ing == 1 && !UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkThink;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if(UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkComp;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);
                }
             

                break;
            case "1"://초심자 교육 2
                //퀘스트 대사 인잇
                for (int i = 0; i < _questlt.Count; i++)
                {
                    if (_questlt[i][_questlt[i].Count - 1] == eQuestTitle.초심자교육2.ToString())
                    {
                        List<string> temp = new List<string>();

                        temp.Add(_questlt[i][1].ToString());
                        temp.Add(_questlt[i][4].ToString());
                        temp.Add(_questlt[i][5].ToString());
                        temp.Add(_questlt[i][6].ToString());
                        temp.Add(_questlt[i][7].ToString());

                        _QuestAnounce.Add(temp);
                    }
                }

                //퀘스트 상태 표시
                if (UserInfoClass._instance._Quest_ing == 0 && !UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkNotice;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._Quest_ing == 1 && !UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkThink;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._isQuestComp)
                {
                    _ZombieStatusMark.sprite = MarkComp;
                    _ZombieStatusMark.color = new Color(1, 1, 1, 1);
                }

                break;
            case "2"://시끄러운 윗동네
                //퀘스트 대사 인잇
                for (int i = 0; i < _questlt.Count; i++)
                {
                    if (_questlt[i][_questlt[i].Count - 1] == eQuestTitle.시끄러운윗동네.ToString())
                    {
                        List<string> temp = new List<string>();

                        temp.Add(_questlt[i][1].ToString());
                        temp.Add(_questlt[i][4].ToString());
                        temp.Add(_questlt[i][5].ToString());
                        temp.Add(_questlt[i][6].ToString());
                        temp.Add(_questlt[i][7].ToString());

                        _QuestAnounce.Add(temp);
                    }
                }

                //퀘스트 상태 표시
                if (UserInfoClass._instance._Quest_ing == 0 && !UserInfoClass._instance._isQuestComp)
                {
                    _MrKimStatusMark.sprite = MarkNotice;
                    _MrKimStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._Quest_ing == 1 && !UserInfoClass._instance._isQuestComp)
                {
                    _MrKimStatusMark.sprite = MarkThink;
                    _MrKimStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._isQuestComp)
                {
                    _MrKimStatusMark.sprite = MarkComp;
                    _MrKimStatusMark.color = new Color(1, 1, 1, 1);
                }

                break;
            case "3"://식재료 마켓
                //퀘스트 대사 인잇
                for (int i = 0; i < _questlt.Count; i++)
                {
                    if (_questlt[i][_questlt[i].Count - 1] == eQuestTitle.식재료마켓.ToString())
                    {
                        List<string> temp = new List<string>();

                        temp.Add(_questlt[i][1].ToString());
                        temp.Add(_questlt[i][4].ToString());
                        temp.Add(_questlt[i][5].ToString());
                        temp.Add(_questlt[i][6].ToString());
                        temp.Add(_questlt[i][7].ToString());

                        _QuestAnounce.Add(temp);
                    }
                }

                //퀘스트 상태 표시
                if (UserInfoClass._instance._Quest_ing == 0 && !UserInfoClass._instance._isQuestComp)
                {
                    _RabbitStatusMark.sprite = MarkNotice;
                    _RabbitStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._Quest_ing == 1 && !UserInfoClass._instance._isQuestComp)
                {
                    _RabbitStatusMark.sprite = MarkThink;
                    _RabbitStatusMark.color = new Color(1, 1, 1, 1);
                }
                else if (UserInfoClass._instance._isQuestComp)
                {
                    _RabbitStatusMark.sprite = MarkComp;
                    _RabbitStatusMark.color = new Color(1, 1, 1, 1);
                }


                break;

            default:
                break;

        }
        UserInfoClass._instance._currentQuestAnounce = _QuestAnounce;

        /*for (int i = 0; i < _NpcAnounce.Count; i++)
            Debug.Log("Npc : " + _NpcAnounce[i].ToString());
        for (int i = 0; i < _PlayerAnounce.Count; i++)
            Debug.Log("Play : " + _PlayerAnounce[i].ToString());*/

    }
}
