namespace ReGaSLZR.Gameplay.View
{
    using Base;
    using Model;

    using Cinemachine;
    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using Zenject;
    using System.Collections;

    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class CameraFocus : BaseReactiveMonoBehaviour
    {

        [SerializeField]
        [Required]
        private CinemachineVirtualCameraBase cam;

        [SerializeField]
        private float delayOnChangeFocus = 1f;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        protected override void RegisterObservables()
        {
            iSequenceGetter.GetActiveUnit()
                .Where(unitHolder => (unitHolder != null))
                .Subscribe(unitHolder => {
                    StopAllCoroutines();
                    StartCoroutine(CorChangeFocus(unitHolder.transform));
                })
                .AddTo(disposablesBasic);
        }

        private IEnumerator CorChangeFocus(Transform focus)
        {
            yield return new WaitForSeconds(delayOnChangeFocus);
            cam.Follow = focus;
            cam.LookAt = focus;
        }

    }

}