# Proxima.SelfEdu.FiniteState

Simple generic Finite State Machine implementation for .NET.

Main implementation is `FiniteStateMachine`, that accepts three parameters:
* Instance of `FiniteStateMachineOptions` that describes the behaviour of the state machine during
configuration and operation, in particular, what situations should be considered exceptional and
which should be just ignored.
* Implementation of `IFiniteStateMachineEventHandler` that provides callbacks for machine to invoke,
for example when it achieves a particular state or applies a certain state-transition rule.
* Configuration action on `FiniteStateMachineBuilder` that should be used to define a set of states
and state-transition rules for the machine.

State-transition rules operate on messages sent to Handle method. They're differentiated by their runtime
type. Transitions can be cyclical. Transitions can be conditional based on the payload of a message.

Machine can contain zero or more states that are final. Upon achieving a final state, no further transitions
can happen. A machine without final states will accept new messages indefinitely. Machine
must have exactly one starting state.

Please refer to .Tests project for examples of setup or operation.