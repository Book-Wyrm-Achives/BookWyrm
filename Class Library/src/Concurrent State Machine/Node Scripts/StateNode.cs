using System;
using Godot;

namespace BookWyrm.StateMachine
{
    public abstract partial class StateNode : Node, IState<string, float>
    {
        public string GetID() => Name;

        [Export] bool activeOnInit = false;
        public bool ActiveOnInit() => activeOnInit;

        public virtual void Enter(float deltaTime = 0.0f) { }

        public virtual void Exit(float deltaTime = 0.0f) { }

        public virtual StateTransitionData<string> Update(float deltaTime)
        {
            return StateNodeTransition.Continue();
        }
    }

    public static class StateNodeTransition
    {
        public static StateTransitionData<string> Continue() => StateTransitionData<string>.Continue();
        public static StateTransitionData<string> ExitTo(params Node[] enterNodes)
        {
            string[] enterStates = new string[enterNodes.Length];
            for (int i = 0; i < enterNodes.Length; i++)
            {
                enterStates[i] = enterNodes[i].Name;
            }

            return new StateTransitionData<string>(true, enterStates, Array.Empty<string>());
        }
    }
}