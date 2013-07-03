using System;

namespace JR.Core.Interfaces
{
    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }
}