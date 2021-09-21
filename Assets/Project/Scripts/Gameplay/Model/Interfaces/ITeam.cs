namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using System.Collections.Generic;
    using UniRx;

    public static class ITeam
    {

        public interface IEnemySetter
        {
            void SetStatus(TeamStatus playerStatus);
        }

        public interface IEnemyGetter
        {
            List<Unit> GetRawStartingUnits();
            IReadOnlyReactiveProperty<TeamStatus> GetStatus();
        }

        public interface IPlayerSetter : IEnemySetter
        {
            void AddScore(int scoreToAdd);
        }

        public interface IPlayerGetter : IEnemyGetter 
        {
            IReadOnlyReactiveProperty<int> GetScore();
        }

    }

}