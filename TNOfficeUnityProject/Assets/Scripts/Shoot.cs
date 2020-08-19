using UnityEngine;
using Photon.Pun;
using TNOffice.Scoring;
using TNOffice.Notifications;

namespace TNOffice.Shooting
{
    public class Shoot : MonoBehaviourPun
    {
        [SerializeField] private float projectileSpeed = 5f;
        [SerializeField] private GameObject projectile = null;
        [SerializeField] private Transform spawnPoint = null;

        [SerializeField] private int unlockScore = 2000;

        private bool _unlocked = false;

        // Update is called once per frame
        void Update()
        {
            // Users unlock the confetti shooter at 2000 points
            if (!_unlocked && ScoringSystem.theScore >= unlockScore)
            {
                // Confetti Shooter unlocked!
                _unlocked = true;

                // Trigger event to let the user know
                NotificationManager.instance.AddMessage((int)unlockScore + " points! You earned the confetti gun!");
                NotificationManager.instance.AddMessage("Hold the Left Mouse Button to shoot confetti!");
            }

            // User lost their confetti ability
            if (_unlocked && ScoringSystem.theScore < unlockScore)
            {
                _unlocked = false;

                // Trigger event to let the user know
                NotificationManager.instance.AddMessage("Oh noes! You lost the confetti gun.");
            }

            if (photonView.IsMine)
            {
                TakeInput();
            }
        }

        private void TakeInput()
        {
            // Fire as long as left mouse is held down.
            if (_unlocked && Input.GetMouseButton(0))
            {
                FireProjectile();
            }
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
