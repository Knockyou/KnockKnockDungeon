using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum eQuestTitle
{
    초심자교육1 = -1,
    초심자교육2 = 1,
    시끄러운윗동네,
    식재료마켓
}
public class UserInfoClass : TSingleton<UserInfoClass>
{
    long _Uuid;
    string _UserId;
    string _NickName;
    int _CurrentCharacterIdx;
    int _Gold;

    //사용자가 보유한 캐릭터 정보
    int _Level; //캐릭터 레벨
    int _Exp; //레벨exp
    int _CurrentExp; //현재 경험치 수치
    string _Complete_quest; //완료한 퀘스트
    string _Current_Quest;//현재 진행중인 퀘스트
    public string _QuestCurrentData;
    public int _Quest_ing;

    //인던전 시 이속 애니메이션 바뀜
    public bool _isBattle = false;
    public bool _isLevelUp = false;

    //퀘스트 
    public bool _isQuestIng = false; //퀘스트 받으면 true
    public bool _isQuestComp = false; //퀘스트 완료 true
    public List<List<string>> _currentQuestAnounce = new List<List<string>>();
    public int _questVal; //퀘스트용 변수 카운트등을 사용해야 할때

    //퀘스트 메뉴얼
    string questManualMinus = "던전 클리어 하기";
    string questManual1 = "레벨 2 달성(레벨은 던전을 클리어하면 렙업을 할수 있다)";
    string questManual2 = "치킨을 6마리 잡아보자";
    string questManual3 = "던전 2번 클리어 하기";

    //어디쓰이는지모름
    bool _isInfoInit = false; // -> 로그인후 초기화 변수 확인용

    public long Uuid
    {
        get { return _Uuid; }
        set { _Uuid = value; }
    }
    public string UserID
    {
        get { return _UserId; }
        set { _UserId = value; }
    }

    public string NickName
    {
        get { return _NickName; }
        set { _NickName = value; }
    }

    public int CharacterIdx
    {
        get { return _CurrentCharacterIdx; }
        set { _CurrentCharacterIdx = value; }
    }

    public int Gold
    {
        get { return _Gold; }
        set { _Gold = value; }
    }

    public int Level
    {
        get { return _Level; }
        set { _Level = value; }
    }

    public int Exp
    {
        get { return _Exp; }
        set { _Exp = value; }
    }

    public int CurrentExp
    {
        get { return _CurrentExp; }
        set { _CurrentExp = value; }
    }

    public string CompleteQuest
    {
        get { return _Complete_quest; }
        set { _Complete_quest = value; }
    }

    public string CurrentQuest
    {
        get { return _Current_Quest; }
        set { _Current_Quest = value; }
    }


    public void InitPlayer(LoginInfoPacket obj)
    {
        _Level = obj._LEVEL;
        _Exp = obj._EXP;
        _CurrentExp = obj._CURRENT_EXP;
        _Gold = obj._GOLD;
        _Complete_quest = obj._COMPLE_QUEST;
        _Current_Quest = obj._CURRENT_QUEST;
        _QuestCurrentData = obj._QUESTDATA;
        _Quest_ing = obj._QUESTING;
        CharacterIdx = obj._AVTIDX;
        QuestManager(0, 0);
    }

    public void LevelUpFun(int currentexp, int updateLevel)
    {
        if (Level < updateLevel)
        {//레벨업
            _isLevelUp = true;
        }
        else//레벨업 안함
        {
            _isLevelUp = false;
            Level = updateLevel;
        }

        CurrentExp = currentexp;
    }

    

