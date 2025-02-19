namespace BookWyrm.Utility;
using System;
using Godot;

public partial class TimerNode : Node
{
    [Signal] public delegate void OnTimerCompleteEventHandler();

    /// <summary>
    /// The amount of time after reset before the <see cref="TimerNode.OnTimerComplete"/> event will fire.
    /// </summary>
    [Export] public float Duration;
    
    /// <summary>
    /// The current state of the timer. Counts down from <see cref="Duration"/> to 0.0.
    /// </summary>
    public float Countdown { get; private set;}

    /// <summary>
    /// The total time elapsed since the last call of <see cref="Reset"/>.
    /// </summary>
    public float TimeElapsed => Duration - Countdown;

    /// <summary>
    /// If true, the timer will trigger the <see cref="TimerNode.OnTimerComplete"/> event if <see cref="Complete"/> is true.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Determines if the countdown has been completed (i.e. <see cref="Countdown"/> less than 0.0)
    /// </summary>
    public bool Complete => Countdown <= 0.0f;

    /// <summary>
    /// If true, the timer will reset after <see cref="TimerNode.OnTimerComplete"/> is triggered.
    /// </summary>
    [Export] public bool ResetOnCompletion { get; private set; }

    /// <summary>
    /// If true, the timer will reset when <see cref="_Ready"/> is called.
    /// </summary>
    [Export] public bool ResetOnReady;
    
    /// <summary>
    /// If true, <see cref="Countdown"/> will not decrease when <see cref="_Process"/> is called.
    /// </summary>
    [Export] public bool PauseTimer;

    public override void _Ready() {
        Countdown = Duration;
        Active = false;

        if(ResetOnReady) {
            Reset();
        }
    }

    public override void _Process(double deltaTime)
    {
        if(!PauseTimer) Countdown -= (float) deltaTime;

        if(Active && Complete) {
            EmitSignal("OnTimerComplete");
            Active = false;

            if(ResetOnCompletion) {
                Reset();
            }
        }
    }

    /// <summary>
    /// Sets <see cref="Active"/> to true and <see cref="Countdown"/> to <see cref="Duration"/>
    /// </summary>
    public void Reset() {
        Active = true;
        Countdown = Duration;
    }
}