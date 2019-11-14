using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game5;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.Env;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;

namespace GameTest.States
{
    [State("init")]
    public class InitializationState : GameState
    {
        private Task currentTask;
        private int index;
        private List<Action> loadingActions;

        public override void Cleanup()
        {
        }

        public override void DrawWorld()
        {
        }

        public override string GetName()
        {
            return "init";
        }

        public override void Init()
        {
            ServiceLocator.Get<IResourceService>().LoadFont("fonts/consolas", "consolas");

            index = 0;
            loadingActions = new List<Action>
            {
                () => { ServiceLocator.Get<IResourceService>().Load(@"Content\json\resources.json"); },
                () => { ServiceLocator.Get<ITileRepository>().RegisterAllTiles(); },
                () => { },
                () => { RegisterStates(); }
            };
        }

        public override void PostAddonInit()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if ((currentTask == null || currentTask.IsCompleted) && index < loadingActions.Count)
            {
                currentTask = Task.Run(loadingActions[index]);
                index++;
            }

            if (index >= loadingActions.Count && currentTask.IsCompleted)
                StateService.PushState(new MenuState(), false);
        }
    }
}