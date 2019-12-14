using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.FormatChecker
{
    public class OldFormatChecker : IFormatChecker
    {
        public OldFormatChecker(Encoding encoding)
        { Encoding = encoding; }

        protected Encoding Encoding { get; private set; }

        public String Mark => "MZђ\u0002\u0000\u0000\u0000\u0000\u0000\u0000Н!ё!LН!This program cannot b\u0000 \u1057 ";

        public bool Check(byte[] bytes)
        {
            bool result;

            using (var memoryStream = new MemoryStream(bytes))
            {
                var bytesCount = Encoding.GetByteCount(Mark);

                var markBytes = new byte[bytesCount];

                memoryStream.Read(markBytes, 0, markBytes.Length);

                var readedMark = Encoding.GetString(markBytes);

                result = String.Equals(readedMark, Mark);
            }

            return result;
        }
    }
}