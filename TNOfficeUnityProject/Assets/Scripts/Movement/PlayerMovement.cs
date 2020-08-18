using Photon.Pun;
using UnityEngine;
using TNOffice.Audio;

namespace TNOffice.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviourPun
    {
        [SerializeField] public float movementSpeed = 2f;
        [SerializeField] private bool usePhoton = true;

        private CharacterController controller = null;
        private Transform mainCameraTransform = null;

        // Droning audio
        [SerializeField] private AudioSource droneAudioSource = null;
        [SerializeField] private float droneAudioScale = 0.6f;
        [SerializeField] private float droneAudioMinVolume = 0.1f;
        [SerializeField] private float droneAudioMaxVolume = 0.8f;

        // Temporary speed boost
        [SerializeField] private AudioSource boostAudioSource = null;
        [SerializeField] private float boostAmount = 2f;
        [SerializeField] private float boostCooldown = 2f;
        [SerializeField] private float boostVolume = 0.6f;
        private float currentBoost = 0f;
        private float boostTimer = 0f;
        private Vector3 boostVector = Vector3.zero;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            mainCameraTransform = Camera.main.transform;
        }

        void Update()
        {
            if (usePhoton && photonView.IsMine)
            {
                TakeInput();
            }

            // Enable input for testing purposes without Photon
            if (!usePhoton)
            {
                TakeInput();
            }
        }

        private void TakeInput()
        {
            var movement = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = 0f,
                z = Input.GetAxisRaw("Vertical"),
            }.normalized;

            // Get forward and right vectors of the main camera
            // The player will move relative to the facing of the camera
            Vector3 forward = mainCameraTransform.forward;
            Vector3 right = mainCameraTransform.right;

            // Remove the 'tilt' element of the vector
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 calculatedMovement = (forward * movement.z + right * movement.x).normalized;

            // Rotate the player to always look forwards
            if (calculatedMovement != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(calculatedMovement);
            }

            // Handle boost
            boostTimer = Mathf.Clamp(boostTimer - Time.deltaTime, 0f, boostCooldown);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (boostTimer <= 0.75 && calculatedMovement.magnitude > 0)
                {
                    photonView.RPC("PlayBoostSound", RpcTarget.All, boostVolume);
                    boostTimer = boostCooldown;
                    boostVector = calculatedMovement;
                }
            }
            currentBoost = (boostTimer / boostCooldown) * boostAmount;

            //
            Vector3 finalMoveVector = (calculatedMovement * (movementSpeed + currentBoost / 2)) + (boostVector * currentBoost);

            // Adjust the volume of the "engine" sound based on the magnitude
            // of the inputs. The more "gas" the player is giving it, the louder the sound
            Debug.Log("PlayerMovement.calculatedMovement.magnitude: " + calculatedMovement.magnitude);
            float droneVolume = Mathf.Clamp(
                droneAudioMinVolume + ((Mathf.Abs(calculatedMovement.magnitude)) / droneAudioScale),
                droneAudioMinVolume, droneAudioMaxVolume
            );
            photonView.RPC("SetDroneVolume", RpcTarget.AllBuffered, droneVolume);

            // Apply movement
            controller.SimpleMove(finalMoveVector);
        }

        [PunRPC]
        private void SetDroneVolume(float volume)
        {
            Debug.Log("PlayerMovement.SetDroneVolume: " + volume);
            droneAudioSource.volume = volume;
        }

        [PunRPC]
        private void PlayBoostSound(float volume = 1.0f)
        {
            boostAudioSource.PlayOneShot(boostAudioSource.clip, volume);
        }
    }
}