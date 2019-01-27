using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private float ValueIncrementRate = 5.53f;

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
    private TransactionEntryUI JobEntryText = null;

    [SerializeField]
    private Transform TransactionsRoot = null;

    [SerializeField]
    private TransactionEntryUI TansactionEntryPrefab = null;

    [SerializeField]
    private AnimationCurve PopEffect;
    
    #endregion

    private bool UpdateTotalAmount = false;
    private float ValueIncrementMultiplier = 15f;

    private double ShownAmount = 0f;
    private double TotalAmount = 0f;
    private Coroutine EndGameScreenCoroutine = null;

    private List<Transaction> m_Receipt;
    private string m_Grade;
    private string m_GigName;
    private double m_GigReward;

    private void Awake()
    {
        if (EndGameScreenCoroutine == null)
        {
            EndGameScreenCoroutine = StartCoroutine(Coroutine_ShowEndGameScore());
        }

        QuitButton.GetComponent<Button>().onClick.AddListener(OnQuitPressed);
        RestartButton.GetComponent<Button>().onClick.AddListener(OnRestartPressed);

      
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnQuitPressed()
    {
        Application.Quit();
    }

    private void OnRestartPressed()
    {
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(0);
    }

    #region Public

    public void ShowEndGameScore(ref List<Transaction> receipt, ref string grade, string gigName, double gigReward)
    {
        gameObject.SetActive(true);
        m_Receipt = receipt;
        RankText.GetComponent<Text>().text = grade;
        m_GigName = gigName;
        m_GigReward = gigReward;

    }

    #endregion

    #region Coroutines

    private IEnumerator Coroutine_ShowEndGameScore()
    {
        yield return (Coroutine_PopIn(ReceiptRoot));

        StartCoroutine(Coroutine_UpdateTotalAmount());
        yield return (Coroutine_AddGigEntry(m_GigName.ToString(), m_GigReward.ToString()));
        TotalAmount += m_GigReward;

        if (m_Receipt != null)
        {
            foreach (Transaction transaction in m_Receipt)
            {
                yield return (Coroutine_AddReceiptEntry(transaction.Name, transaction.Value.ToString()));
                TotalAmount += transaction.Value;
            }
        }
        StartCoroutine(Coroutine_PopIn(RestartButton));
        StartCoroutine(Coroutine_PopIn(QuitButton));

        StartCoroutine(Coroutine_PopIn(RankText));

        EndGameScreenCoroutine = null;
    }


    WaitForSeconds delay = new WaitForSeconds(0.05f);
    private IEnumerator Coroutine_AddGigEntry(string name, string amount)
    {
        for(int i = 0; i < name.Length; i++)
        {
            yield return delay;

            JobEntryText.EntryName.text += name[i];
        }

        for (int i = 0; i < amount.Length; i++)
        {
            yield return delay;

            JobEntryText.EntryAmount.text += amount[i];
        }
    }

    private IEnumerator Coroutine_AddReceiptEntry(string transactionName, string amount)
    {
        TransactionEntryUI entry = GameObject.Instantiate(TansactionEntryPrefab.gameObject, Vector3.zero, Quaternion.identity, TransactionsRoot).GetComponent<TransactionEntryUI>();
        entry.gameObject.SetActive(true);

        for (int i = 0; i < transactionName.Length; i++)
        {
            yield return delay;

            entry.EntryName.text += transactionName[i];
        }

        for (int i = 0; i < amount.Length; i++)
        {
            yield return delay;

            entry.EntryAmount.text += amount[i];
        }
    }

    private IEnumerator Coroutine_PopIn(Transform item)
    {
        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.zero;
        yield return null;

        float dt = 0f;
        while(dt < 1)
        {
            item.transform.localScale = Vector3.one * PopEffect.Evaluate(dt);
            dt += Time.deltaTime * 2f;

            yield return null;
        }
        item.transform.localScale = Vector3.one;
    }

    private IEnumerator Coroutine_PopOut(Transform item)
    {
        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.zero;
        yield return null;

        float dt = 1f;
        while (dt > 0)
        {
            item.transform.localScale = Vector3.one * PopEffect.Evaluate(dt);
            dt -= Time.deltaTime * 2f;

            yield return null;
        }
        item.transform.localScale = Vector3.one;
    }

    

    private IEnumerator Coroutine_UpdateTotalAmount()
    {
        UpdateTotalAmount = true;
        while(UpdateTotalAmount)
        {
            yield return null;

            if (ShownAmount < TotalAmount)
            {
                ShownAmount += ValueIncrementRate * Time.deltaTime * ValueIncrementMultiplier;
                ShownAmount = System.Math.Round(ShownAmount, 2);
            }
            if(ShownAmount > TotalAmount)
            {
                ShownAmount = TotalAmount;
            }
            TotalAmountText.text = "$" + ShownAmount.ToString();
        }
    }

    #endregion
}
