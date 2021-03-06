﻿using LaboratoryOnlineJournal.CryptionProvider;
using LaboratoryOnlineJournal.FormatChecker;
using LaboratoryOnlineJournal.SerializeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeFormatProvider
{
    public abstract class SerializeFormatProvider: ISerializeFormatProvider
    {
        public SerializeFormatProvider(String name, IFormatChecker formatChecker, ISerializeProvider serializeProvider, ICryptionProvider cryption)
        {
            Name = name;
            _formatChecker = formatChecker;
            _serializeProvider = serializeProvider;
            _cryption = cryption;
        }

        public String Name { get; private set; }
        private readonly IFormatChecker _formatChecker;
        private readonly ISerializeProvider _serializeProvider;
        private readonly ICryptionProvider _cryption;

        public bool TryDecodeData(GetRsaDelegate getRSA, byte[] bytes, out DeserializeResult deserializeResult)
        {
            var result = false;
            deserializeResult = null;

            var decodedBytes = _cryption.Decode(bytes, getRSA);

            if (decodedBytes != null && _formatChecker.Check(decodedBytes))
            {
                deserializeResult = _serializeProvider.Deserialize(decodedBytes);
                result = true;
            }

            return result;
        }

        public byte[] EncodeData(RSACryptoServiceProvider rsa, IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID)
        {
            var bytes = _serializeProvider.Serialize(tables, date, userID);
            var result = _cryption.Encode(bytes, rsa);

            return result;
        }
    }
}