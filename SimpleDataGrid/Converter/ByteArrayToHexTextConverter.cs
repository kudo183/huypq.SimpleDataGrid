﻿using System;
using System.Windows.Data;

namespace SimpleDataGrid.Converter
{
    public class ByteArrayToHexTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var byteArray = (byte[])value;
            return ByteArrayToHexViaLookup32(byteArray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var hexString = (string)value;
            return ParseHexString(hexString);
        }

        private static readonly uint[] _lookup32 = CreateLookup32();

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        private static string ByteArrayToHexViaLookup32(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        private static byte[] ParseHexString(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                hex = hex.Insert(0, "0");
            }
            var result = new byte[hex.Length / 2];
            int resultIndex = 0;
            string s;
            for (int i = 0; i < hex.Length; i += 2)
            {
                s = hex.Substring(i, 2);
                result[resultIndex] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                resultIndex++;
            }
            return result;
        }
    }
}
