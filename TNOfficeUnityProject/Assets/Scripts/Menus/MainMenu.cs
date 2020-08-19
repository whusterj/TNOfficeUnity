using TMPro;
using UnityEngine;
using Photon.Pun;

namespace TNOffice.Menus
{
    // Note that this subclasses Photon's Pun Callback
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject joinOfficePanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MinPlayersPerRoom = 2;
        private const int MaxPlayersPerRoom = 20;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void JoinOffice()
        {
            isConnecting = true;
            joinOfficePanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Connecting...";

            // Will join a game if you can
            if (PhotonNetwork.IsConnected)
            {
                // Already connected to Photon, so join a room...
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // If not connected, then connect to Photon with
                // our App ID and whatever other settings.
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            joinOfficePanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No one is waiting for a connection. Creating a new room...");

            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room.");

            // Conditional logic, if we wanted to wait for a certain number of
            // players to join our room.
            // int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            // if (playerCount < MinPlayersPerRoom)
            // {
            //     waitingStatusText.text = "Waiting for other players...";
            // }
            // else
            // {
            //     waitingStatusText.text = "Other players found!";
            //     Debug.Log("Game ready to begin");
            // }

            PhotonNetwork.LoadLevel("TN_Office");
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            // Stop new players from joining, when we've reached the max amount.
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
}