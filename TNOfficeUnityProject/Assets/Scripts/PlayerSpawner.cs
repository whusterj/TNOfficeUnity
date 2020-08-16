using Cinemachine;
using UnityEngine;
using Photon.Pun;
using TNOffice.Shooting;

namespace TNOffice
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private CinemachineFreeLook playerCamera = null;

        void Start()
        {
            // This will also assign this object to the client
            // that did the instantiation.
            var player = PhotonNetwork.Instantiate(
                playerPrefab.name,
                Vector3.zero,
                Quaternion.identity
            );

            // Change the scale of the player
            // player.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            // Set the player camera to follow the player
            playerCamera.Follow = player.transform;
            playerCamera.LookAt = player.transform;

            // Debug.Log("Shoot Script Enabled" + player.GetComponent<Shoot>().GetType());
        }
    }

}
