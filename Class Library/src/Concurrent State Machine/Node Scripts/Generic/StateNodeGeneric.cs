using Godot;

namespace BookWyrm.StateMachine
{
    public abstract partial class StateNode<TData> : Node, IState<string, (float deltaTime, TData data)>
    {
        public string GetID() => Name;

        [Export] bool activeOnInit = false;
        public bool ActiveOnInit() => activeOnInit;

        public void Enter((float deltaTime, TData data) stateData) => Enter(stateData.deltaTime, stateData.data);
        public virtual void Enter(float deltaTime, TData data) { }

        public void Exit((float deltaTime, TData data) stateData) => Exit(stateData.deltaTime, stateData.data);
        public virtual void Exit(float deltaTime, TData data) { }

        public StateTransitionData<string> Update((float deltaTime, TData data) stateData) => Update(stateData.deltaTime, stateData.data);
        public virtual StateTransitionData<string> Update(float deltaTime, TData data)
        {
            return StateNodeTransition.Continue();
        }
    }
}