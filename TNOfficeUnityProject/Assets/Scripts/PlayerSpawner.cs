using System.Collections;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using TNOffice.Notifications;

namespace TNOffice
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private CinemachineFreeLook playerCamera = null;
        [SerializeField] private TextMeshProUGUI notificationText = null;

        void Start()
        {
            // This will also assign this object to the client
            // that did the instantiation.
            var player = PhotonNetwork.Instantiate(
                playerPrefab.name,
                Vector3.zero,
                Quaternion.identity
            );

            // Set the player camera to follow the player
            playerCamera.Follow = player.transform;
            playerCamera.LookAt = player.transform;

            NotificationManager.instance.AddMessage("Press 'E' to boost.");
            // notificationText.text = "Press 'E' to boost.";
        }
    }
}
