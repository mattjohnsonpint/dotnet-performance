// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class LoggingBenchmarkBase
    {
        protected static readonly Action<ILogger, Exception> NoArgumentTraceMessage = LoggerMessage.Define(LogLevel.Trace, 0, "Message");
        protected static readonly Action<ILogger, Exception> NoArgumentErrorMessage = LoggerMessage.Define(LogLevel.Error, 0, "Message");

        protected static readonly Action<ILogger, int, string, Exception> TwoArgumentTraceMessage = LoggerMessage.Define<int, string>(LogLevel.Trace, 0, "Message {Argument1} {Argument2}");
        protected static readonly Action<ILogger, int, string, Exception> TwoArgumentErrorMessage = LoggerMessage.Define<int, string>(LogLevel.Error, 0, "Message {Argument1} {Argument2}");

        protected static Exception Exception = GetRealException();
        
        private static Exception GetRealException()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public class SampleScope : IEnumerable<KeyValuePair<string, object>>
        {
            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                yield return new KeyValuePair<string, object>("A", 1);
                yield return new KeyValuePair<string, object>("B", "2");
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NoopLogger : ILogger
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }

        public class LoggerProvider<T>: ILoggerProvider
            where T: ILogger, new()
        {
            public void Dispose()
            {
            }

            public ILogger CreateLogger(string categoryName)
            {
                return new T();
            }
        }
    }
}
