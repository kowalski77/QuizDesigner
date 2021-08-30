using System;
using System.Diagnostics.CodeAnalysis;

[assembly:CLSCompliant(false)]

[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "The interface is used to identify a set of types at compile time", Scope = "type", Target = "~T:QuizDesigner.Application.IntegrationEvents.IIntegrationEvent")]