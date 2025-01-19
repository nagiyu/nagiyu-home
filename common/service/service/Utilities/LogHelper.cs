using System;

namespace Nagiyu.Common.Service.Utilities
{
    /// <summary>
    /// Log Helper
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void WriteLog(string message)
        {
            System.IO.File.AppendAllText("output.log", $"{DateTime.Now} {message}\n");
        }
    }
}
