namespace ReGaSLZR.Gameplay.View
{
    using Base;
    using Model;

    using Cinemachine;
    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class CameraFocus : BaseReactiveMonoBehaviour
    {

        [SerializeField]
        [Required]
        private CinemachineVirtualCameraBase cam;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        protected override void RegisterObservables()
        {
            iSequenceGetter.GetSequencedUnit()
                .Where(unitHolder => (unitHolder != null))
                .Subscribe(unitHolder => {
                    cam.Follow = unitHolder.transform;
                    cam.LookAt = unitHolder.transform;
                })
                .AddTo(disposablesBasic);
        }

    }

}