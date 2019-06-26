using System;

namespace RaspPiTest.Middleware
{
    public class ErrorResult
    {
        public ErrorResult(int errorCode, string errorDescription)
        {
            Id = GetGuid(errorCode);
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public Guid Id { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        private static Guid GetGuid(int errorCode)
        {
            byte[] buffer = new byte[16];
            var value = DateTime.UtcNow.Ticks;

            buffer[7] = (byte)value;
            buffer[6] = (byte)(value >> 8);
            buffer[5] = (byte)(value >> 16);
            buffer[4] = (byte)(value >> 24);
            buffer[3] = (byte)(value >> 32);
            buffer[2] = (byte)(value >> 40);
            buffer[1] = (byte)(value >> 48);
            buffer[0] = (byte)(value >> 56);

            buffer[11] = (byte)errorCode;
            buffer[10] = (byte)(errorCode >> 8);
            buffer[09] = (byte)(errorCode >> 16);
            buffer[08] = (byte)(errorCode >> 24);

            
            var randomBytes = Guid.NewGuid().ToByteArray();
            buffer[12] = randomBytes[12];
            buffer[13] = randomBytes[13];
            buffer[14] = randomBytes[14];
            buffer[15] = randomBytes[15];

            return new Guid(buffer);
        }

        public static DateTime ParseTimestamp(ErrorResult result)
        {
            var dateBytes = result.Id.ToByteArray().AsSpan().Slice(0, 8);
            dateBytes.Reverse();
            return new DateTime(BitConverter.ToInt64(dateBytes.ToArray(), 0), DateTimeKind.Utc);
        }
    }
}