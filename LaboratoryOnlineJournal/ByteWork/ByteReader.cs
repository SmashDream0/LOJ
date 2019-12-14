using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.ByteWork
{
    public class ByteReader:AByteWork
    {
        public ByteReader(byte[] bytes, Encoding encoding)
            : base(bytes, encoding)
        { }

        public string GetString(ref int position)
        {
            var length = GetInt(ref position);

            if (length == -1)
            { return null; }
            else
            {
                var bytes = GetBytes(ref position, length);

                return _encoding.GetString(bytes);
            }
        }

        public int GetInt(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(int));

            return BitConverter.ToInt32(bytes, 0);
        }

        public uint GetUInt(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(uint));

            return BitConverter.ToUInt32(bytes, 0);
        }

        public bool GetBool(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(bool));

            return BitConverter.ToBoolean(bytes, 0);
        }

        public short GetShort(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(short));

            return BitConverter.ToInt16(bytes, 0);
        }

        public long GetLong(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(long));

            return BitConverter.ToInt64(bytes, 0);
        }

        public byte GetByte(ref int position)
        {
            position += sizeof(byte);

            return _byteBuffer[position];
        }

        public float GetFloat(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(float));

            return BitConverter.ToSingle(bytes, 0);
        }

        public double GetDouble(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(double));

            return BitConverter.ToDouble(bytes, 0);
        }

        public decimal GetDecimal(ref int position)
        {
            var bytes = GetBytes(ref position, sizeof(decimal));

            return ToDecimal(bytes);
        }

        public DateTime GetDateTime(ref int position)
        {
            var ticks = GetLong(ref position);

            if (DateTime.MinValue.Ticks > ticks)
            { return DateTime.MinValue; }

            if (DateTime.MaxValue.Ticks < ticks)
            { return DateTime.MaxValue; }

            return new DateTime(ticks);
        }



        public int? GetNullableInt(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetInt(ref position); }
            else
            { return null; }
        }
        public uint? GetNullableUInt(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetUInt(ref position); }
            else
            { return null; }
        }

        public bool? GetNullableBool(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetBool(ref position); }
            else
            { return null; }
        }

        public short? GetNullableShort(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetShort(ref position); }
            else
            { return null; }
        }

        public float? GetNullableFloat(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetFloat(ref position); }
            else
            { return null; }
        }

        public long? GetNullableLong(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetLong(ref position); }
            else
            { return null; }
        }

        public byte? GetNullableByte(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetByte(ref position); }
            else
            { return null; }
        }

        public float? GeNullableFloat(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetFloat(ref position); }
            else
            { return null; }
        }

        public double? GetNullableDouble(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetDouble(ref position); }
            else
            { return null; }
        }

        public decimal? GetNullableDecimal(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetDecimal(ref position); }
            else
            { return null; }
        }

        public DateTime? GetNullableDateTime(ref int position)
        {
            var exist = GetBool(ref position);

            if (exist)
            { return GetDateTime(ref position); }
            else
            { return null; }
        }

        public static unsafe decimal ToDecimal(byte[] bytes)
        {
            decimal value;
            fixed (byte* sp = bytes)
            {
                value = *(decimal*)(sp);
            }
            //Use the decimal's new constructor to
            //create an instance of decimal
            return value;
        }

        public byte[] GetBytes(ref int position, int length)
        {
            var bytes = new byte[length];

            Array.Copy(_byteBuffer, position, bytes, 0, length);

            position += length;

            return bytes;
        }

        public byte[] GetBytes(ref int position)
        {
            var length = GetInt(ref position);

            if (length < 0)
            { return null; }
            else
            { return GetBytes(ref position, length); }
        }
    }
}
