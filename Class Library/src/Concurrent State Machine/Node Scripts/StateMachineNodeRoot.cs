using Godot;
using BookWyrm.GodotExtensions;

namespace BookWyrm.StateMachine
{
    public partial class StateMachineNodeRoot : Node
    {
        StateMachine<string, float> Machine = null;
        
        enum UpdateModeTypes { PROCESS, PHYSICS_PROCESS }
        [Export] UpdateModeTypes UpdateMode = UpdateModeTypes.PROCESS;

        public override void _Ready()
        {
            Machine = new StateMachine<string, float>(Name, true, this.GetChildrenOfType<StateNode>(false));
            Machine.Enter(0.0f);
        }

        public override void _Process(double delta)
        {
            if(UpdateMode == UpdateModeTypes.PROCESS)
                Machine?.Update((float)delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            if(UpdateMode == UpdateModeTypes.PHYSICS_PROCESS)
                Machine?.Update((float)delta);
        }
    }
}