using System.Collections;
using TMPro;
using UnityEngine;
using Photon.Pun;

namespace TNOffice
{
    public class PlayerNameTag : MonoBehaviourPun
    {
        [SerializeField] private TextMeshProUGUI nameText;

        void Start()
        {
            Debug.Log("NameTagStart:" + photonView.Owner.NickName);
            if (photonView.IsMine) { return; }
            SetName();
        }

        private void SetName()
        {
            Debug.Log("NameTag.SetName: " + photonView.Owner.NickName);
            nameText.text = photonView.Owner.NickName;
        }
    }
}
