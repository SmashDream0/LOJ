using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.FormatChecker
{
    public interface IFormatChecker
    {
        bool Check(byte[] bytes);
    }
}