using System;

namespace ApproximationByBezier.Utilities.Exceptions
{
    public class SlaeSolverException : ApplicationException
    {
        public double Epsilon { get; }
        
        public SlaeSolverException() { }
        public SlaeSolverException(double epsilon) : this(string.Empty, epsilon) { }
        public SlaeSolverException(string? message, double epsilon, Exception? inner = null) : base(message, inner)
        {
            Epsilon = epsilon;
        }
    }
}