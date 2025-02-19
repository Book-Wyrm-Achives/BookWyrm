namespace BookWyrm.Input;
using Godot;
using System;
using System.Collections.Generic;


public partial class InputManager : Node
{
    /// <summary>
    /// A path to where the default Input Scheme is stored. Used to retrieve saved control schemes.
    /// </summary>
    private const string INPUT_SCHEME_PATH = "res://Test Axes/";
    private static InputManager? Instance;

    private Dictionary<string, InputResource> ActiveInputs = new Dictionary<string, InputResource>();

    public override void _Ready()
    {
        if (Instance == null) Instance = this;
        else QueueFree();

        ActiveInputs = new Dictionary<string, InputResource>();
        LoadInputResourcesFromFolder(INPUT_SCHEME_PATH);
    }

    public override void _Input(InputEvent inputEvent)
    {
        foreach (var input in ActiveInputs.Values) input.UpdateInputData(inputEvent);
    }

    /// <summary>
    /// Retrieves the current state of data from an input with a specified name.
    /// </summary>
    /// <typeparam name="T">The type of data to be retrieved</typeparam>
    /// <param name="name">The name of the InputResource being retrieved</param>
    /// <returns>The current value of the InputResource</returns>
    /// <exception cref="ArrayTypeMismatchException">Throws this exception if type <typeparamref name="T"/> does not match specified input</exception>
    /// <exception cref="IndexOutOfRangeException">Throws this exception if name is not found in <see cref="ActiveInputs"/></exception>
    public static T GetInputData<T>(string name)
    {
        if (Instance.ActiveInputs.ContainsKey(name))
        {
            if (Instance.ActiveInputs[name] is InputResource<T> tInput) return tInput.GetData();
            else
            {
                throw new ArrayTypeMismatchException($"Type {typeof(T)} does not match input named {name}.");
            }
        }

        throw new IndexOutOfRangeException($"Input named \"{name}\" not found in ActiveInputs.");
    }

    /// <summary>
    /// Register an InputResource with the InputManager singleton.
    /// </summary>
    /// <param name="input">A resource containing the desired input</param>
    /// <returns>True if resource was successfully added to the InputManager, otherwise false</returns>
    public static bool RegisterInput(InputResource input)
    {
        if (Instance == null) return false;
        if (Instance.ActiveInputs.ContainsKey(input.Name)) return false;

        Instance.ActiveInputs.Add(input.Name, input);
        return true;
    }

    /// <summary>
    /// Remove an InputResource from the InputManager singleton.
    /// </summary>
    /// <param name="inputName">The name of the input to be removed</param>
    /// <returns>True if resource was successfully removed from the InputManager, otherwise false</returns>
    public static bool DeregisterInput(string inputName)
    {
        if (Instance == null) return false;
        if (!Instance.ActiveInputs.ContainsKey(inputName)) return false;

        Instance.ActiveInputs.Remove(inputName);
        return true;
    }

    public void LoadInputResourcesFromFolder(string folderPath) {
        DirAccess dirAccess = DirAccess.Open(folderPath);

        string[] filePaths = dirAccess.GetFiles();
        if(filePaths == null) return;

        foreach(var filePath in filePaths) {
            InputResource input = GD.Load<InputResource>(folderPath + filePath);
            if(input == null) continue;

            RegisterInput(input);
        }
    }
}