namespace ReGaSLZR.Gameplay.Presenter.Spawner
{
    using Base;
    using Enum;
    using Model;
    using Presenter.Action;
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
        private readonly ITeam.IEnemyGetter iEnemyGetter;
        
        [Inject]
        private readonly ITeam.IPlayerGetter iPlayerGetter;

        [Inject]
        private readonly ITeam.IEnemySetter iEnemySetter;

        [Inject]
        private readonly ITeam.IPlayerSetter iPlayerSetter;

        [Inject]
        private readonly Base.IInstantiator iInstantiator;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

        [Inject]
        private readonly ThemeColors iThemeColors;

        [Inject]
        private readonly ITile.IGetter iTile;

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            iTile.IsReady()
                .Where(isReady => isReady)
                .Select(_ => iEnemyGetter.GetRawStartingUnits())
                .Subscribe(units => OnReceiveUnits(
                    Team.Enemy, units))
                .AddTo(disposablesBasic);

            iTile.IsReady()
                .Where(isReady => isReady)
                .Where(isFinished => isFinished)
                .Select(_ => iPlayerGetter.GetRawStartingUnits())
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
                //Safely check for missing spawn markers
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

                SpawnUnit(unit, team, tile);

                index++;

                if (index >= list.Count)
                {
                    break;
                }

                tile = list[index];
            }

            LogUtil.PrintInfo(GetType(), $"OnReceiveUnits(): done with {team}");
            if (team == Team.Enemy)
            {
                iEnemySetter.SetStatus(TeamStatus.InPlay);
                iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}" +
                    $">Enemy Team</color> ready.");
            }
            else 
            {
                iPlayerSetter.SetStatus(TeamStatus.InPlay);
                iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}" +
                    $">Player Team</color> ready.");
            }
        }

        private void SpawnUnit(Model.Unit unit, Team team, Tile tile)
        {
            var spawn = Instantiate(unit,
                        (team == Team.Player) ? parentUnitPlayer : parentUnitEnemies);
            spawn.Init(team);
            iSequenceSetter.AddUnitForSequence(spawn);
            SetUpSpawnedUnitMovement(spawn, team, tile);
            SetUpSpawnedUnitAction(spawn, team);
        }

        private void SetUpSpawnedUnitAction(Model.Unit spawn, Team team)
        {
            if (team == Team.Player)
            {
                spawn.gameObject.AddComponent<PlayerAction>();
            }
            else 
            {
                spawn.gameObject.AddComponent<AIAction>();
            }

            iInstantiator.InjectPrefab(spawn.gameObject);
        }

        private void SetUpSpawnedUnitMovement(Model.Unit spawn, Team team, Tile spawnTile)
        {
            BaseMovement movement;
            if (team == Team.Player)
            {
                movement = spawn.gameObject.AddComponent<PlayerMovement>();
            }
            else
            {
                movement = spawn.gameObject.AddComponent<AIMovement>();
            }

            iInstantiator.InjectPrefab(spawn.gameObject);
            movement.SetPosition(iTile.GetTileWithName(spawnTile.gameObject.name));
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