using MultiUserU6;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform VrTarget;
    public Transform IkTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map(bool active)
    {
        if(!active)
            return;
        IkTarget.position = VrTarget.TransformPoint(trackingPositionOffset);
        IkTarget.rotation = VrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    #region variables
    [SerializeField ,Range(0,1),Space(10)]
    private float _turnSmoothness = 0.1f;
    [Tooltip("Should the rig position follow the headset's?")]
    public bool IsPositionOverrideActive=true;
    [Tooltip("Should the head follow the headset?"),Space(10)]
    public bool IsHeadActive=true;
    [SerializeField]
    private VRMap _head;
    [Tooltip("Should the left hand follow the left controller?"),Space(10)]
    public bool IsLeftHandActive=true;
    [SerializeField]
    private VRMap _leftHand;
    [Tooltip("Should the right hand follow the right controller?"),Space(10)]
    public bool IsRightHandActive=true;
    [SerializeField]
    private VRMap _rightHand;
    public Vector3 _headBodyPositionOffset;
    #endregion variables
    //public float _headBodyYawOffset;
    void LateUpdate()
    {
        _head.Map(IsHeadActive);
        _leftHand.Map(IsLeftHandActive);
        _rightHand.Map(IsRightHandActive);

        if(!IsPositionOverrideActive)
            return;

        transform.position = _head.IkTarget.position + _headBodyPositionOffset;
        float yaw = _head.VrTarget.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z),_turnSmoothness);
    }

    #region methods

    public void SetPositionOverride(bool value){
        IsPositionOverrideActive=value;
    }

    #endregion methods
}
