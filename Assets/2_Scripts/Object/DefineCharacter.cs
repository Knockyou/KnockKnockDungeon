using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineCharacter : MonoBehaviour
{
    float _movSpeed;
    float _attPow;
    float _hp;
    float _mp;
    GameObject _QSkill;
    GameObject _wSkill;
    GameObject _ESkill;

    public DefineCharacter() { }
    public DefineCharacter(float speed, float pow, float hp, float mp, GameObject QS, GameObject WS, GameObject ES)
    {
        _movSpeed = speed;
        _attPow = pow;
        _hp = hp;
        _mp = mp;

        _QSkill = QS;
        _wSkill = WS;
        _ESkill = ES;
    }
}
