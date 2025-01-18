using System;

namespace Nagiyu.Splatoon3Tracker.Service.Exceptions
{
    /// <summary>
    /// DB 例外
    /// </summary>
    public class ParameterException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ParameterException(string message) : base(message)
        {
        }
    }
}
