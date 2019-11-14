using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game5.Data.Helper
{
    public class ThreadHelper
    {
        public static void SpawnAndWait(List<Action> actions)
        {
            var handles = new Task[actions.Count];
            for (var i = 0; i < actions.Count; i++)
            {
                var task = Task.Factory.StartNew(actions[i]);
                handles[i] = task;
            }

            Task.WaitAll(handles);
        }
    }
}