using System.Collections.Generic;

namespace BookWyrm.StateMachine
{
    public class StateMachine<TID, TData> : State<TID, TData> where TID : notnull
    {
        Dictionary<TID, IState<TID, TData>> States;
        HashSet<TID> ActiveStates;
        Queue<TID> ExitQueue;
        Queue<TID> EnterQueue;

        TID idRef;
        public override TID GetID() => idRef;

        bool activeOnInitRef;
        public override bool ActiveOnInit() => activeOnInitRef;

        public StateMachine(TID id, bool activeOnInit, IState<TID, TData>[] states)
        {
            idRef = id;
            activeOnInitRef = activeOnInit;

            ActiveStates = new HashSet<TID>();
            EnterQueue = new Queue<TID>();
            ExitQueue = new Queue<TID>();

            States = new Dictionary<TID, IState<TID, TData>>();
            foreach (var state in states)
            {
                States.Add(state.GetID(), state);
            }
        }

        
        public override void Enter(TData data)
        {
            foreach (var state in States.Values)
            {
                if (state.ActiveOnInit() && ActiveStates.Add(state.GetID()))
                {
                    state.Enter(data);
                }
            }
        }

        public override void Exit(TData data)
        {
            foreach (var state in States.Values)
            {
                if (ActiveStates.Remove(state.GetID()))
                {
                    state.Exit(data);
                }
            }
        }

        public override StateTransitionData<TID> Update(TData data)
        {
            foreach(var activeID in ActiveStates) {
                var state = States[activeID];

                var transitionData = state.Update(data);

                if(transitionData.ExitSelf) {
                    ExitQueue.Enqueue(activeID);
                }

                foreach(var exitID in transitionData.ExitStates) {
                    ExitQueue.Enqueue(exitID);
                }

                foreach(var enterID in transitionData.EnterStates) {
                    EnterQueue.Enqueue(enterID);
                }
            }

            while(ExitQueue.TryDequeue(out var exitID)) {
                if(ActiveStates.Remove(exitID)) {
                    States[exitID].Exit(data);
                }
            }

            while(EnterQueue.TryDequeue(out var enterID)) {
                if(ActiveStates.Add(enterID)) {
                    States[enterID].Enter(data);
                }
            }

            return StateTransitionData<TID>.Continue();
        }
    }
}