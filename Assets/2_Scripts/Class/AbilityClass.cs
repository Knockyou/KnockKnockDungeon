using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClass : MonoBehaviour
{
    //캐릭터는 초기에 한번 실행
    //몬스터는 마리수마다 실행
    public int _Index;
    public int _Type;  //몬스터인지 캐릭터인지 캐릭:0, 몬스터:1
    public float _AttPow; //공격력 수치
    public int _Hp;
    public int _Mp;
    public float _MovSpeed; //현재 캐릭터 이속
    public int _Gold; //캐릭터 : 소유금,   몬스터 : 처치시 주는 골드
    public string _Item; //캐릭터 : 보유 아이템,  몬스터 : 처치시 주는 골드

    //인덱스로 받아서 이미지랑 이펙트 리소스에서 가져올 계획
    public int _Qskill;
    public int _Wskill;
    public int _Eskill;

    //사용자만 사용하는 변수, 던전 들어갈시에 true로 변경
    public bool _isBattle = false;

    //몬스터용 변수
    public int _MonExp; //몬스터 처치시 주는 겅험치
    public int _MonGold; //몬스터 처치시 주는 골드

    public int MonGold
    {
        get { return _MonGold; }
        set { _MonGold = value;  }
    }


    public AbilityClass(){}

    //몬스터, 캐릭터 생성자
    public AbilityClass(int index, int type) // type : 0 -> 캐릭터, type : 1 -> 몬스터
    {
        _Index = index;
        _Type = type;
        LoadInitInfo(); //임시 앞에는 0 : 캐릭터 뒤에는 캐릭터 테이블 인덱스
    }

    

    void LoadInitInfo() // type : 0 -> 캐릭터, type : 1 -> 몬스터
    {
        string path = "CharacterInfo/";
        if (_Type == 0)
            path += "CharacterInfoTable.xml";
        else
            path += "MonsterInfoTable.xml";
        XmlDocument xmlFile = new XmlDocument();

        if (File.Exists(path))
        {
            xmlFile.Load(path);
            XmlNodeList elemenList = xmlFile.GetElementsByTagName("index");

            for (int i = 0; i < elemenList.Count; i++)
            {
                if (elemenList[i].InnerText == _Index.ToString())
                {
                    if (_Type == 0)
                    {
                        _AttPow = int.Parse(elemenList[i].NextSibling.InnerText) * 0.02f;
                        _Hp = int.Parse(elemenList[i].NextSibling.NextSibling.InnerText) * UserInfoClass._instance.Level;
                        _Mp = int.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.InnerText) * UserInfoClass._instance.Level;
                        _MovSpeed = float.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.NextSibling.InnerText);
                    }
                    else
                    {
                        _MonExp = int.Parse(elemenList[i].NextSibling.InnerText);
                        _AttPow = int.Parse(elemenList[i].NextSibling.NextSibling.InnerText) * 0.02f;
                        _Hp = int.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.InnerText);

                        System.Random rd = new System.Random();
                        _MonGold = rd.Next(
                            int.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.NextSibling.InnerText),
                            int.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText)+1);
                        
                        Debug.Log(MonGold);
                        _Item = elemenList[i].NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                        _MovSpeed = float.Parse(elemenList[i].NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText);
                    }
                }
            }
        }

    }
 


    

    //공격데미지 매니저 추가해야함
    public float AttManager(string keyVal) // type : -1 기본, 1 : 퍼뎀, 2 : 버프
    {
        //공격력 공식
        //(att * level) * SkillDefine * 100 / 1
        float rootDamage = 0.0f;
        float returnDamage = 0.0f;
        float defD = 0.0f;
        switch (keyVal)
        {
            case "d":
            case "D":
                rootDamage = UserInfoClass._instance.Level * _AttPow;
                if (_Index == 0)
                {
                    defD = 3.0f;
                    returnDamage = (rootDamage * defD)*1.0F;
                }
                break;
            case "q":
            case "Q":

                rootDamage = UserInfoClass._instance.Level * _AttPow;
                if (_Index == 0)
                {
                    defD = 5.0f;
                    returnDamage = (rootDamage * defD) * 1.0F;
                }
                break;
            case "w":
            case "W":
                rootDamage = UserInfoClass._instance.Level * _AttPow;
                if (_Index == 0)
                {
                    defD = 0.06f;
                    returnDamage = (rootDamage * defD) * 1.0F;

                }
                break;
            case "e":
            case "E":
                rootDamage = UserInfoClass._instance.Level * _AttPow;
                if (_Index == 0)
                {/*0번 캐릭터의 E스킬은 공속 스킬이라 생략*/}
                break;

            case "MonChick":
                if (_Index == 0)
                {
                    returnDamage = _AttPow * 6.0f;
                }
                break;

            default:
                break;
        }

        return returnDamage;
    }


    public float MpFun(string keyVal)
    {
        float returnmpsize = 0.0f;
        float rootmp = (UserInfoClass._instance.Level * _Mp)*0.1f;
        float defM = 0.0f;
        switch (keyVal)
        {
            case "q":
            case "Q":
                rootmp = (UserInfoClass._instance.Level * _Mp) * 0.01f;
                if (_Index == 0)
                {
                    defM = 0.3f;
                    returnmpsize = (rootmp * (defM * 1.0f)) * 1.0f;
                }
                break;
            case "w":
            case "W":
                rootmp = (UserInfoClass._instance.Level * _Mp) * 0.01f;
                if (_Index == 0)
                {
                    defM = 0.7f;
                    returnmpsize = (rootmp * (defM * 1.0f)) * 1.0f;

                }
                break;
            case "e":
            case "E":
                rootmp = (UserInfoClass._instance.Level * _Mp) * 0.01f;
                if (_Index == 0)
                {
                    defM = 0.5f;
                    returnmpsize = (rootmp * (defM * 1.0f)) * 1.0f;
                }
                break;

            default:
                break;
        }

        return returnmpsize;
    }




    
    public void HitManager(int inAttPow)
    {
        _Hp -= inAttPow;
    }


    

    //레벨업, 경험치, hp, mp .....
    public void updatePlayerCharacter()
    {

    }





}
