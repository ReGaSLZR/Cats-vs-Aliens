namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using System.Collections.Generic;
    using UniRx;

    public static class ITeams
    {

        public interface ISetter
        {
            void AddUnit(Unit unit);
            void ClearUnits();
            void SetStatus(TeamStatus playerStatus);
        }

        public interface IGetter
        {
            IReadOnlyReactiveProperty<List<Unit>> GetUnits();
            IReadOnlyReactiveProperty<TeamStatus> GetStatus();
        }

        public interface PlayerSetter : ISetter
        {
            void AddScore(int scoreToAdd);
        }

        public interface IPlayerGetter : IGetter 
        {
            IReadOnlyReactiveProperty<int> GetScore();
        }

    }

}