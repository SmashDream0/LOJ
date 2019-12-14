using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.ByteWork
{
    public class ByteIO
        : ByteReader
    {
        public ByteIO(int byteLength, Encoding encoding)
            : this(new byte[byteLength], encoding)
        { }
        public ByteIO(byte[] bytes, Encoding encoding)
            : base((byte[])bytes.Clone(), encoding)
        { Length = bytes.Length; }
        public ByteIO(int byteLength)
            : this(new byte[byteLength], Encoding.UTF32)
        { }
        public ByteIO(byte[] bytes)
            : this(bytes, Encoding.UTF32)
        { Length = bytes.Length; }

        public byte this[int index]
        {
            get { return _byteBuffer[index]; }
            set { _byteBuffer[index] = value; }
        }

        private void updateBytes(int neededLength)
        {
            if (_byteBuffer.Length < neededLength)
            {
                int newLength = (_byteBuffer.Length) * 2;

                if (newLength < neededLength)
                { newLength = neededLength * 2; }

                Array.Resize(ref _byteBuffer, newLength);
            }

            if (Length < neededLength)
            { Length = neededLength; }
        }

        public int Length { get; set; }

        public byte[] GetBufferBytes()
        {
            return (byte[])_byteBuffer.Clone();
        }

        public byte[] ToByteArray()
        {
            var bytes = new byte[Length];

            Array.Copy(_byteBuffer, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public int Add(string value, int position)
        {
            var bytes = (value == null ? new byte[0] : _encoding.GetBytes(value));

            int length = Add((value == null ? -1 : bytes.Length), position);
            length += AddOnlyBytes(bytes, position + length);

            return length;
        }

        public int Add(int value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(int);
        }

        public int Add(uint value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(uint);
        }

        public int Add(double value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(double);
        }

        public int Add(decimal value, int position)
        {
            var bytes = GetDecimalBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(decimal);
        }

        public int Add(short value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(short);
        }

        public int Add(long value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(long);
        }

        public int Add(byte value, int position)
        {
            var bytes = new[] { value };

            AddOnlyBytes(bytes, position);

            return sizeof(byte);
        }

        public int Add(float value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(float);
        }

        public int Add(bool value, int position)
        {
            var bytes = BitConverter.GetBytes(value);

            AddOnlyBytes(bytes, position);

            return sizeof(bool);
        }

        public int Add(DateTime value, int position)
        {
            return Add(value.Ticks, position);
        }
        

        public int Add(int? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(short? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(long? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(byte? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(bool? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(float? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(double? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = BitConverter.GetBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(decimal? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                var bytes = GetDecimalBytes(value.Value);

                length += AddOnlyBytes(bytes, position + sizeof(bool));
            }

            return length;
        }

        public int Add(DateTime? value, int position)
        {
            var length = Add(value != null, position);

            if (value != null)
            {
                length += Add(value.Value.Ticks, position);
            }

            return length;
        }

        public int Add(byte[] bytes, int position)
        {
            if (bytes == null)
            { return Add(-1, position); }
            else
            {
                position += Add(bytes.Length, position);

                return AddOnlyBytes(bytes, position) + sizeof(int);
            }
        }

        public int AddOnlyBytes(byte[] bytes, int position)
        {
            var oldLength = Length;
            updateBytes(Length + bytes.Length);

            //двигаю в сторону то, что уже есть
            Array.Copy(_byteBuffer, position, _byteBuffer, position + bytes.Length, oldLength - position);

            //переношу новые данные
            Array.Copy(bytes, 0, _byteBuffer, position, bytes.Length);

            return bytes.Length;
        }

        public static unsafe byte[] GetDecimalBytes(decimal value)
        {
            byte[] bytes = new byte[16];

            fixed (byte* sp = bytes)
            {
                *(decimal*)sp = value;
            }

            return bytes;
        }

        public void RemoveBytes(int position, int length)
        {
            //byte[] a = new byte[] { 1, 2, 3, 4, 5 };

            //Array.Copy(a, 3, a, 2, 2);

            Array.Copy(this._byteBuffer, position + length, this._byteBuffer, position, Length - position - length);

            Length -= length;
        }
    }
}