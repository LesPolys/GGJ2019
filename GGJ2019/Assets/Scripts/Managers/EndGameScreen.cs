using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private Transform RankText = null;

    [SerializeField]
    private Transform RestartButton = null;

    [SerializeField]
    private Transform QuitButton = null;

    [SerializeField]
    private Transform ReceiptRoot = null;

    [SerializeField]
    private Text TotalAmountText = null;

    [SerializeField]
    private TransactionEntryUI TansactionEntryPrefab = null;
    
    #endregion

    private bool UpdateTotalAmount = false;
    private float ValueIncrementMultiplier = 5f;

    private float ShownAmount = 0f;
    private float TotalAmount = 0f;
    private Coroutine EndGameScreenCoroutine = null;

    private List<Transaction> m_Receipt;
    private string m_Grade;

    private void Awake()
    {
        if (EndGameScreenCoroutine == null)
        {
            EndGameScreenCoroutine = StartCoroutine(Coroutine_ShowEndGameScore(m_Receipt, m_Grade));
        }
    }

    #region Public

    public void ShowEndGameScore(ref List<Transaction> receipt, ref string grade)
    {
        gameObject.SetActive(true);
        m_Receipt = receipt;
        m_Grade = grade;
    }

    #endregion

    #region Coroutines

    private IEnumerator Coroutine_ShowEndGameScore(List<Transaction> receipt, string grade)
    {
        yield return StartCoroutine(Coroutine_PopIn(ReceiptRoot));

        StartCoroutine(Coroutine_UpdateTotalAmount());
        yield return StartCoroutine(Coroutine_AddGigEntry("Moving Trials", "$1800"));
        TotalAmount += 1800;
        
        foreach(Transaction transaction in receipt)
        {
            yield return StartCoroutine(Coroutine_AddReceiptEntry(transaction.Name, transaction.Value.ToString()));
            TotalAmount += transaction.Value;
        }

        StartCoroutine(Coroutine_PopIn(RestartButton));
        StartCoroutine(Coroutine_PopIn(QuitButton));

        EndGameScreenCoroutine = null;
    }

    private IEnumerator Coroutine_AddGigEntry(string name, string amount) { yield return null; }

    private IEnumerator Coroutine_AddReceiptEntry(string transactionName, string amount) { yield return null; }

    private IEnumerator Coroutine_PopIn(Transform item) { yield return null; }

    private IEnumerator Coroutine_PopOut(Transform item) { yield return null; }

    

    private IEnumerator Coroutine_UpdateTotalAmount()
    {
        UpdateTotalAmount = true;
        while(UpdateTotalAmount)
        {
            yield return null;

            if (ShownAmount < TotalAmount)
            {
                ShownAmount += 5.53f * Time.deltaTime * ValueIncrementMultiplier;
            }
            if(ShownAmount > TotalAmount)
            {
                ShownAmount = TotalAmount;
            }
            TotalAmountText.text = ShownAmount.ToString();
        }
    }

    #endregion
}
