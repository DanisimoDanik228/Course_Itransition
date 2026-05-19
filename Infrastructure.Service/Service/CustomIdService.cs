using Application.Service;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Service.Service
{
    public class CustomIdService : ICustomIdService
    {
        private readonly Random _random = new Random();

        public string GenerateCustomId(string structCustomId)
        {
            int sequence = 1;

            var parts = JsonSerializer.Deserialize<List<PartCustomId>>(structCustomId);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                switch (part.typePartCustomId)
                {
                    case TypePartCustomId.FixedText:
                        result.Append("FixedText");
                        break;

                    case TypePartCustomId.DateTime:
                        result.Append(DateTime.Now.ToString(part.Format));
                        break;

                    case TypePartCustomId.GUID:
                        result.Append(Guid.NewGuid().ToString()); 
                        break;

                    case TypePartCustomId.Sequence:
                        result.Append(sequence);
                        break;

                    case TypePartCustomId.DigitNumber6:
                        int rnd6 = _random.Next(0, 1_000_000);
                        result.Append(FormatNumber(rnd6, 6, part.Format));
                        break;

                    case TypePartCustomId.DigitNumber9:
                        int rnd9 = _random.Next(0, 1_000_000_000);
                        result.Append(FormatNumber(rnd9, 9, part.Format));
                        break;

                    case TypePartCustomId.BitNumber20:
                        int rnd20 = _random.Next(0, 1 << 20);
                        result.Append(FormatNumber(rnd20, 7, part.Format));
                        break;

                    case TypePartCustomId.BitNumber32:
                        long rnd32 = (long)(_random.NextInt64(0,1 << 32));
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
                    AvaliableFormat = []
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
                    AvaliableFormat = ["yyyy-MM-dd/HH-mm", "dd.MM.yyy HH:mm"]
                },
                new PartNameCustomId {
                    TypePartCustomId = (int)TypePartCustomId.Sequence,
                    Name = "Sequence",
                    AvaliableFormat = []
                }
            };
        }

        public bool IsValidCustomId(string structCustomId, string customId)
        {
            throw new NotImplementedException();
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
}
