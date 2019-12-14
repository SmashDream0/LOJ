using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.ByteWork
{
    public abstract class AByteWork
    {
        protected AByteWork(byte[] bytes, Encoding encoding)
        {
            if (bytes == null || bytes.Length == 0)
            { this._byteBuffer = new byte[1]; }
            else
            { this._byteBuffer = bytes; }
            this._encoding = encoding;
        }

        protected byte[] _byteBuffer;
        protected Encoding _encoding;
    }
}
