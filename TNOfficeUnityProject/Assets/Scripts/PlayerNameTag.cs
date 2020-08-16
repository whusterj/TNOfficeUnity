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
            if (photonView.IsMine) { return; }
            SetName();
        }

        private void SetName()
        {
            nameText.text = photonView.Owner.NickName;
        }
    }
}
