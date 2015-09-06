#region

using SharedCode.Tasks.Models;

#endregion

namespace SharedCode.Tasks
{
    /// <summary>
    /// Interfejs wykorzystywany w klasie TaskQueue
    /// </summary>
    /// <typeparam name="Out">Typ wartości wyjściowej</typeparam>
    public interface ITask<Out> where Out : TaskOutput
    {
        void Execute(TaskInput input, Out output);
    }
}
