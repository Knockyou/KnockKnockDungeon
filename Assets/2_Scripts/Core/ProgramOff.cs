using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramOff : MonoBehaviour
{
    static public void Poweroff()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

}
