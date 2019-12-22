using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Synch
{
    public class KeysContainer
    {
        public KeysContainer(Encoding encoding)
        {
            _keys = new Dictionary<uint, Keys>();
            _encoding = encoding;
        }

        private readonly IDictionary<uint, Keys> _keys;
        private readonly Encoding _encoding;

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="id"></param>
        /// <param name="encodeKey"></param>
        /// <param name="decodeKey"></param>
        public void Add(uint id, string encodeKey, string decodeKey)
        {
            if (_keys.ContainsKey(id))
            { throw new Exception($"Идентификатор {id} уже присутствует в словаре"); }

            _keys.Add(id, new Keys(_encoding.GetBytes(encodeKey), _encoding.GetBytes(decodeKey)));
        }

        /// <summary>
        /// Получить код шифрования/дешифрования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetEncodeKey(uint id)
        {
            string result = String.Empty;

            if (_keys.ContainsKey(id))
            { result = _encoding.GetString(_keys[id].EncodeKey); }

            return result;
        }

        /// <summary>
        /// Получить код шифрования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDecodeKey(uint id)
        {
            string result = String.Empty;

            if (_keys.ContainsKey(id))
            { result = _encoding.GetString(_keys[id].DecodeKey); }

            return result;
        }

        /// <summary>
        /// Количество ключей
        /// </summary>
        public int Count => _keys.Count;

        /// <summary>Ключ шифрования</summary>
        private struct Keys
        {
            public Keys(byte[] EncodeKey, byte[] DecodeKey)
            {
                this.EncodeKey = EncodeKey;
                this.DecodeKey = DecodeKey;
            }

            public readonly byte[] EncodeKey;
            public readonly byte[] DecodeKey;
        }
    }
}