using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum CharAni
{
    IDLE   = 0,
    Walk,
    BATTLE,
    ATTACK,
    HIT,
    DEATH
}

public class PlayerControl : MonoBehaviour
{
    Animator _playAni;
    float _movSpeed;
    Camera _mainCam;
    DefineCharacter CharStat; //캐릭터 능력치 구현 하려고 하는것
    float _portalTime;
    bool _portalChk = false;
    float _Timer;
    float _QskillCoolTime;
    float _WskillCoolTime;
    float _EskillCoolTime;
    bool _questArea = false;
    bool _isDeath = false;

    [SerializeField] Image _qskillCool;
    [SerializeField] Image _wskillCool;
    [SerializeField] Image _eskillCool;
    [SerializeField] Transform _ThrowPos;
    [SerializeField] Text _Gold;
    [SerializeField] Text _LvTxt;
    [SerializeField] Text _NickName;
    [SerializeField] Slider _hp;
    [SerializeField] Slider _mp;


    //float qMp = 2 * 0.1f;
    //float wMp = 4 * 0.1f;
    //float eMp = 6 * 0.1f;
    GameObject narrObj;
    GameObject _mynarrObj;

    GameObject _attackbox;
    GameObject _wSkillObj;
    GameObject _wSkillSphere;
    float _wSkillDuration = 5.0f;
    float _wSkillTime;
    bool _wSkillEx = false;
    //public float _wskillDamge = 0.5f;

    float _eSkillDuration = 3.0f;
    float _eSkillTime;
    bool _eSkillEx = false;

    public AbilityClass _ability;

    public int _questAnounceCnt = 0;

    GameObject _NpcTempOb;
    Text _NPCspeech;
    Text _Myspeech;

    Image _queststatus;
    void Awake()
    {
        //테스트용 코드
        //UserInfoClass._instance._isBattle = true;

        _attackbox = transform.Find("AttackBox").gameObject;
        //_attackbox.SetActive(true);

        _mynarrObj = GameObject.Find("AbilityCanvas").transform.Find("Narration").gameObject;
        _Myspeech = GameObject.Find("AbilityCanvas").transform.Find("Narration").GetComponentInChildren<Text>();
        _queststatus = GameObject.Find("SKillCanvas").transform.Find("QuestBtn").transform.Find("Status").GetComponent<Image>();

        _ability = new AbilityClass(UserInfoClass._instance.CharacterIdx, 0);
        _hp.value = _ability._Hp;
        /*qMp = qMp / _ability._Mp;
        wMp = wMp / _ability._Mp;
        eMp = eMp / _ability._Mp;*/
        _movSpeed = _ability._MovSpeed;
        _Gold.text = UserInfoClass._instance.Gold.ToString();
        _LvTxt.text = UserInfoClass._instance.Level.ToString();
        _playAni = gameObject.GetComponentInChildren<Animator>();
        _mainCam = Camera.main;
        CharStat = new DefineCharacter();
        _mainCam.transform.position = new Vector3(transform.position.x, 5, transform.position.z - 6);
        _mainCam.transform.localEulerAngles = new Vector3(34.0f, 0, 0);

        _wSkillObj = gameObject.transform.Find("WSkillEff").gameObject;
        _wSkillSphere = gameObject.transform.Find("PurpleAuraSphere").gameObject;

        //일단 주석

        _NickName.text = UserInfoClass._instance.NickName.ToString();

        if (UserInfoClass._instance._isBattle)
            _movSpeed = _movSpeed + 0.5f;


        /*Debug.Log(_ability.MpFun("q"));
        Debug.Log(_ability.MpFun("w"));
        Debug.Log(_ability.MpFun("e"));*/
    }

    void Start()
    {
        if (UserInfoClass._instance._isQuestComp)
        {
            _queststatus.color = new Color(0, 1, 0, 1);
            for (int i = 0; i < UserInfoClass._instance._currentQuestAnounce.Count; i++)
            {
                if (UserInfoClass._instance._currentQuestAnounce[i][3] == "2")
                {
                    _questAnounceCnt = i;
                    break;
                }

            }
        }
    }

