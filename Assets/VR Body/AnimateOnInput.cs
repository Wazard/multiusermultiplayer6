using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AnimationInput
{
    public string animationPropertyName;
    public InputActionProperty action;
}

public class AnimateOnInput : MonoBehaviour
{
    [SerializeField]
    private List<AnimationInput> _animationInputs;
    private Animator _animator;

    private void Start()
    {
        _animator??=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in _animationInputs)
        {
            float actionValue = item.action.action.ReadValue<float>();
            _animator.SetFloat(item.animationPropertyName, actionValue);
        }
    }
}
