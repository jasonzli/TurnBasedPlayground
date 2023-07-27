using System.Threading.Tasks;

namespace Code.BattleSystem
{
    // Interface for the basic action
    public interface IBattleAction
    {
        /// <summary>
        /// What this action does
        /// </summary>
        BattleActionParameters Parameters { get; }
        /// <summary>
        /// Who is doing it
        /// </summary>
        IBattleActor Source { get; }
        /// <summary>
        /// To whom
        /// </summary>
        IBattleActor Target { get; }
        /// <summary>
        /// Did it work
        /// </summary>
        /// <returns>True if yes</returns>
        bool Execute();
    }
}