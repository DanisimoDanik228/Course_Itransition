using Application.Service;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Infrastructure.Service.Service
{
    public class CustomIdService : ICustomIdService
    {
        private readonly Random _random = new Random();

        public string GenerateCustomId(string structCustomId, long sequence)
        {
            var parts = JsonSerializer.Deserialize<List<PartCustomId>>(structCustomId);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                var id = int.Parse(part.Id);
                switch (id)
                {
                    case (int)TypePartCustomId.FixedText:
                        result.Append(part.Format);
                        break;

                    case (int)TypePartCustomId.DateTime:
                        result.Append(DateTime.Now.ToString(part.Format.Replace('D', 'd').Replace('Y', 'y')));
                        break;

                    case (int)TypePartCustomId.GUID:
                        result.Append(Guid.NewGuid().ToString());
                        break;

                    case (int)TypePartCustomId.Sequence:
                        result.Append(sequence);
                        break;

                    case (int)TypePartCustomId.DigitNumber6:
                        int rnd6 = _random.Next(0, 1_000_000);
                        result.Append(FormatNumber(rnd6, 6, part.Format));
                        break;

                    case (int)TypePartCustomId.DigitNumber9:
                        int rnd9 = _random.Next(0, 1_000_000_000);
                        result.Append(FormatNumber(rnd9, 9, part.Format));
                        break;

                    case (int)TypePartCustomId.BitNumber20:
                        int rnd20 = _random.Next(0, 1 << 20);
                        result.Append(FormatNumber(rnd20, 7, part.Format));
                        break;

                    case (int)TypePartCustomId.BitNumber32:
                        long rnd32 = (long)(_random.NextInt64(0, 1 << 32));
                        result.Append(FormatNumber(rnd32, 10, part.Format));
                        break;
                }
            }

            return result.ToString();
        }

        public IEnumerable<PartNameCustomId> GetAllPartCustomId()
        {
            return new List<PartNameCustomId>
            {
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.FixedText,
                    Name = "FixedText",
                    AvaliableFormat = ["FixedText"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.BitNumber20,
                    Name = "20-bit random number",
                    AvaliableFormat = ["With lead zero", "Without lead zero"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.BitNumber32,
                    Name = "32-bit random number",
                    AvaliableFormat = ["With lead zero", "Without lead zero"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.DigitNumber6,
                    Name = "6-digit random number",
                    AvaliableFormat = ["With lead zero", "Without lead zero"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.DigitNumber9,
                    Name = "9-digit random number",
                    AvaliableFormat = ["With lead zero", "Without lead zero"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.GUID,
                    Name = "GUID",
                    AvaliableFormat = []
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.DateTime,
                    Name = "Date/time",
                    AvaliableFormat = ["YYYY-MM-DD-HH-mm", "DD.MM.YYYY HH:mm"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.Sequence,
                    Name = "Sequence",
                    AvaliableFormat = []
                }
            };
        }
        public bool IsValidCustomId(string structCustomId, string customId, long sequence)
        {
            var parts = JsonSerializer.Deserialize<List<PartCustomId>>(structCustomId);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                var id = int.Parse(part.Id);
                switch (id)
                {
                    case (int)TypePartCustomId.FixedText:
                        result.Append(CustomIdRegEx.regExFixedText(part.Format));
                        break;

                    case (int)TypePartCustomId.DateTime:
                        result.Append(CustomIdRegEx.regExDateTime(part.Format));
                        break;

                    case (int)TypePartCustomId.GUID:
                        result.Append(CustomIdRegEx.regExGUID());
                        break;

                    case (int)TypePartCustomId.Sequence:
                        result.Append(CustomIdRegEx.regExSequence(sequence.ToString()));
                        break;

                    case (int)TypePartCustomId.DigitNumber6:
                        result.Append(CustomIdRegEx.regExDigitNumber6(part.Format));
                        break;

                    case (int)TypePartCustomId.DigitNumber9:
                        result.Append(CustomIdRegEx.regExDigitNumber9(part.Format));
                        break;

                    case (int)TypePartCustomId.BitNumber20:
                        result.Append(CustomIdRegEx.regExBitNumber20(part.Format));
                        break;

                    case (int)TypePartCustomId.BitNumber32:
                        result.Append(CustomIdRegEx.regExBitNumber32(part.Format));
                        break;
                }
            }

            result.Insert(0,"^");
            result.Append("$");

            var res = Regex.Match(customId ,result.ToString());
            return res.Success;
        }
        private string FormatNumber(long number, int len, string format)
        {
            if (format == "With lead zero")
            {
                return number.ToString("D" + len);
            }
            return number.ToString();
        }
    }

    public static class CustomIdRegEx
    {
        public static string regExFixedText(string format)
        {
            return format;
        }

        public static string regExBitNumber20(string format)
        {
            // 0 - 1048575
            if (format == "With lead zero")
            {
                return "(0{6}\\d|0{5}[1-9]\\d|0{4}[1-9]\\d{2}|0{3}[1-9]\\d{3}|00[1-9]\\d{4}|0[1-9]\\d{5}|10[0-3]\\d{4}|104[0-7]\\d{3}|1048[0-4]\\d{2}|10485[0-6]\\d|104857[0-5])";
            }
            else
            {
                return "(\\d|[1-9]\\d{1,5}|10[0-3]\\d{4}|104[0-7]\\d{3}|1048[0-4]\\d{2}|10485[0-6]\\d|104857[0-5])";
            }
        }

        public static string regExBitNumber32(string format)
        {
            // 0 - 4294967295
            if (format == "With lead zero")
            {
                return "(0{9}\\d|0{8}[1-9]\\d|0{7}[1-9]\\d{2}|0{6}[1-9]\\d{3}|0{5}[1-9]\\d{4}|0{4}[1-9]\\d{5}|0{3}[1-9]\\d{6}|00[1-9]\\d{7}|0[1-9]\\d{8}|[1-3]\\d{9}|4[01]\\d{8}|42[0-8]\\d{7}|429[0-3]\\d{6}|4294[0-8]\\d{5}|42949[0-5]\\d{4}|429496[0-6]\\d{3}|4294967[01]\\d{2}|42949672[0-8]\\d|429496729[0-5])";
            }
            else
            {
                return "(\\d|[1-9]\\d{1,8}|[1-3]\\d{9}|4[01]\\d{8}|42[0-8]\\d{7}|429[0-3]\\d{6}|4294[0-8]\\d{5}|42949[0-5]\\d{4}|429496[0-6]\\d{3}|4294967[01]\\d{2}|42949672[0-8]\\d|429496729[0-5])";
            }
        }

        public static string regExDigitNumber6(string format)
        {
            return (format == "With lead zero") ? "\\d{6}" : "(0|[1-9]{6})";
        }

        public static string regExDigitNumber9(string format)
        {
            return (format == "With lead zero") ? "\\d{9}" : "(0|[1-9]{9})";
        }

        public static string regExGUID()
        {
            return "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
        }

        public static string regExDateTime(string format)
        {
            var pattern = format
                .Replace("YYYY", "\\d{4}")
                .Replace("MM", "(0[1-9]|1[0-2])")
                .Replace("DD", "(0[1-9]|[12]\\d|3[01])")
                .Replace("HH", "(0[1-9]|1\\d|2[0-3])")
                .Replace("mm", "(0\\d|[1-5]\\d)");

            return pattern;
        }

        public static string regExSequence(string format)
        {
            return format;
            //return "\\d{1,100}";
        }
    }
}
