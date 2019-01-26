using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager s_instance = null;

    public static UIManager Instance { get { return s_instance; } }

    private void Awake()
    {
        s_instance = this;
    }


}
