using System;

namespace ModdingToolkit.Core
{
    public class ExecutionException : Exception
    {
        public ExecutionException(ExitCodes code, string message) : base(message)
        {
            Code = code;
        }


        public ExitCodes Code { get; set; }
    }
}