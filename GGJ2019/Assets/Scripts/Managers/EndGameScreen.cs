using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField]
    private Transform RankText = null;

    [SerializeField]
    private Transform RestartButton = null;

    [SerializeField]
    private Transform QuitButton = null;

    [SerializeField]
    private Transform ReceiptRoot = null;

    [SerializeField]
    private Text ReceiptText = null;

    // Starts the opening animations
    private void OnEnable() { }


    
    #region Coroutines

    private IEnumerator Coroutine_PopIn() { yield return null; }

    private IEnumerator Coroutine_PopOut() { yield return null; }

    #endregion
}
