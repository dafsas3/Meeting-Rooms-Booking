using System.Collections.Generic;

namespace MeetingRoomsBooking.Features.Abstractions.Domain
{
    public abstract class DomainException : Exception
    {
        public string Code { get; }
        public IReadOnlyDictionary<string, object?> Meta {  get; }

        protected DomainException(
            string code,
            string message,
            IDictionary<string, object?>? meta = null) : base(message)
        {
            Code = code;
            Meta = meta is null
                ? new Dictionary<string, object?>()
                : new Dictionary<string, object?>(meta);
        }
    }
}
