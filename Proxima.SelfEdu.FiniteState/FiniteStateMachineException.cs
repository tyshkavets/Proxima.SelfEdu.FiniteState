namespace Proxima.SelfEdu.FiniteState;

/// <summary>
/// Base class used by all custom exceptions from finite state machine.
/// </summary>
public abstract class FiniteStateMachineException : Exception
{
    internal FiniteStateMachineException(string message) : base(message)
    {
    }
}