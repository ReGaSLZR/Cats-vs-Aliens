namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using System.Collections.Generic;
    using UniRx;

    public static class ITeam
    {

        public interface IEnemySetter
        {
            void AddUnit(Unit unit);
            void ClearUnits();
            void SetStatus(TeamStatus playerStatus);
        }

        public interface IEnemyGetter
        {
            IReadOnlyReactiveProperty<List<Unit>> GetUnits();
            IReadOnlyReactiveProperty<TeamStatus> GetStatus();
        }

        public interface PlayerSetter : IEnemySetter
        {
            void AddScore(int scoreToAdd);
        }

        public interface IPlayerGetter : IEnemyGetter 
        {
            IReadOnlyReactiveProperty<int> GetScore();
        }

    }

}