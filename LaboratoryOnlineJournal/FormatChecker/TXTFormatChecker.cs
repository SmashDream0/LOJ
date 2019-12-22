using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.FormatChecker
{
    public class TXTFormatChecker : IFormatChecker
    {
        public TXTFormatChecker(Encoding encoding)
        { Encoding = encoding; }

        protected Encoding Encoding { get; private set; }

        public String Mark = "It's unsecure text!";

        public virtual bool Check(byte[] bytes)
        {
            var result = bytes != null;

            if (result)
            {
                using (var sr = new StreamReader(new MemoryStream(bytes), Encoding))
                {
                    var mark = sr.ReadLine().Trim();

                    result = String.Equals(mark, Mark);
                }
            }

            return result;
        }
    }
}
