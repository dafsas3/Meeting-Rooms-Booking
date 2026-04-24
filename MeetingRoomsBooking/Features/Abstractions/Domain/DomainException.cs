using System.Collections.Generic;

namespace MeetingRoomsBooking.Features.Abstractions.Domain
{
    public abstract class DomainException : Exception
    {
        public string Code { get; }
        public int StatusCode { get; }

        public IReadOnlyDictionary<string, object?> Meta {  get; }

        protected DomainException(
            string code,
            string message,
            int statusCode,
            IDictionary<string, object?>? meta = null) : base(message)
        {
            Code = code;
            StatusCode = statusCode;

            Meta = meta is null
                ? new Dictionary<string, object?>()
                : new Dictionary<string, object?>(meta);
        }
    }
}
