namespace ReGaSLZR.Gameplay.Model
{

    using Base;
    using Enum;

    using NaughtyAttributes;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UniRx;

    public class Unit : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        private UnitData data;

        [Space]

        [SerializeField]
        [Required]
        private SpriteRenderer background;

        [SerializeField]
        [Required]
        private SpriteRenderer visual;

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

        #region Accessor

        public UnitData Data => data;

        #endregion

        #region Class Implementation

        public void Init(Color colorBG, Team team)
        {
            background.color = colorBG;
            data.Init(team);

            textDisplayName.text = data.DisplayName;
            visual.sprite = data.Icon;
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