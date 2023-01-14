namespace Proxima.SelfEdu.FiniteState;

public abstract class FiniteStateMachineException : Exception
{
    internal FiniteStateMachineException(string message) : base(message)
    {
    }
}