using System.Threading.Tasks;

namespace Code.BattleSystem
{
    public interface IBattleAction
    {
        BattleActionParameters Parameters { get; }
        IBattleActor Source { get; }
        IBattleActor Target { get; }
        Task<bool> Execute();
    }
}