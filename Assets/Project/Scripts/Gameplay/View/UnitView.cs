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

    [RequireComponent(typeof(Model.Unit))]
    public class UnitView : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private Image background;

        [SerializeField]
        [Required]
        private RawImage visual;

        [Header("UI")]

        [SerializeField]
        [Required]
        private TextMeshProUGUI textDisplayName;

        [SerializeField]
        [Required]
        private Slider sliderHp;

        [SerializeField]
        [Required]
        private Slider sliderAttack;

        [SerializeField]
        [Required]
        private Slider sliderSpeed;

        #endregion

        #region Private Fields

        [SerializeField]
        [ReadOnly]
        private Model.Unit unit;

        [Inject]
        private readonly ThemeColors iThemeColor;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            unit = GetComponent<Model.Unit>();   
        }

        private void Start()
        {
            unit.GetOnInit()
                .Where(isInit => isInit)
                .Subscribe(_ => Init())
                .AddTo(disposablesTerminal);
        }

        #endregion

        #region Class Implementation

        private void Init()
        {
            var data = unit.Data;
            background.color = iThemeColor.GetBGColor(data.Team);
            textDisplayName.text = data.DisplayName;
            visual.texture = data.Icon;
            sliderHp.maxValue = UnitData.MAX_HP;
            sliderAttack.maxValue = UnitData.MAX_ATTACK;
            sliderSpeed.maxValue = UnitData.MAX_SPEED;

            sliderAttack.value = data.StatAttack;
            sliderSpeed.value = data.StatSpeed;

            data.GetCurrentHp()
                .Subscribe(hp => sliderHp.value = hp)
                .AddTo(disposablesTerminal);
        }

        #endregion

    }

}