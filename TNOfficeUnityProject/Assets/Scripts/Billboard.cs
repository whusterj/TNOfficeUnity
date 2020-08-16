using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TNOffice.Utilities
{
    public class Billboard : MonoBehaviour
    {
        private Transform mainCameraTransform;

        // Start is called before the first frame update
        void Start()
        {
            // Cache a reference to the main camera transform
            mainCameraTransform = Camera.main.transform;
        }

        // Use LateUpdate, so that this transform is applied last.
        void LateUpdate()
        {
            // Make the Billboard always face the camera
            transform.LookAt(
                transform.position + mainCameraTransform.rotation * Vector3.forward,
                mainCameraTransform.rotation * Vector3.up
            );
        }
    }
}