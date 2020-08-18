using UnityEngine;
using Photon.Pun;
using TNOffice.Scoring;

namespace TNOffice
{
    public class PlayerCollision : MonoBehaviourPun
    {
        [SerializeField] private float collisionFactor = 50f;
        [SerializeField] private GameObject damageParticlesPrefab = null;

        //
        private int frontHitFactor = 0;   // No damage when hit from front
        private int belowHitFactor = 0;  // No damage when hit from below
        private int aboveHitFactor = 1;   // 1X damage from above
        private int sideHitFactor = 1;    // 1X damage from side
        private int rearHitFactor = 2;    // 2X damage from rear

        void OnCollisionEnter(Collision other)
        {
            // Check if current player collided with another
            // Figure out which player is traveling faster.
            // If the player's velocity is high enough, that player wins
            // Deduct points from the losing player 

            if (photonView.IsMine && other.collider.tag == "Player" && !other.collider.gameObject.GetPhotonView().IsMine)
            {
                float collisionForce = other.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log("PlayerCollision: === CURRENT PLAYER COLLIDED WITH ANOTHER PLAYER - FORCE: " + collisionForce + " ===");

                var relativePosition = transform.InverseTransformPoint(other.transform.position);

                int scoreAdjustment = 0;
                if (relativePosition.x > 0)
                {
                    // Debug.Log("The object is to the right");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * sideHitFactor;
                }
                else
                {
                    // Debug.Log("The object is to the left");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * sideHitFactor;
                }

                if (relativePosition.y > 0)
                {
                    // Debug.Log("The object is above.");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * aboveHitFactor;
                }
                else
                {
                    // Debug.Log("The object is below.");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * belowHitFactor;
                }

                if (relativePosition.z > 0)
                {
                    // Debug.Log("The object is in front.");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * frontHitFactor;
                }
                else
                {
                    // Debug.Log("The object is behind.");
                    scoreAdjustment += Mathf.FloorToInt(collisionForce / collisionFactor) * rearHitFactor;
                }

                ScoringSystem.theScore = Mathf.FloorToInt(
                    Mathf.Max(ScoringSystem.theScore - scoreAdjustment, 0)
                );

                if (scoreAdjustment > 0)
                {
                    PhotonNetwork.Instantiate(
                        damageParticlesPrefab.name,
                        new Vector3(
                            gameObject.transform.position.x,
                            gameObject.transform.position.y,
                            gameObject.transform.position.z
                        ),
                        Quaternion.identity
                    );
                }
            }
            else
            {
                Debug.Log("PlayerCollision: === COLLISION WITH SOMETHING ELSE ===");
            }
        }
    }
}