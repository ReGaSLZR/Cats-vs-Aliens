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

    public class UnitFocuser : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private CinemachineVirtualCameraBase cam;

        [SerializeField]
        [Required]
        private SpriteRenderer unitHighlighter;

        [SerializeField]
        private float delayOnChangeFocus = 1f;

        #endregion

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        #region Unity Callbacks

        private void Awake()
        {
            unitHighlighter.enabled = false;
        }

        #endregion

        #region Class Overrides

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

        #endregion

        #region Class Implementation

        private IEnumerator CorChangeFocus(Transform focus)
        {
            unitHighlighter.enabled = true;
            unitHighlighter.transform.SetParent(focus);
            unitHighlighter.transform.position = focus.position;

            yield return new WaitForSeconds(delayOnChangeFocus);

            cam.Follow = focus;
            cam.LookAt = focus;
        }

        #endregion

    }

}