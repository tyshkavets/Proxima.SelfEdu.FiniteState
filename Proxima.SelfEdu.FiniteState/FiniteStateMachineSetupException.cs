namespace Proxima.SelfEdu.FiniteState;

/// <summary>
/// All exceptions that occurred during machine setup.
/// </summary>
public class FiniteStateMachineSetupException : FiniteStateMachineException
{
    internal FiniteStateMachineSetupException(string message) : base(message)
    {
    }
}