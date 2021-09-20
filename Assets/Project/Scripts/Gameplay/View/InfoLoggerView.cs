namespace ReGaSLZR.Gameplay.View
{
    using Base;
    using Model;

    using NaughtyAttributes;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class InfoLoggerView : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private TextMeshProUGUI prefabEntry;

        [Header("UI")]

        [SerializeField]
        [Required]
        private ScrollRect scrollRect;

        #endregion

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        #region Unity Callbacks

        private void Awake()
        {
            var childCount = scrollRect.content.childCount;

            for (int x=childCount-1; x>=0; x--)
            {
                DestroyImmediate(scrollRect.content.GetChild(x).gameObject);
            }
        }

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            iLevelGetter.GetCurrentLog()
                .Where(log => !string.IsNullOrEmpty(log))
                .Subscribe(log => {
                    var entry = Instantiate(prefabEntry, scrollRect.content);
                    entry.text = log;
                    entry.transform.SetSiblingIndex(0);
                    scrollRect.normalizedPosition = new Vector2(0, 0);
                    scrollRect.verticalNormalizedPosition = 1f;
                })
                .AddTo(disposablesBasic);

        }

        #endregion

    }

}