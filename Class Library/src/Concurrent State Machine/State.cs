using System;

namespace BookWyrm.StateMachine
{
    public abstract class State<TID, TData> : IState<TID, TData> where TID : notnull
    {
        public abstract TID GetID();
        public abstract bool ActiveOnInit();

        public virtual void Enter(TData data) { }

        public virtual void Exit(TData data) { }

        public virtual StateTransitionData<TID> Update(TData data)
        {
            return StateTransitionData<TID>.Continue();
        }
    }

    public interface IState<TID, TData> where TID : notnull
    {
        public TID GetID();
        public bool ActiveOnInit();

        public void Enter(TData data);
        public void Exit(TData data);
        public StateTransitionData<TID> Update(TData data);
    }

    public struct StateTransitionData<TID> where TID : notnull
    {
        public bool ExitSelf;
        public TID[] EnterStates;
        public TID[] ExitStates;

        public StateTransitionData(bool exitSelf, TID[] enterStates, TID[] exitStates)
        {
            ExitSelf = exitSelf;
            EnterStates = enterStates;
            ExitStates = exitStates;
        }

        public static StateTransitionData<TID> Continue() => new StateTransitionData<TID>(false, Array.Empty<TID>(), Array.Empty<TID>());
        public static StateTransitionData<TID> ExitTo(params TID[] enterStates) => new StateTransitionData<TID>(true, enterStates, Array.Empty<TID>());
    }
}