using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkHUDAlt : MonoBehaviour
{
    NetworkManager netMan;
    public Text text;
    public GameObject button;
    public GameObject hostButton;
    public GameObject cancelHost;
    public InputField input;

    // Start is called before the first frame update
    void Start()
    {
        netMan = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
        netMan.StartHost();
        hostButton.SetActive(false);
        cancelHost.SetActive(true);
    }

    public void StartClient()
    {
        netMan.StartClient();

        if (NetworkClient.isConnected && !NetworkClient.active)
        {
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null)
            {
                NetworkClient.AddPlayer();
            }
        }
        else
        {
            // Cancel Client Connecting

            //Change Text From Connect to Connecting To...
            text.text = "Connecting to " + netMan.networkAddress;


            //Instantiate the Cancel Connect Button
            button.SetActive(true);
        }
    }

    public void StopClient()
    {
        netMan.StopClient();
        button.SetActive(false);
        text.text = "Connect As Client";
    }

    public void UpdateAddress()
    {
        if (input.text == "")
        {
            netMan.networkAddress = "localhost";
        }
        else
        {
            netMan.networkAddress = input.text;
        }
    }

    public void StopHost()
    {
        netMan.StopHost();
        cancelHost.SetActive(false);
        hostButton.SetActive(true);
    }
}
