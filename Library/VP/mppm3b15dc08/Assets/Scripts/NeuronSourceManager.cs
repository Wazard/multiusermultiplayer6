using System;
using System.Threading.Tasks;
using Neuron;
using UnityEngine;

namespace MultiUserU6{
    /// <summary>
    /// NeuronSourceManager with connection check
    /// </summary>
    public class NeuronSourceManager : Neuron.NeuronSourceManager
    {
        #region variables
        
        [SerializeField,Tooltip("Current Character's NeuronInstanceTransform"), Space(10)]
        private NeuronInstance neuronInstance;
        public Action<bool> NeuronConnectionStateUpdate;
        public static NeuronSourceManager Instance;

        #endregion variables

        #region methods
        private void Awake()
        {
            Instance ??= this;

            if(Instance!=this)
                Destroy(gameObject);
        }
        private void Start()
        {
            neuronInstance??=GetComponentInChildren<NeuronTransformsInstance>();
            ToggleConnect();
            DisableIKOnConnection();
        }
        public bool IsConnectionActive(){
            return neuronInstance.boundActor.AvatarName!=null;
        }

        private async void DisableIKOnConnection(){
            int timeAwaited = 0;
            do{
                await Task.Delay(500);
                timeAwaited+=500;
            }while(!hasConnected || timeAwaited<2500);

            //NeuronConnectionStateUpdate.Invoke(IsConnectionActive());
            NeuronConnectionStateUpdate.Invoke(true);
        }
        #endregion methods
    }
};