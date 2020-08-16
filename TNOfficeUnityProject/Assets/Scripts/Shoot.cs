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
            photonView.RPC("FireProjectile", RpcTarget.All);
        }

        // As a PunRPC, this will run on each client, so the results will not
        // necessarily be the same for everyone.
        [PunRPC]
        private void FireProjectile()
        {
            Debug.Log("SHOOTING!");

            for (int i = 0; i < 20; i++)
            {
                var projectileInstance = Instantiate(
                    projectile,
                    spawnPoint.position,
                    spawnPoint.rotation
                );
                projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileSpeed;
            }
        }
    }
}
