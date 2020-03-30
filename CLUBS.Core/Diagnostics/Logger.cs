using System;
using System.Collections.Generic;
using System.Text;

namespace CLUBS.Core.Diagnostics
{
    public interface ILogger
    {
        void Log(string msg);
        void Log(string msg,LogLevel loglevel);
    }
    public enum LogLevel
    {
        Development,Normal,Warning,Error
    }
    public static class Logger
    {
        public static ILogger CurrentLogger = new ConsoleLogger();
    }
    public class ConsoleLogger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Log(string msg, LogLevel loglevel)
        {
            switch (loglevel)
            {
                case LogLevel.Development:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
