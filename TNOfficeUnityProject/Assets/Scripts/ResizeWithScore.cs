using UnityEngine;
using TNOffice.Scoring;
using Photon.Pun;
using Cinemachine;
using TNOffice.Movement;

namespace TNOffice.Player
{
    public class ResizeWithScore : MonoBehaviourPun
    {
        // Configure the score when the scale factor should be 1.0
        [SerializeField] private float fullSizeScore = 2500f;
        [SerializeField] private float minSize = 0.05f;
        // 
        private PlayerMovement _playerMovement = null;
        [SerializeField] private float _speedFactor = 2;

        // Variables to cache Cinemachine components
        private CinemachineFreeLook _cameraController = null;
        private CinemachineComposer[] _rigComps = null;

        private void Start()
        {
            // Cache the Player's movement component
            _playerMovement = GetComponent<PlayerMovement>();

            // Cache the Camera Controller and Rig Composers
            _cameraController = FindObjectOfType<CinemachineFreeLook>();
            _rigComps = new CinemachineComposer[]{
                _cameraController.GetRig(0).GetCinemachineComponent<CinemachineComposer>(),
                _cameraController.GetRig(1).GetCinemachineComponent<CinemachineComposer>(),
                _cameraController.GetRig(2).GetCinemachineComponent<CinemachineComposer>(),
            };
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                // Compute a scale based on the score that should make the object 'full size'
                float scale = ScoringSystem.theScore / fullSizeScore + minSize;
                gameObject.transform.localScale = new Vector3(scale, scale, scale);

                // Smaller players move more slowly
                _playerMovement.movementSpeed = scale * _speedFactor;

                // Adjust the Cinemachine rig offsets so they are pointing at the
                // new center of the object.
                foreach (CinemachineComposer rigComp in _rigComps)
                {
                    rigComp.m_TrackedObjectOffset.y = scale / 2;
                }

                // Adjust rig heights and radius to properly restrict camera moves
                _cameraController.m_Orbits[0].m_Height = scale * 4f;
                _cameraController.m_Orbits[0].m_Radius = scale * 5f;
                _cameraController.m_Orbits[1].m_Height = scale * 2f;
                _cameraController.m_Orbits[1].m_Radius = scale * 4f;
                _cameraController.m_Orbits[2].m_Height = scale * 0.2f;
                _cameraController.m_Orbits[2].m_Radius = scale * 3f;
            }
        }
    }
}