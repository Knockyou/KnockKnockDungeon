using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    public int _cost;

    void Update()
    {
        transform.Rotate(new Vector3(0, 360, 0), Time.deltaTime * 350.0f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            PlayerControl pc;
            pc = other.gameObject.GetComponent<PlayerControl>();
            UserInfoClass._instance.Gold += _cost;
            TcpClient._instance.GetGold(UserInfoClass._instance.Gold);
            Destroy(this.gameObject);
        }
    }
}


