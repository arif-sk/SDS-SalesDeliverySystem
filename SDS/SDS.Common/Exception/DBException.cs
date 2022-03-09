using System;

namespace Caretaskr.Common.Exceptions
{
    public class DBException : Exception
    {
        public DBException(Exception ex)
            : base("Error Occured while Saving",ex)
        {
        }
    }
}