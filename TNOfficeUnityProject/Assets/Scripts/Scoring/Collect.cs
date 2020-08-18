using System.Collections;
using UnityEngine;
using Photon.Pun;
using TNOffice.Audio;

namespace TNOffice.Scoring
{
    public class Collect : MonoBehaviourPun
    {
        private Animator _animator = null;

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!_animator.GetBool("IsExiting") && collider.tag == "Player" && collider.gameObject.GetPhotonView().IsMine)
            {
                Debug.Log("=== COLLISION WITH PLAYER ===");

                // Tell the server that we collected this item
                // If the server agrees that this item has not been collected,
                // then it will award points and start tearing down the collectible
                photonView.RPC("SetCollectedServer", RpcTarget.MasterClient);
            }
            else
            {
                Debug.Log("=== COLLISION WITH OTHER ===");
            }
        }

        IEnumerator OnCompleteExitAnimation()
        {
            yield return new WaitForSeconds(.5f);
            Debug.Log("Coroutine Checking animation state: " + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            photonView.RPC("DestroyCollectible", RpcTarget.MasterClient);
        }

        [PunRPC]
        private void AwardPoints()
        {
            // Increment this client's score
            ScoringSystem.theScore += 50;
            AudioManager.instance.Play("CollectFood");

            // Call coroutine, which will trigger destruction of the collider,
            // once the exit animation has completed.
            StartCoroutine("OnCompleteExitAnimation");
            // photonView.RPC("DestroyCollectible", RpcTarget.MasterClient);
        }

        [PunRPC]
        private void SetExitingClients()
        {
            _animator.SetBool("IsExiting", true);
        }

        [PunRPC]
        private void SetCollectedServer(PhotonMessageInfo info)
        {

            if (!_animator.GetBool("IsExiting"))
            {
                // If the server agrees with the client that the item is
                // available to collect, then mark it as 'collected' and
                // award points to the sender.
                photonView.RPC("SetExitingClients", RpcTarget.AllBuffered);
                photonView.RPC("AwardPoints", info.Sender);
            }
        }

        [PunRPC]
        private void DestroyCollectible()
        {
            Debug.Log("DestroyCollectible.IsMasterClient: " + PhotonNetwork.IsMasterClient);

            if (PhotonNetwork.IsMasterClient && gameObject.GetPhotonView().IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}