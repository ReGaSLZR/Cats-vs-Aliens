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

        [SerializeField]
        [Required]
        private TextMeshProUGUI prefabEntry;

        [Header("UI")]

        [SerializeField]
        [Required]
        private ScrollRect scrollRect;

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        private void Awake()
        {
            var childCount = scrollRect.content.childCount;

            for (int x=childCount-1; x>=0; x--)
            {
                DestroyImmediate(scrollRect.content.GetChild(x).gameObject);
            }
        }

        protected override void RegisterObservables()
        {
            iLevelGetter.GetCurrentLog()
                .Where(log => !string.IsNullOrEmpty(log))
                .Subscribe(log => {
                    var entry = Instantiate(prefabEntry, scrollRect.content);
                    entry.text = log;
                    scrollRect.normalizedPosition = new Vector2(0, 0);
                    scrollRect.verticalNormalizedPosition = 0f;
                })
                .AddTo(disposablesBasic);

        }

    }

}