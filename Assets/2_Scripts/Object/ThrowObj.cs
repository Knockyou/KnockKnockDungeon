using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour
{
    public float _speed = 100.0f;
    Rigidbody rd;
    public float _destroyTime = 3.0f;

    Vector3 _localCreatePos;
    public float _destroyDist = 15.0f;

    public float _damage;


    void Awake()
    {
        _localCreatePos = transform.position;
        rd = GetComponent<Rigidbody>();
        rd.AddForce(transform.forward * _speed);
        Destroy(gameObject, _destroyTime);
    }

    void Update()
    {
        float distance = (transform.position - _localCreatePos).magnitude;
        if(distance >= _destroyDist)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Instantiate(Resources.Load<GameObject>("HitEffect/HitRedRandomText"), gameObject.transform.position, gameObject.transform.rotation);
        Destroy(this.gameObject);
    }

    /*void OnTriggerEnter(Collider other)
    {
        //Instantiate(Resources.Load<GameObject>("HitEffect/HitRedRandomText"), gameObject.transform.position, gameObject.transform.rotation);
        Destroy(this.gameObject);
    }*/
}
