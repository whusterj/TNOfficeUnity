using UnityEngine;
using Photon.Pun;

namespace TNOffice.Shooting
{
    public class Shoot : MonoBehaviourPun
    {
        [SerializeField] private float projectileSpeed = 5f;
        [SerializeField] private GameObject projectile = null;
        [SerializeField] private Transform spawnPoint = null;

        void Start()
        {
            // Debug.Log("Shoot.Start");
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log("Shoot.Update");
            if (photonView.IsMine)
            {
                TakeInput();
            }
        }

        private void TakeInput()
        {
            // Debug.Log("Taking player input...");

            // Fire on left mouse button click.
            if (!Input.GetMouseButtonDown(0)) { return; }
            FireProjectile();
        }

        private void FireProjectile()
        {
            Debug.Log("SHOOTING!");

            for (int i = 0; i < 20; i++)
            {
                var projectileInstance = PhotonNetwork.Instantiate(
                    projectile.name,
                    spawnPoint.position,
                    spawnPoint.rotation
                );
                projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileSpeed;
            }
        }
    }
}
