namespace BookWyrm.Input;
using Godot;

public partial class KeyAxis : InputResource<float>
{
    [Export] Key PositiveKey;
    bool PositivePressed = false;

    [Export] Key NegativeKey;
    bool NegativePressed = false;

    public override float GetData() => (PositivePressed ? 1.0f : 0.0f) - (NegativePressed ? 1.0f : 0.0f);

    public override void UpdateInputData(InputEvent inputEvent)
    {
        if(inputEvent is InputEventKey keyEvent) {
            if(keyEvent.IsPressed()) {
                if(keyEvent.Keycode == PositiveKey) PositivePressed = true;
                if(keyEvent.Keycode == NegativeKey) NegativePressed = true;
            }

            if(keyEvent.IsReleased()) {
                if(keyEvent.Keycode == PositiveKey) PositivePressed = false;
                if(keyEvent.Keycode == NegativeKey) NegativePressed = false;
            }
        }
    }
}