using System;
using System.Collections.Generic;
using System.Linq;

using static System.BitConverter;
using static System.Text.Encoding;

using Package =
    System.Collections.Generic.Dictionary<
        string,
        System.Collections.Generic.Dictionary<string, string>>;

namespace IAS04110
{
    public class Parser
    {
        private readonly UnitResolver _resolver;

        public Parser(UnitResolver resolver)
        {
            _resolver = resolver;
        }

        public static string FromUnicodeBytes(byte[] bytes)
        {
            bytes = TrimEnd(bytes);
            var body = bytes[4..];
            var message = Unicode.GetString(body);
            return message;
        }

        public static byte[] ToUnicodeBytes(string message)
        {
            var length = (message.Length + 1) * 2 + 4;
            var header = GetBytes(length);
            var body = Unicode.GetBytes(message);
            var footer = new byte[] { 0b0000_0000, 0b0000_0000 };
            var content = header.Concat(body)
                                .Concat(footer)
                                .ToArray();
            return content;
        }

        public static byte[] ToUnicodeBytes(int number)
        {
            var header = GetBytes(4);
            var body = GetBytes(number);
            var content = header.Concat(body)
                                .ToArray();
            return content;
        }

        private static byte[] TrimEnd(byte[] bytes)
        {
            var sizeBytes = new ArraySegment<byte>(bytes, 0, 4);
            var size = ToInt32(sizeBytes);
            //var result = new ArraySegment<byte>(bytes, 0, size);
            return bytes[..size];
        }

        public Package ParsePackage(byte[] bytes)
        {
            // Cleanup Trailing Bytes
            bytes = TrimEnd(bytes);

            // Measurement Package
            // 4 Byte - Packet Size
            //var packetBytes = bytes[..4];
            //var packetSize = ToInt32(packetBytes);

            // 4 Byte - Channel Count
            var channelCountBytes = bytes[4..8];
            var channelCount = ToInt32(channelCountBytes);

            // Rest - Channel Packages
            var channelBytes = bytes[8..];

            var channelData = ReadChannels(channelBytes, channelCount);

            return channelData;
        }

        private Package ReadChannels(
            byte[] bytes, int count)
        {
            var output = new Package();

            for (var c = 0; c < count; c++)
            {
                var pointCountBytes = bytes[..4];
                var pointCount = ToInt32(pointCountBytes);

                var channelNameBytes =
                    bytes.Skip(4)
                         .TakeWhile(b => b > 0)
                         .ToArray();
                var channelName = ASCII.GetString(channelNameBytes);

                var offset = channelNameBytes.Length + 1 + 4;
                var pointBytes = bytes[offset..];
                Dictionary<string, string> pointValues;
                (pointValues, bytes) = ReadPoints(pointBytes, pointCount);
                output[channelName] = pointValues;
            }
            return output;
        }

        private (Dictionary<string, string>, byte[]) ReadPoints(
            byte[] bytes, int count)
        {
            var output = new Dictionary<string, string>();

            for (var p = 0; p < count; p++)
            {
                var nameBytes = bytes.TakeWhile(b => b > 0).ToArray();
                var name = ASCII.GetString(nameBytes);
                bytes = bytes[(name.Length + 1)..];
                var unit = _resolver.GetUnit(name);
                var fourByteData = unit.Type == typeof(int);

                if (fourByteData)
                {
                    var valueBytes = bytes[..4];
                    var value = ToInt32(valueBytes);
                    output[name] = value.ToString(unit.Format) + unit.Name;
                    bytes = bytes[4..];
                }
                else
                {
                    var valueBytes = bytes[..8];
                    var value = ToDouble(valueBytes);
                    output[name] = value.ToString(unit.Format) + unit.Name;
                    bytes = bytes[8..];
                }
            }
            return (output, bytes);
        }
    }
}