    void Update()
    {
        _Timer += Time.deltaTime;

        _Gold.text = UserInfoClass._instance.Gold.ToString();

        if(_Timer - _wSkillTime >= _wSkillDuration && _wSkillEx)
        {
            _wSkillObj.SetActive(false);
            _wSkillSphere.SetActive(false);
        }

        if (_Timer - _eSkillTime >= _eSkillDuration && _eSkillEx)
        {
            _eSkillEx = false;
        }

        if (_LvTxt.text != UserInfoClass._instance.Level.ToString())
            _LvTxt.text = UserInfoClass._instance.Level.ToString();

        RaycastHit hit;
        float rayDistance = 0.7f;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), transform.forward * rayDistance, Color.blue, 0.3f);
        
        if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y + 0.6f, transform.position.z), transform.forward, out hit, rayDistance))
        {
            if (hit.transform.tag == "QuestArea")
            {
                GameObject go = hit.transform.gameObject; //-> NPCObject
                _NpcTempOb = go;
                narrObj = go.transform.GetChild(1).GetChild(0).gameObject;

                if ((UserInfoClass._instance._isQuestIng && go.name == "MrZombi")
                    && (UserInfoClass._instance.CurrentQuest == "-1" || UserInfoClass._instance.CurrentQuest == "1"))
                {
                    Image imgmark = go.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
                    imgmark.sprite = Resources.Load<Sprite>("Image/ThinkMark");
                    imgmark.color = new Color(1, 1, 1, 1);
                }

                if ((UserInfoClass._instance._isQuestIng && go.name == "MrKim")
                    && (UserInfoClass._instance.CurrentQuest == "2"))
                {
                    Image imgmark = go.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
                    imgmark.sprite = Resources.Load<Sprite>("Image/ThinkMark");
                    imgmark.color = new Color(1, 1, 1, 1);
                }

                if ((UserInfoClass._instance._isQuestIng && go.name == "Rabbit")
                && (UserInfoClass._instance.CurrentQuest == "3"))
                {
                    Image imgmark = go.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
                    imgmark.sprite = Resources.Load<Sprite>("Image/ThinkMark");
                    imgmark.color = new Color(1, 1, 1, 1);
                }
                narrObj.SetActive(true);
                _NPCspeech = narrObj.GetComponentInChildren<Text>();
                _questArea = true;
            }
        }
        else
        {
            if(_questArea)
                narrObj.SetActive(false);
            _questArea = false;
        }


        if (_Timer - _portalTime >= 2 && _portalChk)
        {
            LoadingManager.LoadScene("Dungeon1");
            UserInfoClass._instance._isBattle = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (UserInfoClass._instance._isBattle)
                ChangeAni(CharAni.ATTACK);

            if (_questArea && !UserInfoClass._instance._isBattle && _questArea)
                QuestPro();
        }

        //if (Input.GetKeyDown(KeyCode.Q) && UserInfoClass._instance._isBattle && _qskillCool.fillAmount == 0 && _mp.value >= qMp)
        if (Input.GetKeyDown(KeyCode.Q) && UserInfoClass._instance._isBattle && _qskillCool.fillAmount == 0 && _mp.value >= _ability.MpFun("q"))
        {
            ChangeAni(CharAni.ATTACK);
            GameObject go = Instantiate(Resources.Load<GameObject>("SkillEffect/HotPotao"), _ThrowPos.position, _ThrowPos.rotation);
            ThrowObj hotPotato = go.GetComponent<ThrowObj>();
            //hotPotato._damage = _ability.AttManager() * 0.2f;
            hotPotato._damage = _ability.AttManager("q");
            _QskillCoolTime = _Timer;
            _qskillCool.fillAmount = 1;
            StartCoroutine(CoolTime(3, _qskillCool));
            _mp.value -= _ability.MpFun("q"); 
        }

        if (Input.GetKeyDown(KeyCode.W) && UserInfoClass._instance._isBattle && _wskillCool.fillAmount == 0 && _mp.value >= _ability.MpFun("w"))
        {
            _WskillCoolTime = _Timer;
            _wskillCool.fillAmount = 1;
            StartCoroutine(CoolTime(5, _wskillCool));
            _mp.value -= _ability.MpFun("w");
            _wSkillObj.SetActive(true);
            _wSkillSphere.SetActive(true);
            _wSkillEx = true;
            _wSkillTime = _Timer;

        }
        else

        if (Input.GetKeyDown(KeyCode.E) && UserInfoClass._instance._isBattle && _eskillCool.fillAmount == 0 && _mp.value >= _ability.MpFun("e"))
        {
            _EskillCoolTime = _Timer;
            _eskillCool.fillAmount = 1;
            StartCoroutine(CoolTime(7, _eskillCool));
            _mp.value -= _ability.MpFun("e");
            _eSkillEx = true;
            _eSkillTime = _Timer;
            

        }

        if (!_playAni.GetCurrentAnimatorStateInfo(0).IsName("f_melee_combat_attack_A") &&
            (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        //캐릭터 이동
        {
                float zm = Input.GetAxis("Vertical");
                float xm = Input.GetAxis("Horizontal");

                /*transform.position += new Vector3(xm, 0, zm) * Time.deltaTime * _movSpeed;
                _mainCam.transform.position += new Vector3(xm, 0, zm) * Time.deltaTime * _movSpeed;*/

                transform.Translate(new Vector3(xm, 0, zm).normalized * Time.deltaTime * _movSpeed, Space.World);
                _mainCam.transform.position = new Vector3(transform.position.x, 5, transform.position.z - 6);

                Quaternion rotation = Quaternion.LookRotation(new Vector3(xm, 0, zm));
                transform.rotation = Quaternion.Euler(0, 360, 0) * rotation;

                ChangeAni(CharAni.Walk);
        }
        else
        {
            ChangeAni(CharAni.IDLE);
        }




        if (UserInfoClass._instance.CurrentQuest == "1" && UserInfoClass._instance._isQuestIng
            && UserInfoClass._instance.Level >= 2)
        {
            UserInfoClass._instance._isQuestComp = true; 
            VillageManager villmana = new VillageManager();
            villmana.QuestSenario();
            _queststatus.color = new Color(0, 1, 0, 1);
        }
    }

    IEnumerator CoolTime(float cool, Image fillamount)
    {
        while(cool > 0f)
        {
            cool -= Time.deltaTime;
            fillamount.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
    }

    void ChangeAni(CharAni status)
    {
        switch (status)
        {
            case CharAni.IDLE:
                _playAni.SetBool("Walk", false);
                if (UserInfoClass._instance._isBattle)
                    _playAni.SetBool("BATTLE", true);
                break;
            case CharAni.Walk:
                _playAni.SetBool("Walk", true);
                break;
            case CharAni.ATTACK:
                if(_eSkillEx)
                {//공속 스킬
                    _playAni.SetFloat("ATTSPEED", 1.5f);
                }
                else
                {//공속 되돌림
                    _playAni.SetFloat("ATTSPEED", 1.0f);
                }
                _playAni.SetTrigger("ATTACK");
                _playAni.SetBool("IDLE", true);
                break;
            case CharAni.HIT:
                _playAni.SetTrigger("Hit");
                break;
            case CharAni.DEATH:
                _playAni.SetTrigger("DEATH");
                Instantiate(Resources.Load<GameObject>("HitEffect/CharacterDeath"), transform.position, Quaternion.identity);
                break;
            default:
                break;

        
        }
    }

    public void Attackbox(int type)
    {
        if (type == 1)
            _attackbox.SetActive(true);
        else
            _attackbox.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.collider.name);

        if(other.collider.tag == "Portal")
        {
            _portalChk = true;
            _portalTime = _Timer;
        }


        if(other.collider.name == "AttackBox")
        {
            GameObject parent = other.transform.root.gameObject;

            string strName = parent.name.Substring(0, 8);
            //Debug.Log(strName);

            if (strName == "MonChick")//(Clone)
            {
                Vector3 hitPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(Resources.Load<GameObject>("HitEffect/Hit"), hitPos, Quaternion.identity);
                ChangeAni(CharAni.HIT);
                //float damage = parent.GetComponent<ChickControl>()._ability.AttManager();
                float damage = parent.GetComponent<ChickControl>()._ability.AttManager("MonChick");
                Hitmanager(damage);
            }
        }
        
    }

    void OnCollisionExit(Collision other)
    {

        if (other.collider.tag == "Portal")
        {
            _portalChk = false;
            _portalTime = 0;
        }
        
    }

    void Hitmanager(float damge)
    {
        //Debug.Log("나는 몇 맞음 : " + damge);
        _hp.value -= damge;
        if(_hp.value == 0)
        {
            _isDeath = true;
            ChangeAni(CharAni.DEATH);
        }
    }

    public void QuestPro()
    {   //커렌트 퀘스트 번호와 npc이름 이 같으면 퀘스트 재사 진행
        if((_NpcTempOb.name == "MrZombi" && UserInfoClass._instance.CurrentQuest == "-1")
            || (_NpcTempOb.name == "MrZombi" && UserInfoClass._instance.CurrentQuest == "1")
            || (_NpcTempOb.name == "MrKim" && UserInfoClass._instance.CurrentQuest == "2")
            || (_NpcTempOb.name == "Rabbit" && UserInfoClass._instance.CurrentQuest == "3"))
        {
            //지금 D키를 누르는 횟수가 대사 마지막과 같을 때 를 완료라고 친다
            if(_questAnounceCnt == UserInfoClass._instance._currentQuestAnounce.Count)
            {
                //퀘스트 완료 이제 들어감

                _mynarrObj.SetActive(false); //플레어의 대화창 비활성화
                UserInfoClass._instance._isQuestIng = false; //디비 아니고 유저클래스 ing -> false로 바꿈

                //좌측 퀘스트 팝업 0?? 으로 초기화
                GameObject go = GameObject.Find("SKillCanvas").transform.Find("QuestBtn").transform.Find("QuestMenual").gameObject;
                Text Title = go.transform.Find("Title").GetComponent<Text>();
                Title.text = "퀘스트 없음";
                Text Content = go.transform.Find("Content").GetComponent<Text>();
                Content.text = "내용없음";
                Text Score = go.transform.Find("Score").GetComponent<Text>();
                Score.text = " - ";

                //D key 횟수 0으로 초기화
                _questAnounceCnt = 0;

                //0으로 초기화후 재귀함수
                //왜냐 하면 바로 다음 퀘스트로 진행 할 수 도 있으니
                QuestPro();

                //퀘스트 대사 다시 읽음
                TcpClient._instance.ComplteQuestUpdate();
                VillageManager villManager = new VillageManager();
                villManager.QuestSenario();

            }


            for (int i = 0; i < UserInfoClass._instance._currentQuestAnounce.Count; i++)
            {
                if (UserInfoClass._instance._currentQuestAnounce[i][1] == "-" && i == _questAnounceCnt)
                {//플레이어 말함
                    _mynarrObj.SetActive(true);
                    if (!UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp)
                    {
                        _Myspeech.text = UserInfoClass._instance._currentQuestAnounce[i][2];
                        _questAnounceCnt++;
                        if (UserInfoClass._instance._currentQuestAnounce[i + 1][3] == "1")
                        {
                            UserInfoClass._instance._isQuestIng = true;
                            TcpClient._instance.QuestData("Questccept");
                            break;
                        }
                    }
                    else if (UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp)
                    {//퀘스트 진행중 1번 말
                        if (UserInfoClass._instance._currentQuestAnounce[i][3] == "1")
                            _Myspeech.text = UserInfoClass._instance._currentQuestAnounce[i][2];
                    }
                    else if (UserInfoClass._instance._isQuestComp)
                    {
                        if (UserInfoClass._instance._currentQuestAnounce[i][3] == "2")
                        {
                            _Myspeech.text = UserInfoClass._instance._currentQuestAnounce[i][2];
                            _questAnounceCnt++;
                        }
                    }
                    break;
                }
                else if (UserInfoClass._instance._currentQuestAnounce[i][2] == "-" && i == _questAnounceCnt)
                {//npc
                    _mynarrObj.SetActive(false);

                    if (!UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp)
                    {
                        _NPCspeech.text = UserInfoClass._instance._currentQuestAnounce[i][1];
                        _questAnounceCnt++;
                        if (UserInfoClass._instance._currentQuestAnounce[i + 1][3] == "1")
                        {
                            UserInfoClass._instance._isQuestIng = true;
                            /*VillageManager mal = new VillageManager();
                            mal.QuestNpcInit(UserInfoClass._instance.CurrentQuest);*/
                            break;
                        }
                    }
                    else if (UserInfoClass._instance._isQuestIng && !UserInfoClass._instance._isQuestComp)
                    {//퀘스트 진행중 1번 말
                        if (UserInfoClass._instance._currentQuestAnounce[i][3] == "1")
                            _NPCspeech.text = UserInfoClass._instance._currentQuestAnounce[i][1];
                    }
                    else if (UserInfoClass._instance._isQuestComp)
                    {
                        if (UserInfoClass._instance._currentQuestAnounce[i][3] == "2")
                        {
                            _NPCspeech.text = UserInfoClass._instance._currentQuestAnounce[i][1];
                            _questAnounceCnt++;
                        }
                    }
                    break;
                }
            }
        }
    }

    public void QuestBtn()
    {

        GameObject go = GameObject.Find("SKillCanvas").transform.Find("QuestBtn").transform.Find("QuestMenual").gameObject;

        if (go.activeSelf == true)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            _queststatus.color = new Color(0,1,0,0);

            if(UserInfoClass._instance._isQuestIng || UserInfoClass._instance._isQuestComp)
            {
                string[] txtarr;
                txtarr = UserInfoClass._instance.questManualInit();
                Text Title = go.transform.Find("Title").GetComponent<Text>();
                Title.text = txtarr[0].ToString();
                Text Content = go.transform.Find("Content").GetComponent<Text>();
                Content.text = txtarr[1].ToString();
                Text Score = go.transform.Find("Score").GetComponent<Text>();
                Score.text = txtarr[2].ToString();

            }


        }
    }

}
