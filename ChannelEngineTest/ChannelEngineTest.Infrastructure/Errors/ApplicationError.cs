using System;
using ChannelEngineTest.Infrastructure.Extensions;
using FluentResults;

namespace ChannelEngineTest.Infrastructure.Errors
{
    public class ApplicationError : Error
    {
        public const string AggregateError = nameof(AggregateError);
        public const string ErrorCodeKey = "ErrorCode";
        public const string PropertyNameKey = "PropertyName";

        protected ApplicationError()
        {
            Metadata[ErrorCodeKey] = GetType().Name;
        }

        public string ErrorCode => Metadata[ErrorCodeKey] as string ?? throw new InvalidOperationException();

        public string PropertyName
        {
            get => this.GetPropertyName();
            protected init => Metadata[PropertyNameKey] = value;
        }
    }
}