using System;
using UnityEngine;

namespace MultiUserU6
{
    /// <summary>
    /// Physically accurate conveyor belt. Uses physics in order to move everything that touches it.
    /// </summary>
    public class ConveyorPhysics : MonoBehaviour
    {
        [SerializeField, Tooltip("Speed of which items on top are moved"), Range(0.01f, 2)]
        private float _beltSpeed = .5f;
        [SerializeField, Tooltip("Conveyor belt status")]
        private bool _isActive = true;
        
        private void OnTriggerStay(Collider other)
        {
            if (!_isActive)
                return;

            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                Vector3 force = transform.forward * _beltSpeed;
                otherRigidbody.linearVelocity = force;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the touching object has a Rigidbody
            if (other.attachedRigidbody != null)
            {
                // You could add any initialization logic for new touching objects here
            }
        }
    }
}
