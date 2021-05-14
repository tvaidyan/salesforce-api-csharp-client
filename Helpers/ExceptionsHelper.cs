using System;
using System.Collections.Generic;
using System.Text;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Helpers
{
    public static class ExceptionsHelper
    {
        public static string GetExceptionDetailsAsString(Exception exception)
        {
            //get the details on this exception and any inner-exceptions
            var errMsg = new StringBuilder();
            errMsg.AppendLine("Exception: " + exception.Message);
            errMsg.AppendLine("Type:" + exception.GetType());
            errMsg.AppendLine("Source: " + exception.Source);
            errMsg.AppendLine("StackTrace: " + exception.StackTrace);
            errMsg.AppendLine("========================================");

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                errMsg.AppendLine("InnerException: " + exception.Message);
                errMsg.AppendLine("Type:" + exception.GetType());
                errMsg.AppendLine("InnerException Source: " + exception.Source);
                errMsg.AppendLine("InnerException StackTrace: " + exception.StackTrace);
                errMsg.AppendLine("****************************************");
            }

            return errMsg.ToString();
        }

        public static List<Exception> GetExceptionsAsList(Exception exception)
        {
            var exceptionsList = new List<Exception>
            {
                exception
            };

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                exceptionsList.Add(exception);
            }

            return exceptionsList;
        }
    }
}