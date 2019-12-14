using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Cryption
{
    public class OldEncryption : ICryption
    {
        public OldEncryption()
        { }

        private struct crypt_struct
        {
            public crypt_struct(Random rnd, int MinCount)
            {
                this.mass = new byte[rnd.Next(MinCount, MinCount * 2)];
                rnd.NextBytes(mass);
                this.index = 0;
            }
            public crypt_struct(Random rnd, int MinCount, int MaxCount)
            {
                this.mass = new byte[rnd.Next(MinCount, MaxCount)];
                rnd.NextBytes(mass);
                this.index = 0;
            }
            public crypt_struct(byte[] MassFrom)
            {
                this.mass = MassFrom;
                this.index = 0;
            }
            public crypt_struct(string Pass)
            {
                this.mass = Encoding.UTF32.GetBytes(Pass);
                this.index = 0;
            }

            public byte Encrypt(byte b)
            {
                if (index > mass.Length - 1)
                { index = 0; }

                unchecked
                {
                    return (byte)(b + mass[index++] - index);
                }
            }

            public byte Decrypt(byte b)
            {
                if (index > mass.Length - 1)
                { index = 0; }

                unchecked
                {
                    return (byte)(b - mass[index++] + index);
                }
            }

            public readonly byte[] mass;
            int index;

            public string pass { get { return Encoding.UTF32.GetString(mass); } }
        }

        private static string ByteString(byte[] array)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            { sb.Append(array[i]).Append(','); }

            return sb.ToString();
        }

        /// <summary>Расшифровать массив</summary>
        /// <param name="result">Массив шифрации</param>
        /// <returns></returns>
        public unsafe byte[] Decode(byte[] mass, Func<uint, RSACryptoServiceProvider> getRSA)
        {
            byte[] result = mass.Clone() as byte[];

            uint UserID;
            fixed (void* numPtr = &result[result.Length - 4])
            { UserID = *(uint*)numPtr; }  //id пользователя

            var rsa = getRSA(UserID);

            crypt_struct urbytes, mrpass;

            int enckeyLength;

            {
                //длина внешнего пароля
                fixed (void* numPtr = &result[0])
                { enckeyLength = *(int*)numPtr; }

                var tmass = new byte[enckeyLength];
                Array.Copy(result, 8, tmass, 0, enckeyLength);

                int m1ln, m2ln;

                fixed (void* numPtr = &result[0])
                { m1ln = *(int*)numPtr; }

                fixed (void* numPtr = &result[4])
                { m2ln = *(int*)numPtr; }

                var tmp = ByteString(tmass);

                urbytes = new crypt_struct(rsa.Decrypt(tmass, true));    //дешифрованный шифр   //тут все правильно!
            }

            //дешифрую основной и первый пароль вторым шифром
            {
                int Start = 8 + enckeyLength;
                int End = result.Length - 4;

                for (int i = Start; i < End; i++)
                { result[i] = urbytes.Decrypt(result[i]); }
            }

            {
                int mrpLength;   //длина внутреннего пароля
                fixed (void* numPtr = &result[4])
                { mrpLength = *(int*)numPtr; }

                var tmass = new byte[mrpLength];
                Array.Copy(result, 8 + enckeyLength, tmass, 0, tmass.Length);

                mrpass = new crypt_struct(tmass);    //внутренний пароль смещения
            }

            //дешифрую основной набор первым шифром
            {
                int Start = mrpass.mass.Length + enckeyLength + 8;
                int End = result.Length - 4;

                for (int i = Start; i < End; i++)
                { result[i] = mrpass.Decrypt(result[i]); }
            }
            {
                int MassFrom = mrpass.mass.Length + enckeyLength + 8;
                int MassLength = result.Length - MassFrom;

                Array.Copy(result, MassFrom, result, 0, MassLength);
                Array.Resize(ref result, MassLength);
            }

            return result;
        }

        /// <summary>Шифровать массив</summary>
        /// <param name="mass">Массив для шифрования</param>
        public unsafe byte[] Encode(byte[] mass, RSACryptoServiceProvider rsa)
        {
            var result = mass.Clone() as byte[];

            var rnd = new Random();

            var mrpass = new crypt_struct(rnd, result.Length / 2);   //внутренний пароль смещения   
            var urbytes = new crypt_struct(rnd, 200, 200);    //внешний пароль смещения

            var passPos = result.Length;
            int enckeyLength;

            {
                var tmp = ByteString(urbytes.mass);

                var enckey = rsa.Encrypt(urbytes.mass, true); //шифрованый шифр

                tmp = ByteString(enckey);

                enckeyLength = enckey.Length;   //длина шифрованного пароля

                //новая длина, с учетом длины паролей
                Array.Resize(ref result, result.Length + mrpass.mass.Length + enckey.Length + 8);   //+8 - длина внешнего пароля 4(позиция 0) + длина внутреннего пароля(позиция 4)

                //сдвигаю на указатели длин паролей и длины внутреннего + внешнего паролей. id автора не шифруется
                Array.Copy(result, 0, result, 8 + enckey.Length + mrpass.mass.Length, passPos);

                fixed (void* numPtr = &result[0])
                { *(int*)numPtr = enckey.Length; } //длина внешнего пароля

                fixed (void* numPtr = &result[4])
                { *(int*)numPtr = mrpass.mass.Length; } //длина внутреннего пароля

                //копирую внешний пароль
                Array.Copy(enckey, 0, result, 8, enckey.Length);
                //копирую внутренний пароль
                Array.Copy(mrpass.mass, 0, result, enckey.Length + 8, mrpass.mass.Length);
            }

            //шифрую данные первым шифром
            {
                int Start = mrpass.mass.Length + enckeyLength + 8;
                int End = result.Length - 4;

                for (int i = Start; i < End; i++)
                { result[i] = mrpass.Encrypt(result[i]); }
            }
            //шифрую данные и первый пароль вторым шифром(иначе - все, кроме указателя длины внешнего пароля и id пользователя)
            {
                int Start = 8 + enckeyLength;
                int End = result.Length - 4;

                for (int i = Start; i < End; i++)
                { result[i] = urbytes.Encrypt(result[i]); }
            }

            return result;
        }
    }
}