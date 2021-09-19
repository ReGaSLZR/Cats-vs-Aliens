﻿namespace ReGaSLZR.Gameplay.Presenter.Spawner
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
        private readonly ThemeColors iThemeColors;

        [Inject]
        private readonly ITile.IGetter iTile;

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            iEnemyGetter.GetIsInitFinished()
                .Where(isFinished => isFinished)
                .Select(_ => iEnemyGetter.GetUnits().Value)
                .Subscribe(units => OnReceiveUnits(
                    Team.Enemy, units))
                .AddTo(disposablesBasic);

            iPlayerGetter.GetIsInitFinished()
                .Where(isFinished => isFinished)
                .Select(_ => iPlayerGetter.GetUnits().Value)
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
                
                var spawn = Instantiate(unit, 
                    (team == Team.Player) ? parentUnitPlayer : parentUnitEnemies);
                spawn.SetBGColor(iThemeColors.GetBGColor(team));
                spawn.Data.Init();
                iSequenceSetter.AddUnitForSequence(spawn);
                SetUpSpawnedUnitMovement(spawn, team, tile);

                index++;

                if (index >= list.Count)
                {
                    return;
                }

                tile = list[index];
            }

            LogUtil.PrintInfo(GetType(), $"OnReceiveUnits(): done with {team}");
            if (team == Team.Enemy)
            {
                iEnemySetter.SetStatus(TeamStatus.InPlay);
            }
            else 
            {
                iPlayerSetter.SetStatus(TeamStatus.InPlay);
            }
        }

        private void SetUpSpawnedUnitMovement(Model.Unit spawn, Team team, Tile spawnTile)
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
            movement.SetPosition(iTile.GetTileAt(spawnTile.Position));
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