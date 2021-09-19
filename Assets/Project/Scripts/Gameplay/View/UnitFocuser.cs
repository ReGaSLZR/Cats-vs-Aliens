﻿namespace ReGaSLZR.Gameplay.View
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

        [SerializeField]
        [Required]
        private CinemachineVirtualCameraBase cam;

        [SerializeField]
        [Required]
        private SpriteRenderer unitHighlighter;

        [SerializeField]
        private float delayOnChangeFocus = 1f;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        private void Awake()
        {
            unitHighlighter.enabled = false;
        }

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
            unitHighlighter.enabled = true;
            unitHighlighter.transform.position = focus.position;

            yield return new WaitForSeconds(delayOnChangeFocus);

            cam.Follow = focus;
            cam.LookAt = focus;
        }

    }

}