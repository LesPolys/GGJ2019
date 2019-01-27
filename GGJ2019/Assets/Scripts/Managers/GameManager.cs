using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_SecondsUntilGameStart = 5f;

    #region Singlton
    private static GameManager s_instance = null;
    public static GameManager Instance { get { return s_instance; } }
    #endregion
    
    private float m_TimeRemaining = 5f; // In Seconds

    private List<Transaction> m_Reciept;



    //[SerializeField]
    //List<Transform> spawnPoints;
    [SerializeField]
    List<Furniture> houseFurniture;
    
    private void Awake()
    {
        s_instance = this;
        m_Reciept = new List<Transaction>();
    }

    private IEnumerator Start()
    {
        while(m_SecondsUntilGameStart > 0f)
        {
            yield return null;
            m_SecondsUntilGameStart -= Time.deltaTime;
        }
        StartGame();
    }


    public void StartGame()
    {
        StartCoroutine(Coroutine_Update());
    }

    private void EndGame()
    {
        Debug.Log("Game Ended!");
        UIManager.Instance.ShowEndGameScreen(ref m_Reciept, "Eh+");
    }

    private void ShowTransactionUI(Transform origin, int value)
    {
        if (origin != null)
        {
            // TODO: Display the difference
        }
        else
        {
            Debug.LogWarning("An origin was not given for the transaction. No UI will be shown.");
        }
    }


    #region Public

    public void AddToReceipt(Transform origin, string name, float value)
    {
        m_Reciept.Add(new Transaction() { Value = (int)value, Name = name, Origin = origin });
        ShowTransactionUI(origin, (int)value);
    }

    public void RemoveFromReceipt(RequestCostAdjustmentDelegate callback)
    {
        for(int i = 0; i < m_Reciept.Count; i++)
        {
            Transaction transaction = m_Reciept[i];
            if(transaction.Callback == callback)
            {
                m_Reciept.Remove(transaction);
                ShowTransactionUI(transaction.Origin, transaction.Value);
            }
        }
    }

    public void AddAllItemsToReceipt()
    {
        foreach(Furniture piece in houseFurniture)
        {
            AddToReceipt(piece.gameObject.transform, piece.furnitureName, piece.finalScore);
        }
    }

    #endregion


    #region Coroutines

    private IEnumerator Coroutine_Update()
    {
        Debug.Log("Game Started!");
        while (m_TimeRemaining > 0f)
        {
            yield return null;
            m_TimeRemaining -= Time.deltaTime;
        }
        AddAllItemsToReceipt();
        EndGame();
    }

    #endregion

}

public delegate float RequestCostAdjustmentDelegate();
public struct Transaction
{
    public int Value;
    public string Name;
    public Transform Origin;
    public RequestCostAdjustmentDelegate Callback;
}