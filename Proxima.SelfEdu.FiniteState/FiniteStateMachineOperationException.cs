namespace Proxima.SelfEdu.FiniteState;

/// <summary>
/// All exceptions that occured during machine operation.
/// </summary>
public class FiniteStateMachineOperationException : FiniteStateMachineException
{
    public FiniteStateMachineOperationException(string message) : base(message)
    {
    }
}