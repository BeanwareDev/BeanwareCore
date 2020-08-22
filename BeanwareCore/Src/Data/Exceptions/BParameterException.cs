using System;

namespace BeanwareCore.Src.Data.Exceptions
{
    /// <summary>
    /// Extended ArgumentException providing detailed information about the incorrect parameter value
    /// </summary>
    public sealed class BParameterException : Exception
    {
        // Variables
        public string ParameterName { get; private set; }
        public string MethodName { get; private set; }
        public string ClassName { get; private set; }
        public string ErrorMessage { get; private set; }

        // Constructors
        public BParameterException(string parameterName, string methodName, string className, string errorMessage) : base(
            $"Parameter \"{parameterName}\" of method \"{methodName}\" in \"{className}\": {errorMessage}"
        ){
            ParameterName = parameterName;
            MethodName = methodName;
            ClassName = className;
            ErrorMessage = errorMessage;
        }
    }
}
