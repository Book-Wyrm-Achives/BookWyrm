using System.Collections.Generic;
using BookWyrm.GodotExtensions;

namespace BookWyrm.StateMachine
{
    public partial class StateMachineNodeBranch : StateNode
    {
        StateMachine<string, float> Machine = null;

        public override void _Ready()
        {
            Machine = new StateMachine<string, float>(Name, ActiveOnInit(), this.GetChildrenOfType<StateNode>(false));
        }

        public override void Enter(float deltaTime = 0) => Machine?.Enter(deltaTime);
        public override void Exit(float deltaTime = 0) => Machine?.Exit(deltaTime);
        public override StateTransitionData<string> Update(float deltaTime) => Machine?.Update(deltaTime)??StateNodeTransition.Continue();
    }
}