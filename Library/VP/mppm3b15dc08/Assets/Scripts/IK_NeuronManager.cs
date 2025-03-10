using System.Threading.Tasks;
using Neuron;
using UnityEngine;

namespace MultiUserU6{
    /// <summary>
    /// Checks if perception neuron is active, decides automatically to enable IK or not
    /// </summary>
public class IK_NeuronManager : MonoBehaviour
{
    [SerializeField,Tooltip("GameObject where all the IK settings are stored")]
    private Transform _characterInverseKinematicTransform;
    [SerializeField]
    private IKTargetFollowVRRig iKTargetFollowVRRig;
    [SerializeField,Tooltip("Character's neuron transforms instance")]
    private NeuronInstance neuronInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNeuronInstance();
    }

    private async void SetNeuronInstance(){
        int timeAwaited = 0;
        while(NeuronSourceManager.Instance == null && timeAwaited<2000){
            await Task.Delay(200);
            timeAwaited+=200;
        };

        if(timeAwaited>=2000){
            return;
        }
        NeuronSourceManager.Instance.NeuronConnectionStateUpdate += ToggleIK;
    }

    private void ToggleIK(bool value){
        _characterInverseKinematicTransform.gameObject.SetActive(!value);
        iKTargetFollowVRRig.SetPositionOverride(!value);

        neuronInstance.enabled = value;
    }
}
}