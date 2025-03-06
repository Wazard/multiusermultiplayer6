using System;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using XRMultiplayer;

namespace MultiUserU6
{
    /// <summary>
    /// Physically accurate conveyor belt. Uses physics in order to move everything that touches it.
    /// </summary>
public class ConveyorBelt : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of which items on top are moved"), Range(0.01f,2)]
    private float _beltSpeed = .5f;
    [SerializeField,Tooltip("Conveyor belt status")]
    private bool _isActive = true;
    private Rigidbody _rigidBody;
    private NetworkManagerVRMultiplayer _networkManager;

    void OnEnable()
    {
        _networkManager = FindFirstObjectByType<NetworkManagerVRMultiplayer>();
    }
        void Start()
    {
        _networkManager.OnConnectionEvent += (NetworkManager, ConnectionEventData) => {_rigidBody.isKinematic=true;};
        _rigidBody ??= GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!_isActive)
            return;

        Vector3 currentPosition = _rigidBody.position;
        
        _rigidBody.position += -transform.forward * _beltSpeed * Time.fixedDeltaTime;
        _rigidBody.MovePosition(currentPosition);
    }

}
}