    public void QuestManager(int val, int type)
    {
        if(type == 0)
        {//로그인시 데이터 불러오기
            switch(_Current_Quest)
            {
                case "-1":
                    if (_QuestCurrentData == "1")
                        _isQuestComp = true;
                    else if (_Quest_ing == 1)
                        _isQuestIng = true;
                    
                    break;
                case "1":
                    if (int.Parse(_QuestCurrentData) >= 1 || Level >= 2)
                    {
                        _isQuestComp = true;
                        VillageManager villmana = new VillageManager();
                        villmana.QuestSenario();

                    }
                    else if (_Quest_ing == 1)
                        _isQuestIng = true;
                    break;
                case "2":
                    if (_QuestCurrentData == "6")
                        _isQuestComp = true;
                    else if (_Quest_ing == 1)
                        _isQuestIng = true;
                    break;
                case "3":
                    if (_QuestCurrentData == "2")
                        _isQuestComp = true;
                    else if (_Quest_ing == 1)
                        _isQuestIng = true;
                    break;
                default:
                    break;
            }
        }
        else if(type == 1)
        {
            if(_isQuestIng)
            {
                switch (CurrentQuest)
                {
                    case "-1":
                        QuestBeginner1(val);
                        break;
                    case "1":
                        QuestBeginner2();
                        break;
                    case "2":
                        QuestNoiseNeighborhood(val);
                        break;
                    case "3":
                        QuestGrocery(val);
                        break;
                    default:
                        break;
                }
            }

        }
        else if (type == 2)
        {//인게임에서 퀘스트 완료후 npc말건후 진짜 완료했을시 새로고침
            if (_isQuestIng)
            {
                switch (CurrentQuest)
                {
                    case "-1":
                        
                        break;
                    case "1":
                        
                        break;
                    case "2":
                        
                        break;
                    case "3":
                        
                        break;
                    default:
                        break;
                }
            }

        }


    }
    
    public void QuestBeginner1(int val)
    {//1회던전 클리어 : -1
        _questVal = val;
        if (_questVal == 1)
        {
            //send Q Data
            TcpClient._instance.QuestData(_questVal.ToString());
            _QuestCurrentData = _questVal.ToString();
            _isQuestComp = true;
            //_Current_Quest = "1";
            /*VillageManager villManager = new VillageManager();
            villManager.QuestNpcInit(_Current_Quest);*/

        }
    }

    public void QuestBeginner2()
    {//2레벨업 달성 : 1
        //그냥 Level로 send Q Data
        TcpClient._instance.QuestData(Level.ToString());
        _QuestCurrentData = Level.ToString();
        if (Level >= 2)
        {
            _isQuestComp = true;
            //_Current_Quest = "2";
            /*VillageManager villManager = new VillageManager();
            villManager.QuestNpcInit(_Current_Quest);*/
        }
    }

    public void QuestNoiseNeighborhood(int val)
    {//몬스터 6마리 잡기 : 2
        Debug.Log("확인용 : " + _QuestCurrentData);
        //안끊기고 바로 갈때 0 부터 시작

        if(_questVal != val)
        {//val로 send Q Data
            TcpClient._instance.QuestData(val.ToString());
            _QuestCurrentData = val.ToString();
        }
        _questVal = val;
        if (_questVal >= 6)
        {
            _isQuestComp = true;
            //_Current_Quest = "3";
            /*VillageManager villManager = new VillageManager();
            villManager.QuestNpcInit(_Current_Quest);*/
        }
    }

    public void QuestGrocery(int val)
    {//던전 2번 클리어 : 3
        //그냥 _questVal send Q Data
        TcpClient._instance.QuestData(_questVal.ToString());
        _QuestCurrentData = _questVal.ToString();
        _questVal += val;
        if (_questVal == 2)
        {
            _isQuestComp = true;
            //_Current_Quest = "999";
           /* VillageManager villManager = new VillageManager();
            villManager.QuestNpcInit(_Current_Quest);*/
        }
    }

    public string[] questManualInit()
    {
        string[] temp = new string[3];
        switch (CurrentQuest)
        {
            case "-1":
                temp[0] = eQuestTitle.초심자교육1.ToString();
                temp[1] = questManualMinus;
                temp[2] = "1 / "+_QuestCurrentData;
                break;
            case "1":
                temp[0] = eQuestTitle.초심자교육2.ToString();
                temp[1] = questManual1;
                temp[2] = "2 / " + _Level;
                break;
            case "2":
                temp[0] = eQuestTitle.시끄러운윗동네.ToString();
                temp[1] = questManual2;
                temp[2] = "6 / " + _QuestCurrentData;
                break;
            case "3":
                temp[0] = eQuestTitle.식재료마켓.ToString();
                temp[1] = questManual3;
                temp[2] = "2 / " + _QuestCurrentData;
                break;
            case "0":
                temp[0] = "퀘스트 없음".ToString();
                temp[1] = "내용없음".ToString();
                temp[2] = " - ".ToString();
                break;
            default:
                break;
        }

        return temp;
    }


}
