using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum MonPersonality
{
    Boring      = 0, //움직이지 않음, 때리면 가만히 있고 떄렸는데 사용자가 범위에 있으면 때리고 없으면 가만히 있음
    Basic,           //때리면 쫒아오다 멀어지면 안옴
    Active,          //때리면 계속 쫒아옴
    Upset,            //때리기 전에 마주치면 와서 공격함 계속 쫒아옴
    Max
}

enum MonAni
{
    IDLE = 0,
    Walk,
    ATTACK,
    HIT,
    DEATH
}


public class ChickControl : MonoBehaviour
{
    Animator _monAni;
    float _movSpeed;
    [SerializeField] Slider _hp;
    [SerializeField] BoxCollider _hitBox;
    [SerializeField] SphereCollider _thinkArea;
    [SerializeField] Canvas _hpbar;
    MonPersonality _MonPer;
    static System.Random rd = new System.Random();
    int temp = rd.Next(0, (int)MonPersonality.Max);
    GameObject _attackbox;
    bool _isDeath = false;
    public AbilityClass _ability;
    bool _durationHit = false;
    float _durationDamage;
    public int rewardGold = 0;

    GameObject _MapInit;

    void Awake()
    {
        _ability = new AbilityClass(0, 1);
        //rewardGold = _ability.MonGold;
        _monAni = gameObject.GetComponent<Animator>();
        _attackbox = transform.Find("AttackBox").gameObject;
        _MapInit = GameObject.Find("Map").gameObject;
    }

    void start()
    {//ThinkArea
        
        
        //_attackbox.SetActive(true);
        _MonPer = (MonPersonality)temp;
        _movSpeed = _ability._MovSpeed;
        _hp.value = _ability._Hp;

        
        //Debug.Log(_MapInit.name);
    }

   

    void Update()
    {

        if(_hp.value == 0)
        {
            _isDeath = true;
            ChangeAni(MonAni.DEATH);
        }

        if(_durationHit)
        {
            StartCoroutine(DurationHitFun());
        }


        /*RaycastHit hit;
        float rayDistance = 6.0f;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), transform.forward * rayDistance, Color.blue, 0.3f);

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), transform.forward, out hit, rayDistance))
        {
            if(hit.transform.tag == "Player")
            {
                transform.LookAt(hit.transform);
                ChangeAni(MonAni.Walk);
                transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, Time.deltaTime * _movSpeed);
            }
        }*/
        //_hp.transform.up(0,1,0);



        //_hpbar.transform.Rotate(0, 0, 0);
    }

    IEnumerator DurationHitFun()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("HitEffect/OrangeHit"), transform.position, Quaternion.identity);
        Hitmanager(_durationDamage);
    }

    void ChangeAni(MonAni status, int type = 0) // type -> 1 : hotpotato
    {
        switch (status)
        {
            case MonAni.IDLE:
                break;
            case MonAni.Walk:
                _monAni.SetBool("Walk",true);
                break;
            case MonAni.ATTACK:
                _monAni.SetTrigger("ATTACK");
                /*Vector3 attPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(Resources.Load<GameObject>("HitEffect/Hit"), attPos, Quaternion.identity);*/
                break;
            case MonAni.HIT:
                _monAni.SetTrigger("HIT");
                _monAni.SetBool("IDLE", true);
                Vector3 hitPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                if(type == 1)
                {
                    Instantiate(Resources.Load<GameObject>("HitEffect/HitRedRandomText"), hitPos, Quaternion.identity);
                }
                else
                    Instantiate(Resources.Load<GameObject>("HitEffect/Blood"), hitPos, Quaternion.identity);
                break;
            case MonAni.DEATH:
                _monAni.SetTrigger("Death");
                Instantiate(Resources.Load<GameObject>("HitEffect/Death"), transform.position, Quaternion.identity);
                Vector3 coinPos = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                GameObject go = Instantiate(Resources.Load<GameObject>("Object/CoinObject"), coinPos, Quaternion.identity);
                CoinObject cost = go.GetComponent<CoinObject>();
                cost._cost = _ability.MonGold; //Error
                //cost._cost = rewardGold;
                //Debug.Log(rewardGold);
                _MapInit.GetComponent<MapInit>()._monlive += 1;
                _MapInit.GetComponent<MapInit>()._totalExp += _ability._MonExp;
                _MapInit.GetComponent<MapInit>()._getGoldCount += _ability.MonGold;
                Destroy(this.gameObject);
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
        if(other.collider.name == "AttackBox")
        {
            //transform.LookAt(other.transform.position);
            
            ChangeAni(MonAni.HIT);
            //Vector3 hitPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            float damage = other.gameObject.GetComponent<PlayerControl>()._ability.AttManager("d");
            Hitmanager(damage);
            ChangeAni(MonAni.ATTACK);
        }

        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PurpleAuraSphere")
        {
            _durationHit = true;
            //_durationDamage = other.gameObject.GetComponent<PlayerControl>()._wskillDamge;
            //_durationDamage = other.gameObject.transform.parent.GetComponent<PlayerControl>()._wskillDamge;
            _durationDamage = other.gameObject.transform.parent.GetComponent<PlayerControl>()._ability.AttManager("w");
            //_durationDamage = _durationDamage * 0.1f;
            //_durationDamage = _durationDamage;
        }
        if (other.tag == "HotPotato")
        {
            //Instantiate(Resources.Load<GameObject>("HitEffect/HitRedRandomText"), transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            ChangeAni(MonAni.HIT, 1);
            //Vector3 hitPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            float damage = other.gameObject.GetComponent<ThrowObj>()._damage;//
            Hitmanager(damage);
            //ChangeAni(MonAni.ATTACK, 1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "PurpleAuraSphere")
        {
            _durationHit = false;
            _durationDamage = 0;
        }
    }

    void Hitmanager(float damge)
    {
        //Debug.Log("병아리 몇 맞음 : " + damge);
        _hp.value -= damge;
    }
}
