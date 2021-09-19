namespace ReGaSLZR.Gameplay.View.Spawner
{
    using Base;
    using Enum;
    using Model;
    using Presenter.Movement;
    using Util;

    using NaughtyAttributes;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class UnitSpawner : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [Header("Parent GameObjects")]

        [SerializeField]
        [Required]
        private Transform parentUnitPlayer;

        [SerializeField]
        [Required]
        private Transform parentUnitEnemies;

        [Header("Tile Markers")]

        [SerializeField]
        private List<Tile> enemyTileMarker = new List<Tile>();

        [SerializeField]
        private List<Tile> playerTileMarker = new List<Tile>();

        #endregion

        #region Private Fields

        [Inject]
        private readonly ITeam.IEnemyGetter iEnemyTeam;
        
        [Inject]
        private readonly ITeam.IPlayerGetter iPlayerTeam;

        [Inject]
        private readonly Base.IInstantiator iInstantiator;

        [Inject]
        private readonly ThemeColors iThemeColors;

        [Inject]
        private readonly ITile.IGetter iTileGetter;

        [Inject]
        private readonly ICamera.ISetter iCameraSetter;

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            iEnemyTeam.GetUnits()
                .Subscribe(units => OnReceiveUnits(
                    Team.Enemy, units))
                .AddTo(disposablesBasic);

            iPlayerTeam.GetUnits()
                .Subscribe(units => OnReceiveUnits(
                    Team.Player, units))
                .AddTo(disposablesBasic);
        }

        #endregion

        #region Class Implementation

        private void OnReceiveUnits(Team team, List<Model.Unit> units)
        {
            int index = 0;
            var list = (team == Team.Player) ? playerTileMarker : enemyTileMarker;
            var tile = list[index];

            if (list.Count == 0)
            {
                return;
            }

            foreach (var unit in units)
            {
                while (tile == null)
                {
                    if (index >= list.Count)
                    {
                        LogUtil.PrintWarning(GetType(), $"OnReceiveUnits(): " +
                            $"All spawner tiles are null for Team {team}");
                        return;
                    }

                    index++;
                    tile = list[index];
                }
                
                var spawn = Instantiate(unit.Prefab, (team == Team.Player) ? parentUnitPlayer : parentUnitEnemies);
                spawn.SetBGColor(iThemeColors.GetBGColor(team));
                SetUpSpawnedUnitMovement(spawn, team, tile);
                iCameraSetter.SetFocusTarget(spawn.transform);

                index++;

                if (index >= list.Count)
                {
                    return;
                }

                tile = list[index];
            }

            LogUtil.PrintInfo(GetType(), $"OnReceiveUnits(): done with {team}");
        }

        private void SetUpSpawnedUnitMovement(UnitHolder spawn, Team team, Tile spawnTile)
        {
            BaseMovement movement = null;
            if (team == Team.Player)
            {
                movement = spawn.gameObject.AddComponent<PlayerMovement>();
            }
            else
            {
                movement = spawn.gameObject.AddComponent<AIMovement>();
            }

            iInstantiator.InjectPrefab(spawn.gameObject);
            movement.SetPosition(iTileGetter.GetTileAt(spawnTile.Position));
        }

        public void AddSpawnTile(Team team, Tile tile)
        {
            var list = (team == Team.Player) ? playerTileMarker : enemyTileMarker;    
            list.Add(tile);
        }

        public void ClearSpawnTiles()
        {
            playerTileMarker.Clear();
            enemyTileMarker.Clear();
        }

        #endregion

    }

}