using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;


namespace TNOffice.Menus
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string name)
        {
            Debug.Log("SetPlayerName.name:" + name);
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            Debug.Log("Saving Player Name...");
            string playerName = nameInputField.text;

            Debug.Log("Saving as: " + playerName);
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }
    }
}