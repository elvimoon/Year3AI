using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class NetworkHUD : MonoBehaviour
{
	public NetworkManager manager;
	[SerializeField] public bool showGUI = true;
	[SerializeField] public int offsetX;
	[SerializeField] public int offsetY;

	// Runtime variable
	bool showServer = false;

	void Awake()
	{
		manager = GetComponent<NetworkManager>();
	}

	void Update()
	{
		if (!showGUI)
			return;

		if (!NetworkClient.active && !NetworkServer.active)
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				manager.StartServer();
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				manager.StartHost();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				manager.StartClient();
			}
		}
		if (NetworkServer.active && NetworkClient.active)
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				manager.StopHost();
			}
		}
	}

	void OnGUI()
	{
		if (!showGUI)
			return;

		int xpos = 500 + offsetX;
		int ypos = 300 + offsetY;
		int spacing = 30;

		if (!NetworkClient.active && !NetworkServer.active)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
			{
				manager.StartHost();
			}
			ypos += spacing;

			if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
			{
				manager.StartClient();
			}
			manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
			ypos += spacing;

			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
			{
				manager.StartServer();
			}
			ypos += spacing;
		}
		else
		{
			if (NetworkServer.active)
			{
				GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkAddress);
				ypos += spacing;
			}
			if (NetworkClient.active)
			{
				GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkAddress);
				ypos += spacing;
			}
		}

		if (NetworkClient.active)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
			{
				NetworkClient.Ready();

				if (NetworkClient.localPlayer == null)
				{
					NetworkClient.AddPlayer();
				}
			}
			ypos += spacing;
		}

		if (NetworkServer.active || NetworkClient.active)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
			{
				manager.StopHost();
			}
			ypos += spacing;
		}
	}
}
