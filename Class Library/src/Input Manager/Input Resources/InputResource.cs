namespace BookWyrm.Input;
using Godot;

/// <summary>
/// A general storage type for all inputs in the Input Manager
/// </summary>
public abstract partial class InputResource : Resource
{
    [Export] public string Name = "";

    /// <summary>
    /// Updates the input's data based on the information provided by an input event.
    /// </summary>
    /// <param name="inputEvent">The input event passed from the InputManager's _Input(inputEvent) method.</param>
    public abstract void UpdateInputData(InputEvent inputEvent);
}

/// <summary>
/// A type-specific storage type of inputs in the InputManager
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract partial class InputResource<T> : InputResource {

    /// <summary>
    /// Retrieves the current state of the input data
    /// </summary>
    public abstract T GetData();

    public static implicit operator T(InputResource<T> input) => input.GetData();
}