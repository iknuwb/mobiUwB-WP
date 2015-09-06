#region

using System.Collections.Generic;
using SharedCode.Tasks.Models;

#endregion

namespace SharedCode.Tasks
{
    /// <summary>
    /// Zarządza kolejnością wykonywanych procesów.
    /// </summary>
    /// <typeparam name="Out">Typ parametru wyjściowego</typeparam>
    public class TasksQueue<Out> where Out : TaskOutput
    {
        /// <summary>
        /// Lista wszystkich zadań do wykonania.
        /// </summary>
        private readonly List<TaskPair> tasks;

        public TasksQueue()
        {
            tasks = new List<TaskPair>();
        }

        /// <summary>
        /// Dodaje nowy element do kolejki.
        /// </summary>
        /// <param name="task">Element implementujący ITask</param>
        /// <param name="taskInput">Dane wejściowe</param>
        public void add(ITask<Out> task, TaskInput taskInput)
        {
            tasks.Add(new TaskPair(task,taskInput));
        }

        /// <summary>
        /// Wykonuje wszystkie zadania.
        /// </summary>
        /// <param name="outputToFill">Obiekt wynikowy modyfikowany przez wszystkie zadania.</param>
        public void performAll(Out outputToFill)
        {
            foreach (TaskPair tuple in tasks)
            {
                if (outputToFill.Succeeded)
                {
                    ITask<Out> task = tuple.task;
                    TaskInput input = tuple.taskInput;
                    task.Execute(input, outputToFill);
                }
            }
        }

        /// <summary>
        /// Klasa pomocnicza opakowująca.
        /// </summary>
        private class TaskPair
        {
            /// <summary>
            /// Nadaje lub pobiera zadanie do wykonania.
            /// </summary>
            public ITask<Out> task { get; private set; }
            /// <summary>
            /// Nadaje lub pobiera dane wejściowe do zadania.
            /// </summary>
            public TaskInput taskInput { get; private set; }

            public TaskPair(ITask<Out> task, TaskInput taskInput)
            {
                this.task = task;
                this.taskInput = taskInput;
            }
        }
    }
}
