using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionEntryUI : MonoBehaviour
{
    public Text EntryName;
    public Text EntryAmount;

    public void InitTransactionEntry(string name, string amount)
    {
        SetEntryName(name);
        SetEntryAmount(amount);

        gameObject.SetActive(true);
    }

    public void SetEntryName(string name)
    {
        if(EntryName != null)
        {
            EntryName.text = name;
        }
    }

    public void SetEntryAmount(string amount)
    {
        if(EntryAmount != null)
        {
            EntryAmount.text = amount;
        }
    }
}
