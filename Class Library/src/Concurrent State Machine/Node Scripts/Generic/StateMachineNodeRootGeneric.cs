using Godot;
using BookWyrm.GodotExtensions;
using System;

namespace BookWyrm.StateMachine
{

    public abstract partial class StateMachineNodeRoot<TData> : Node
    {
        StateMachine<string, (float, TData)> Machine = null;

        enum UpdateModeTypes { PROCESS, PHYSICS_PROCESS }
        [Export] UpdateModeTypes UpdateMode = UpdateModeTypes.PROCESS;

        public abstract TData GetData();

        public override void _Ready()
        {
            Machine = new StateMachine<string, (float, TData)>(Name, true, this.GetChildrenOfType<StateNode<TData>>(false));
            Machine.Enter((0.0f, GetData()));
        }

        public override void _Process(double delta)
        {
            if (UpdateMode == UpdateModeTypes.PROCESS)
                Machine?.Update(((float)delta, GetData()));
        }

        public override void _PhysicsProcess(double delta)
        {
            if (UpdateMode == UpdateModeTypes.PHYSICS_PROCESS)
                Machine?.Update(((float)delta, GetData()));
        }
    }
}