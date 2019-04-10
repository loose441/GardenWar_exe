using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectButton : MonoBehaviour
{
    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings("1.0v");
    }

    private void OnJoinedLobby()
    {
        Debug.Log("a");
    }
}
