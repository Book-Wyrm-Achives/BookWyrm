using Godot;
using BookWyrm.GodotExtensions;

namespace BookWyrm.StateMachine
{
    public partial class StatemachineNodeBranch<TData> : StateNode<TData>
    {
        StateMachine<string, (float, TData)> Machine = null;

        public override void _Ready()
        {
            Machine = new StateMachine<string, (float, TData)>(Name, ActiveOnInit(), this.GetChildrenOfType<StateNode<TData>>(false));
        }

        public override void Enter(float deltaTime, TData data) => Machine?.Enter((deltaTime, data));
        public override void Exit(float deltaTime, TData data) => Machine?.Exit((deltaTime, data));
        public override StateTransitionData<string> Update(float deltaTime, TData data) => Machine?.Update((deltaTime, data)) ?? StateNodeTransition.Continue();

    }
}