using UnityEngine;
using Photon.Pun;

public class Food : MonoBehaviourPun
{
    [SerializeField] private float elapsed = 0f;
    [SerializeField] private float lifetime = 60f;

    void Update()
    {
        // Expire at the end of lifetime
        elapsed += Time.deltaTime;
        if (elapsed > lifetime && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
