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

                Vector3 randomVec3 = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                Vector3 randomized = projectileInstance.transform.up + randomVec3;
                projectileInstance.GetComponent<Rigidbody>().velocity = randomized * projectileSpeed * Random.Range(0.5f, 1.0f);
            }
        }
    }
}
