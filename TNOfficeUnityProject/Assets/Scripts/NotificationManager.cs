using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TNOffice.Notifications
{
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI notificationText = null;
        [SerializeField] private Animator animator = null;

        private bool _showingMessage = false;
        private Queue<string> messages;

        public static NotificationManager instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            messages = new Queue<string>();
        }

        void Update()
        {
            if (!_showingMessage && messages.Count > 0)
            {
                ShowNextMessage();
            }
        }

        private void ShowNextMessage()
        {
            _showingMessage = true;
            string message = messages.Dequeue();
            notificationText.text = message;
            animator.SetBool("isOpen", true);
            StartCoroutine("MessageTimer");
        }

        IEnumerator MessageTimer()
        {
            yield return new WaitForSeconds(2f);
            animator.SetBool("isOpen", false);
            yield return new WaitForSeconds(1f);
            _showingMessage = false;
        }

        public void AddMessage(string message)
        {
            messages.Enqueue(message);
        }
    }
}