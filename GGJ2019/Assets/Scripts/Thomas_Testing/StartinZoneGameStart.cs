using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartinZoneGameStart : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited!");

        GameManager.Instance.StartGame();
        this.enabled = false;
    }
}
