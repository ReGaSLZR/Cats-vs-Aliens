namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using UniRx;

    public class ILevel
    {
        public interface ISetter
        {
            void SetSelectedUnit(Unit unit);
            void SetLog(string log);
            void SetState(LevelState state);
        }

        public interface IGetter
        {
            IReadOnlyReactiveProperty<string> GetCurrentLog();
            IReadOnlyReactiveProperty<LevelState> GetState();
            IReadOnlyReactiveProperty<Unit> GetSelectedUnit();
        }
    }

}