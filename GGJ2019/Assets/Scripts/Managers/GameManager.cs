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
    
    private void Awake()
    {
        s_instance = this;
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

    #region Private

    public void StartGame()
    {
        StartCoroutine(Coroutine_Update());
    }

    private void EndGame()
    {
        Debug.Log("Game Ended!");
        UIManager.Instance.ShowEndGameScreen(ref m_Reciept, "Eh #"); // TODO: Pass in receipt transactions
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

    #endregion

    #region Public

    public void AddToReceipt(RequestCostAdjustmentDelegate callback, Transform origin, string Name, int value)
    {
        for (int i = 0; i < m_Reciept.Count; i++)
        {
            Transaction transaction = m_Reciept[i];

            if (transaction.Callback == callback)
            {
                transaction.Origin = origin;

                ShowTransactionUI(origin, transaction.Value - value);
                transaction.Value = value;
                transaction.Name = name;

                return;
            }
        }

        m_Reciept.Add(new Transaction() { Value = value, Name = name, Callback = callback, Origin = origin });
        ShowTransactionUI(origin, value);
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