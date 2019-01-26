using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singlton
    private static GameManager s_instance = null;
    public static GameManager Instance { get { return s_instance; } }
    #endregion
    
    private float m_TimeRemaining = 120f; // In Seconds
    private UIManager m_UIManager = null;

    public delegate float RequestCostAdjustmentDelegate();
    private List<RequestCostAdjustmentDelegate> m_Reciept;
    
    private void Awake()
    {
        s_instance = this;
    }

    public void AddToReciept(RequestCostAdjustmentDelegate callback) { }

    public void RemoveFromReciet(RequestCostAdjustmentDelegate callback) { }
}
