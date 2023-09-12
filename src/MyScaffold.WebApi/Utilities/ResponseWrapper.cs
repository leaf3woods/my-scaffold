using System.Data.SqlTypes;

namespace MyScaffold.WebApi.Utilities
{
    public class ResponseWrapper<TRead>
    {
        public string? Info { get; set; }
        public TRead? Payload { get; set; }
        public int Status { get; set; } = StatusCodes.Status200OK;

        public static ResponseWrapper<TRead> Create(TRead read, string? info = null, int status = StatusCodes.Status200OK)
        {
            return new ResponseWrapper<TRead>()
            {
                Info = info,
                Payload = read,
                Status = status
            };
        }
    }

    public static class ReadDtoWrapperExtension
    {
        public static ResponseWrapper<TRead?> Wrap<TRead>(this TRead? read,
            string? info = null, int status = StatusCodes.Status200OK) =>
            new()
            {
                Info = info,
                Payload = read,
                Status = status
            };

        public static ResponseWrapper<TNull?> WrapNull<TNull>(string? info = null,
            int status = StatusCodes.Status200OK) where TNull : INullable =>
            new()
            {
                Info = info,
                Payload = default(TNull),
                Status = status
            };

        public static ResponseWrapper<IEnumerable<TRead>> Wrap<TRead>(this IEnumerable<TRead> reads,
            string? info = null, int status = StatusCodes.Status200OK) =>
            new()
            {
                Info = info,
                Payload = reads,
                Status = status
            };
    }
}