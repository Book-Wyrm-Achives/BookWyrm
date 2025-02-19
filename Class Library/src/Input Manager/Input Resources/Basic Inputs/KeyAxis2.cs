namespace BookWyrm.Input;
using Godot;

public partial class KeyAxis2 : InputResource<Vector2>
{
    [Export] Key UpKey;
    bool UpPressed = false;

    [Export] Key DownKey;
    bool DownPressed = false;

    [Export] Key RightKey;
    bool RightPressed = false;

    [Export] Key LeftKey;
    bool LeftPressed = false;

    public override Vector2 GetData() =>
        Vector2.Up * (UpPressed ? 1.0f : 0.0f) +
        Vector2.Down * (DownPressed ? 1.0f : 0.0f) +
        Vector2.Right * (RightPressed ? 1.0f : 0.0f) +
        Vector2.Left * (LeftPressed ? 1.0f : 0.0f);

    public override void UpdateInputData(InputEvent inputEvent)
    {
        if(inputEvent is InputEventKey keyEvent) {
            if(keyEvent.IsPressed()) {
                if(keyEvent.Keycode == UpKey) UpPressed         = true;
                if(keyEvent.Keycode == DownKey) DownPressed     = true;
                if(keyEvent.Keycode == RightKey) RightPressed   = true;
                if(keyEvent.Keycode == LeftKey) LeftPressed     = true;
            }

            if(keyEvent.IsReleased()) {
                if(keyEvent.Keycode == UpKey) UpPressed         = false;
                if(keyEvent.Keycode == DownKey) DownPressed     = false;
                if(keyEvent.Keycode == RightKey) RightPressed   = false;
                if(keyEvent.Keycode == LeftKey) LeftPressed     = false;
            }
        }
    }
}