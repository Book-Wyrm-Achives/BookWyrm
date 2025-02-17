namespace BookWyrm.Utility;
using System;

/// <summary>
/// A basic timer implementation. Can be updated to trigger an event after an amount of time is passed.
/// Use the <see cref="Update"/> method to update the <see cref="Countdown"/>.
/// </summary>
public class Timer
{

    /// <summary>
    /// The amount of time after reset before the <see cref="OnTimerComplete"/> event will fire.
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    /// The total time elapsed since the last call of <see cref="Reset"/>.
    /// </summary>
    public float TimeElapsed => Duration - Countdown;

    /// <summary>
    /// The current state of the timer. Counts down from <see cref="Duration"/> to 0.0.
    /// </summary>
    public float Countdown { get; private set; }

    /// <summary>
    /// If true, the timer will trigger the <see cref="OnTimerComplete"/> event if <see cref="Complete"/> is true.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Determines if the countdown has been completed (i.e. <see cref="Countdown"/> less than 0.0)
    /// </summary>
    public bool Complete => Countdown <= 0.0f;

    /// <summary>
    /// If true, <see cref="Countdown"/> will not decrease when <see cref="Update"/> is called.
    /// </summary>
    public bool PauseTimer;

    /// <summary>
    /// If true, the timer will reset after <see cref="OnTimerComplete"/> is triggered.
    /// </summary>
    public bool ResetOnCompletion;

    /// <summary>
    /// Called once the timer is <see cref="Active"/> and <see cref="Complete"/>.
    /// </summary>
    public event Action? OnTimerComplete;


    /// <summary>
    /// A basic timer implementation. Can be updated to trigger an event after an amount of time is passed.
    /// Use the <see cref="Update"/> method to update the <see cref="Countdown"/>.
    /// </summary>
    /// <param name="duration"> The amount of time after reset before the <see cref="OnTimerComplete"/> event will fire.</param>
    /// <param name="activateOnCreation">If true, the <see cref="Reset"/> method will be immediately fired on construction.</param>
    public Timer(float duration, bool activateOnCreation = false)
    {
        Duration = duration;
        Countdown = Duration;
        Active = false;

        if (activateOnCreation) Reset();
    }

    /// <summary>
    /// Updates <see cref="Countdown"/> by <paramref name="deltaTime"/> and triggers <see cref="OnTimerComplete"/> if <see cref="Active"/> and <see cref="Complete"/> are both true.
    /// </summary>
    /// <param name="deltaTime">The amount of time to be subtracted from <see cref="Countdown"/></param>
    public void Update(float deltaTime)
    {
        if(!PauseTimer) Countdown -= deltaTime;

        if (Active && Complete)
        {
            OnTimerComplete?.Invoke();
            Active = false;

            if (ResetOnCompletion)
            {
                Reset();
            }
        }
    }

    /// <summary>
    /// Sets <see cref="Active"/> to true and <see cref="Countdown"/> to <see cref="Duration"/>
    /// </summary>
    public void Reset()
    {
        Active = true;
        Countdown = Duration;
    }
}
