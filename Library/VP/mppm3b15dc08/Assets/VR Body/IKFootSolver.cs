using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    #region variables
    public bool isMovingForward;

    [SerializeField] LayerMask _terrainLayer = default;
    [SerializeField] Transform _body = default;
    [SerializeField] IKFootSolver _otherFoot = default;
    [SerializeField] float _speed = 4;
    [SerializeField] float _stepDistance = .2f;
    [SerializeField] float _stepLength = .2f;
    [SerializeField] float _sideStepLength = .1f;

    [SerializeField] float _stepHeight = .3f;
    [SerializeField] Vector3 _footOffset = default;

    public Vector3 FootRotOffset;
    public float FootYPosOffset = 0.1f;

    public float RayStartYOffset = 0;
    public float RayLength = 1.5f;
    
    float _footSpacing;
    Vector3 _oldPosition, _currentPosition, _newPosition;
    Vector3 _oldNormal, _currentNormal, _newNormal;
    float _lerp;
    #endregion variables

    private void Start()
    {
        _footSpacing = transform.localPosition.x;
        _currentPosition = _newPosition = _oldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = _currentPosition + Vector3.up * FootYPosOffset;
        transform.localRotation = Quaternion.Euler(FootRotOffset);

        Ray ray = new Ray(_body.position + (_body.right * _footSpacing) + Vector3.up * RayStartYOffset, Vector3.down);

        Debug.DrawRay(_body.position + (_body.right * _footSpacing) + Vector3.up * RayStartYOffset, Vector3.down);
            
        HandleMovement(ray);

        if (_lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
            tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

            _currentPosition = tempPosition;
            _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
            _lerp += Time.deltaTime * _speed;
        }
        else
        {
            _oldPosition = _newPosition;
            _oldNormal = _newNormal;
        }
    }

    private void HandleMovement(Ray ray){
        if (Physics.Raycast(ray, out RaycastHit info, RayLength, _terrainLayer.value))
        {
            if (Vector3.Distance(_newPosition, info.point) > _stepDistance && !_otherFoot.IsMoving() && _lerp >= 1)
            {
                _lerp = 0;
                Vector3 direction = Vector3.ProjectOnPlane(info.point - _currentPosition,Vector3.up).normalized;

                float angle = Vector3.Angle(_body.forward, _body.InverseTransformDirection(direction));

                isMovingForward = angle < 50 || angle > 130;

                if(isMovingForward)
                {
                    _newPosition = info.point + direction * _stepLength + _footOffset;
                    _newNormal = info.normal;
                }
                else
                {
                    _newPosition = info.point + direction * _sideStepLength + _footOffset;
                    _newNormal = info.normal;
                }

            }
        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_newPosition, 0.1f);
    }



    public bool IsMoving()
    {
        return _lerp < 1;
    }



}
