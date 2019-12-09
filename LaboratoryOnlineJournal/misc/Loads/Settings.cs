using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using NPOI.HSSF.UserModel;
//using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;

namespace LaboratoryOnlineJournal
{
    public static partial class Misc
    {
        public static void Prepare()
        {
            var Length = new int[Enum.GetNames(typeof(data.Strings)).Length];

            Length[(int)data.Strings.UseSQL] = 5;
            Length[(int)data.Strings.SqlIp] = 15;
            Length[(int)data.Strings.SqlIp1] = 15;
            Length[(int)data.Strings.SqlIp2] = 15;
            Length[(int)data.Strings.SqlIpLast] = 15;
            Length[(int)data.Strings.SqlLogin] = 15;
            Length[(int)data.Strings.SqlPassword] = 15;
            Length[(int)data.Strings.DATABASE] = 15;
            Length[(int)data.Strings.SqlPort] = 6;
            Length[(int)data.Strings.DirectIMAPAdress] = 15;
            Length[(int)data.Strings.DirectIMAPPort] = 6;
            Length[(int)data.Strings.DirectIMAPUseSSL] = 5;
            Length[(int)data.Strings.SMTPAdress] = 15;
            Length[(int)data.Strings.SMTPPort] = 6;
            Length[(int)data.Strings.SMTPUseSSL] = 5;
            Length[(int)data.Strings.MailLogin] = 35;
            Length[(int)data.Strings.MailPass] = 25;
            Length[(int)data.Strings.LastUser] = 5;

            Length[(int)data.Strings.DirectSMTPAdress] = 15;
            Length[(int)data.Strings.DirectSMTPPort] = 6;
            Length[(int)data.Strings.DirectSMTPCrypt] = 5;
            Length[(int)data.Strings.DirectMailLogin] = 35;
            Length[(int)data.Strings.DirectMailPass] = 25;
            Length[(int)data.Strings.AutoCrypto] = 5;
            Length[(int)data.Strings.Changes] = 5;

            data.PrgSettings = new Settings(Application.StartupPath + "\\App\\", "Settings.dll", Enum.GetNames(typeof(data.Forms)).Length, Length);
        }
    }
}