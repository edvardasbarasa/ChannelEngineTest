using System;
using System.Collections.Generic;
using System.Linq;
using ChannelEngineTest.Infrastructure.Errors;
using FluentResults;

namespace ChannelEngineTest.Infrastructure.Extensions
{
    public static class FluentResultExtensions
    {
        public static string GetPropertyName(this IError error)
        {
            return error.Metadata.GetValueOrDefault(ApplicationError.PropertyNameKey) as string;
        }

        public static IEnumerable<IError> FlattenErrors(this IEnumerable<IError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            return errors.SelectMany(Flatten);
        }

        private static IEnumerable<IError> Flatten(this IError error)
        {
            yield return error;
            foreach (var reason in error.Reasons.SelectMany(Flatten))
            {
                yield return reason;
            }
        }
    }
}