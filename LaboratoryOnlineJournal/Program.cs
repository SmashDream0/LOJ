using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Security.Authentication;
using System.Runtime.InteropServices;
using System.Reflection;
using ImapX;
using SevenZip;
using System.Threading;

namespace LaboratoryOnlineJournal
{
    static class Program
    {
        /// <summary>Установить фокус на стороннюю форму</summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            //MessageBox.Show("Точка входа");

            {
                var S = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);

                if (S.Length > 1)
                {
                    var CurrPID = System.Diagnostics.Process.GetCurrentProcess().Id;

                    if (MessageBox.Show("Программа уще запущена\nЗакрыть ту, что была запущена раньше?\nВ противном случае запуск этой копии будет прерван.", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        for (int i = 0; i < S.Length; i++)
                        {
                            if (S[i].Id != CurrPID)
                            { S[i].Kill(); }
                        } 
                    }
                    else
                    {
                        for (int i = 0; i < S.Length; i++)
                        {
                            if (S[i].Id != CurrPID)
                            {
                                SetForegroundWindow(S[i].MainWindowHandle);
                                break;
                            }
                        }
                        Application.ExitThread();
                        return;
                    }
                }
            }

            var m = new List<string>(3);

            if (!System.IO.File.Exists(Application.StartupPath + "\\MySql.Data.dll"))
            { m.Add("MySql.Data.dll"); }

            if (!System.IO.File.Exists(Application.StartupPath + "\\NPOI.dll"))
            { m.Add("NPOI.dll"); }

            if (!System.IO.File.Exists(Application.StartupPath + "\\AutoTable.dll"))
            { m.Add("AutoTable.dll"); }

            if (m.Count > 0)
            {
                var message = "";
                for (int i = 0; i < m.Count; i++)
                {
                    if (m[i] != null)
                    { message += '\n' + (i + 1).ToString() + ")" + m[i]; }
                    else
                    { break; }
                }

                MessageBox.Show("Похоже, что отсутсвуют некоторые компоненты программы:" + message + "\nЧтобы устранить проблему обратитесь в отдел ИТиТ");
                return;
            }

            Application.SetCompatibleTextRenderingDefault(false);

            SetArgs(Args);

            if (File.Exists(Application.StartupPath + "\\commands.txt"))
            {
                using (var sr = new StreamReader(Application.StartupPath + "\\commands.txt"))
                {
                    var FArgs = new List<string>();

                    while (!sr.EndOfStream)
                    { FArgs.Add(sr.ReadLine()); }

                    SetArgs(FArgs.ToArray());
                }
            }

            if (data.DeleteConf && File.Exists(Application.StartupPath + "\\commands.txt"))
            { File.Delete(Application.StartupPath + "\\commands.txt"); }

            AutoUpdate.UpdateAutoUpdate(data.StName, null);

            Misc.Prepare();  //гружу настройки

            var form = new Startup_Form();
            if (!form.IsDisposed)
            {
                Application.EnableVisualStyles();
                Application.Run(form);
            }
        }

        static void SetArgs(string[] Args)
        {
            for (int i = 0; i < Args.Length; i++)
            {
                var cmd = Args[i].ToLower();

                if (cmd.Length > data.CMD.AlowToChange.Length && cmd.IndexOf(data.CMD.AlowToChange, 0, data.CMD.AlowToChange.Length) > -1)
                {
                    if (!bool.TryParse(cmd.Substring(data.CMD.AlowToChange.Length), out data.AllowModify))
                    { data.AllowModify = false; }
                }
                else if (cmd.Length > data.CMD.SettingsFile.Length && cmd.IndexOf(data.CMD.SettingsFile, 0, data.CMD.SettingsFile.Length) > -1)
                { data.StName = cmd.Substring(data.CMD.SettingsFile.Length); }
                else if (cmd.Length > data.CMD.SetIncrem.Length && cmd.IndexOf(data.CMD.SetIncrem, 0, data.CMD.SetIncrem.Length) == 0)
                {
                    if (!int.TryParse(cmd.Substring(data.CMD.SetIncrem.Length), out data.Increm))
                    { data.Increm = -1; }
                }
                else if (cmd.Length > data.CMD.DeleteMe.Length && cmd.IndexOf(data.CMD.DeleteMe, 0, data.CMD.DeleteMe.Length) > -1)
                {
                    if (!bool.TryParse(cmd.Substring(data.CMD.DeleteMe.Length), out data.DeleteConf))
                    { data.DeleteConf = false; }
                }
            }
        }
    }

    public static class AutoUpdate
    {
        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public enum PROCESSOR_ARCHITECTURE : short { INTEL = 0, AMD64 = 9, IA64 = 6 }

            public PROCESSOR_ARCHITECTURE wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        public static string Get7zdllName()
        {
            var si = new SYSTEM_INFO();
            GetNativeSystemInfo(ref si);

            switch (si.wProcessorArchitecture)
            {
                case SYSTEM_INFO.PROCESSOR_ARCHITECTURE.AMD64:
                case SYSTEM_INFO.PROCESSOR_ARCHITECTURE.IA64:
                    if (!File.Exists("7z64_cp.dll"))
                    { return null; }
                    else
                    { return "7z64_cp.dll"; }
                case SYSTEM_INFO.PROCESSOR_ARCHITECTURE.INTEL:
                    if (!File.Exists("7z32_cp.dll"))
                    { return null; }
                    else
                    { return "7z32_cp.dll"; }
                default: throw new Exception("Неизвестная архитектура");
            }
        }

        public class settings_class:IDisposable
        {
            public settings_class(string FileName)
            {
                this.SettingsFileName = FileName;
                Open();
            }

            string Path { get { return Application.StartupPath + "\\" + SettingsFileName; } }
            string SettingsFileName;

            public void Close()
            {
                //ChangeTicks = File.GetLastWriteTime(Path).Ticks;
                fs.Close();
            }

            public void Dispose()
            {
                this.Close();
                this.fs.Dispose();
            }

            //long ChangeTicks = 0;

            public bool Open()
            {
                if (!File.Exists(Path))
                {
                    fs = new FileStream(Path, FileMode.Create);
                    this.MainPass = "";
                    fs.Flush();
                }
                else
                    fs = new FileStream(Path, FileMode.Open);

                return true;
            }
            //пароль    -25
            //файл запуска с параметром(одним)  -25
            //пароль к архиву   -25
            //дата последнего обновления по версии текущего ПК  -8
            //тип обновления(с сервера/с почты) -1
            //дата изменения последнего файла обновления    -8
            //путь до файла с обновлением   -255
            //название почты обновления -25
            //пароль к почте    -25
            //адрес Imap    -25
            //порт  -5
            //шифрование    -1

            static byte[] check = { 4, 56, 45, 6, 56, 48, 48, 48, 48, 8, 6, 4, 2, 42, 1, 6, 6, 16, 57, 8, 78, 7, 24, 2, 12, 3, 5, 4, 4, 8, 4, 54, 65, 2, 1, 54, 65, 1, 235, 7, 87, 8, 254, 12, 1, 1, 58, 4, 8, 44, 255, 48, 4, 85, 65, 4, 81, 194, 98, 4, 94, 8 };

            struct value_size
            {
                /// <summary>Пароль доступа</summary>
                public const byte MainPass = 25;
                /// <summary>файл запуска</summary>
                public const byte FileName = 50;
                /// <summary>параметр файла запуска</summary>
                public const byte Param = 50;
                /// <summary>Пароль к архиву</summary>
                public const byte ArchPass = 50;
                /// <summary>дата последнего обновления по версии текущего ПК</summary>
                public const byte LastUpdateDate = 19;
                /// <summary>Тип обновления</summary>
                public const byte UpdateType = 1;
                /// <summary>дата изменения последнего файла обновления</summary>
                public const byte LastFileDate = 19;
                /// <summary>путь до файла с обновлением</summary>
                public const byte FileUpdateLocation = 255;
                /// <summary>Пароль доступа</summary>
                public const byte MailName = 50;
                /// <summary>название почты обновления</summary>
                public const byte MailPass = 50;
                /// <summary>название Imap</summary>
                public const byte MailImap = 50;
                /// <summary>порт</summary>
                public const byte MailPort = 5;
                /// <summary>шифрование</summary>
                public const byte MailSecure = 3;
                /// <summary>Запускать исполняемый файл</summary>
                public const byte StartFile = 1;
                /// <summary>Автоматически определять шифрование</summary>
                public const byte AutoCrypto = 1;
            }

            public enum UpdateType_enum : byte { Path, Mail, None };

            struct value_location
            {
                /// <summary>Пароль доступа</summary>
                public const int Locn_MainMass = 0;
                /// <summary>файл запуска</summary>
                public const int Locn_FileName = Locn_MainMass + value_size.MainPass;
                /// <summary>параметр файла запуска</summary>
                public const int Locn_Param = Locn_FileName + value_size.FileName;
                /// <summary>параметр файла запуска</summary>
                public const int Locn_ArchPass = Locn_Param + value_size.Param;
                /// <summary>дата последнего обновления по версии текущего ПК</summary>
                public const int Locn_LastUpdateDate = Locn_ArchPass + value_size.ArchPass;
                /// <summary>Тип обновления</summary>
                public const int Locn_UpdateType = Locn_LastUpdateDate + value_size.LastUpdateDate;
                /// <summary>дата изменения последнего файла обновления</summary>
                public const int Locn_LastFileDate = Locn_UpdateType + value_size.UpdateType;
                /// <summary>путь до файла с обновлением</summary>
                public const int Locn_FileUpdateLocation = Locn_LastFileDate + value_size.LastFileDate;
                /// <summary>Пароль доступа</summary>
                public const int Locn_MailName = Locn_FileUpdateLocation + value_size.FileUpdateLocation;
                /// <summary>название почты обновления</summary>
                public const int Locn_MailPass = Locn_MailName + value_size.MailName;
                /// <summary>название Imap</summary>
                public const int Locn_MailImap = Locn_MailPass + value_size.MailPass;
                /// <summary>порт</summary>
                public const int Locn_MailPort = Locn_MailImap + value_size.MailImap;
                /// <summary>шифрование</summary>
                public const int Locn_MailSecure = Locn_MailPort + value_size.MailPort;
                /// <summary>шифрование</summary>
                public const int Locn_StartFile = Locn_MailSecure + value_size.MailSecure;
                /// <summary>шифрование</summary>
                public const int Locn_AutoCrypto = Locn_StartFile + value_size.AutoCrypto;
            }

            FileStream fs;

            static byte[] Crypting(byte[] res, int location, sbyte Increment)   //Шифрует текст
            {
                unchecked
                {
                    int j;
                    if (location > check.Length)
                    { j = location % check.Length; }
                    else
                    { j = location; }

                    for (int i = 0; i < res.Length; i++)
                    {
                        if (j == check.Length)
                        { j = 0; }
                        res[i] = (byte)(res[i] - ((check[j++] + (i + 1) * location) * Increment));
                    }
                }
                return res;
            }

            static byte[] CryptIt(byte[] res, int location)   //Шифрует текст
            {
                return Crypting(res, location, 1);
            }

            static string DeCryptItString(Stream str, int location, int Length) //Расшифровывает текст
            {
                var bytes = new byte[Length];
                str.Position = location;
                str.Read(bytes, 0, Length);

                return Encoding.Default.GetString(Crypting(bytes, location, -1)).Replace("\0", "");
            }

            static byte[] DeCryptItByte(byte[] res, int location) //Расшифровывает текст
            {
                return Crypting(res, location, -1);
            }

            static void CryptIt(string Volume, Stream str, int location, int length)
            {
                var bytes = new byte[length];

                if (Volume.Length > length)
                { Encoding.Default.GetBytes(Volume, 0, length, bytes, 0); }
                else
                { Encoding.Default.GetBytes(Volume, 0, Volume.Length, bytes, 0); }

                CryptIt(bytes, location);

                str.Position = location;
                str.Write(bytes, 0, bytes.Length);
            }

            public string MainPass
            {
                get { return DeCryptItString(fs, value_location.Locn_MainMass, value_size.MainPass); }
                set { CryptIt(value, fs, value_location.Locn_MainMass, value_size.MainPass); }
            }

            public string FileName
            {
                get { return DeCryptItString(fs, value_location.Locn_FileName, value_size.FileName); }
                set { CryptIt(value, fs, value_location.Locn_FileName, value_size.FileName); }
            }

            public string Param
            {
                get { return DeCryptItString(fs, value_location.Locn_Param, value_size.Param); }
                set { CryptIt(value, fs, value_location.Locn_Param, value_size.Param); }
            }

            public string ArchPass
            {
                get { return DeCryptItString(fs, value_location.Locn_ArchPass, value_size.ArchPass); }
                set { CryptIt(value, fs, value_location.Locn_ArchPass, value_size.ArchPass); }
            }

            public DateTime LastUpdateDate
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_LastUpdateDate, value_size.LastUpdateDate);
                    DateTime dt;
                    if (DateTime.TryParse(value, out dt))
                    { return dt; }
                    else
                    { return DateTime.Now; }
                }
                set { CryptIt(value.ToString(), fs, value_location.Locn_LastUpdateDate, value_size.LastUpdateDate); }
            }

            public UpdateType_enum UpdateType
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_UpdateType, value_size.UpdateType);
                    byte b;
                    if (!byte.TryParse(value, out b))
                    { b = 0; }

                    switch ((UpdateType_enum)b)
                    {
                        case UpdateType_enum.Mail:
                            return UpdateType_enum.Mail;
                        default:
                        case UpdateType_enum.Path:
                            return UpdateType_enum.Path;
                    }
                }
                set { CryptIt(((byte)value).ToString(), fs, value_location.Locn_UpdateType, value_size.UpdateType); }
            }

            public DateTime LastFileDate
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_LastFileDate, value_size.LastFileDate);
                    DateTime dt;
                    if (DateTime.TryParse(value, out dt))
                    { return dt; }
                    else
                    { return DateTime.Now; }
                }
                set { CryptIt(value.ToString(), fs, value_location.Locn_LastFileDate, value_size.LastFileDate); }
            }

            public string FileUpdateLocation
            {
                get { return DeCryptItString(fs, value_location.Locn_FileUpdateLocation, value_size.FileUpdateLocation); }
                set { CryptIt(value, fs, value_location.Locn_FileUpdateLocation, value_size.FileUpdateLocation); }
            }

            public string MailName
            {
                get { return DeCryptItString(fs, value_location.Locn_MailName, value_size.MailName); }
                set { CryptIt(value, fs, value_location.Locn_MailName, value_size.MailName); }
            }

            public string MailPass
            {
                get { return DeCryptItString(fs, value_location.Locn_MailPass, value_size.MailPass); }
                set { CryptIt(value, fs, value_location.Locn_MailPass, value_size.MailPass); }
            }

            public string MailImap
            {
                get { return DeCryptItString(fs, value_location.Locn_MailImap, value_size.MailImap); }
                set { CryptIt(value, fs, value_location.Locn_MailImap, value_size.MailImap); }
            }

            public int MailPort
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_MailPort, value_size.MailPort);
                    int i;
                    if (int.TryParse(value, out i))
                    { return i; }
                    else
                    { return 0; }
                }
                set { CryptIt(value.ToString(), fs, value_location.Locn_MailPort, value_size.MailPort); }
            }

            public bool StartFile
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_StartFile, value_size.StartFile);
                    byte i;
                    if (byte.TryParse(value, out i))
                    { return Convert.ToBoolean(i); }
                    else
                    { return false; }
                }
                set
                {
                    if (value)
                    { CryptIt("1", fs, value_location.Locn_StartFile, value_size.StartFile); }
                    else
                    { CryptIt("0", fs, value_location.Locn_StartFile, value_size.StartFile); }
                }
            }

            public SslProtocols MailSecure
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_MailSecure, value_size.MailSecure);
                    byte b;
                    if (!byte.TryParse(value, out b))
                    { b = 0; }

                    switch ((SslProtocols)b)
                    {
                        case System.Security.Authentication.SslProtocols.Default:
                        case System.Security.Authentication.SslProtocols.Ssl2:
                        case System.Security.Authentication.SslProtocols.Ssl3:
                        case System.Security.Authentication.SslProtocols.Tls:
                            return (SslProtocols)b;
                        case System.Security.Authentication.SslProtocols.None:
                        default:
                            return System.Security.Authentication.SslProtocols.None;
                    }
                }
                set { CryptIt(((byte)value).ToString(), fs, value_location.Locn_MailSecure, value_size.MailSecure); }
            }

            public bool AutoCrypto
            {
                get
                {
                    var value = DeCryptItString(fs, value_location.Locn_AutoCrypto, value_size.AutoCrypto);
                    byte i;
                    if (byte.TryParse(value, out i))
                    { return Convert.ToBoolean(i); }
                    else
                    { return false; }
                }
                set
                {
                    if (value)
                    { CryptIt("1", fs, value_location.Locn_AutoCrypto, value_size.AutoCrypto); }
                    else
                    { CryptIt("0", fs, value_location.Locn_AutoCrypto, value_size.AutoCrypto); }
                }
            }
        }

        public static void UpdateAutoUpdate(string FileName, string Command)
        {
            settings_class Settings;
            var Log = false;

            switch (Command)
            {
                /*case "settings":
                    Settings = new settings_class(FileName);
                SettingsMark: ;
                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);

                    if (Settings.MainPass.Length == 0)
                    { new Settings_Form(Settings).ShowDialog(); }
                    else
                    { new Password_Form(Settings).ShowDialog(); }
                    return;
                case "log":
                    Log = true;
                    goto Default;
                default:
                    if (Command.Length > 4 && Command.Substring(0, 4) == "fsn-")
                    { FileName = Command.Substring(4, Command.Length - 4); }
                Default: ;*/
                default:
                    Settings = new settings_class(FileName);

                    /*if (Settings.MainPass.Length == 0)
                    { goto SettingsMark; }*/
                    break;
            }

            var lines = new StringBuilder();
            lines.AppendLine(DateTime.Now.ToString() + " - Старт обновления");

            string FilePath = null;
            var date = DateTime.MaxValue;

            switch (Settings.UpdateType)
            {
                case settings_class.UpdateType_enum.Mail:
                    if (!File.Exists("ImapX_cp.dll"))
                    {
                        lines.AppendLine(DateTime.Now.ToString() + " - Библиотека ImapX не найдена");
                        goto End;
                    }
                    {
                        var s = new FindUpdate_class(lines, Settings);
                        new Progress_Form(s).ShowDialog();
                        FilePath = s.FilePath;
                        date = s.Date;
                    }

                    break;
                case settings_class.UpdateType_enum.Path:
                    lines.AppendLine(DateTime.Now.ToString() + " - поиск файла обновления");

                    if (Settings.FileUpdateLocation.Length == 0 || !File.Exists(Settings.FileUpdateLocation))
                    { FilePath = null; }
                    else
                    {
                        FilePath = Settings.FileUpdateLocation;

                        date = File.GetLastWriteTime(FilePath);

                        date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);  //приходится костылить, чтоб даты были одинаковыми, с точностью до секунды

                        if (date <= Settings.LastFileDate)
                        { FilePath = null; }
                    }

                    break;
                default:
                    lines.AppendLine(DateTime.Now.ToString() + " - неизвестный тип обновления");
                    break;
            }

            if (FilePath != null && File.Exists(FilePath))
            {
                var Processes = System.Diagnostics.Process.GetProcessesByName(Settings.FileName);

                for (int i = 0; i < Processes.Length; i++)
                {
                    if (!Processes[i].CloseMainWindow())
                    { Processes[i].Kill(); }
                }

                {
                    var sevenzdllname = Get7zdllName();

                    if (sevenzdllname == null)
                    {
                        lines.AppendLine(DateTime.Now.ToString() + " - библиотека не найдена");
                        goto End;
                    }
                    else
                    {
                        SevenZip.SevenZipCompressor.SetLibraryPath(
                            Path.Combine(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            sevenzdllname));
                        lines.AppendLine(DateTime.Now.ToString() + " - распаковка архива обновления");
                    }
                }

                {
                    var Password = Settings.ArchPass;

                    Settings.Close();

                    if (new Progress_Form(new Extract_class(FilePath, Password)).ShowDialog() == DialogResult.OK)
                    {
                        lines.AppendLine(DateTime.Now.ToString() + " - распаковка прошла успешно");

                        Settings.Open();
                        Settings.LastFileDate = date;
                        Settings.LastUpdateDate = DateTime.Now;
                    }
                    else
                    {
                        lines.AppendLine(DateTime.Now.ToString() + " - распаковна не была произведена");
                        Settings.Open();
                    }
                }
            }

            if (Settings.StartFile)
            { System.Diagnostics.Process.Start(Settings.FileName); }

        End: ;

            if (Log)
            { WriteLog(lines.ToString()); }
        }

        static void WriteLog(string Message)
        {
            var SW = (new StreamWriter(Application.StartupPath + "\\" + Application.ProductName + "_Log.txt", false));
            SW.WriteLine(DateTime.Now.ToString() + " - " + Message);
            SW.Close();
        }

        class Extract_class : Progress_Form.AObject
        {
            public Extract_class(string ArchPath, string ArchPass)
                : base(true)
            {
                this.ArchPath = ArchPath;
                this.ArchPass = ArchPass;
            }
            string ArchPath, ArchPass;

            protected override bool Do()
            {
                SevenZipExtractor extr;

                if (ArchPass.Length > 0)
                { extr = new SevenZipExtractor(ArchPath, ArchPass); }
                else
                { extr = new SevenZipExtractor(ArchPath); }

                extr.Extracting += (pdn, pdt) => Action("Распаковка", 100, pdt.PercentDone);
                extr.ExtractArchive(Application.StartupPath);
                extr.Dispose();

                return true;
            }
        }

        class FindUpdate_class : Progress_Form.AObject
        {
            public FindUpdate_class(StringBuilder lines, settings_class Settings)
                : base(true)
            {
                this.lines = lines;
                this.Settings = Settings;
                this.Date = Settings.LastFileDate;
            }

            settings_class Settings;
            public string FilePath = null;
            public DateTime Date;

            StringBuilder lines;

            protected override bool Do()
            {
                int MaxCount = 3;
                int CurrCount = 0;

                Action("Подключение к почте", MaxCount, CurrCount++);
                lines.AppendLine(DateTime.Now.ToString() + " - подключение к почте");

                ImapX.ImapClient Client;

                if (Settings.AutoCrypto)
                { Client = new ImapX.ImapClient(Settings.MailImap, Settings.MailPort, Settings.AutoCrypto); }
                else
                { Client = new ImapX.ImapClient(Settings.MailImap, Settings.MailPort, Settings.MailSecure); }

                if (!Client.Connect())
                {
                    Action("Подключение к почте не удалось: проверьте настройки подключения либо сервер не доступен", MaxCount, CurrCount++);
                    lines.AppendLine(DateTime.Now.ToString() + " - подключение к почте не удалось");
                    return true;
                }
                if (!Client.Login(Settings.MailName, Settings.MailPass))
                {
                    Action("Подключение к почте не удалось: проверьте логин и/или пароль", MaxCount, CurrCount++);
                    lines.AppendLine(DateTime.Now.ToString() + " - подключение к почте не удалось");
                    return true;
                }

                MaxCount++;
                Action("Поиск обновлений", MaxCount, CurrCount++);
                lines.AppendLine(DateTime.Now.ToString() + " - успешное подключение к почте");

                var SearchedSubject = Path.GetFileNameWithoutExtension(Settings.FileName);
                //var messages = Client.Folders.Inbox.Search("CHARSET UTF-8 UNSEEN SUBJECT {" + Encoding.GetEncoding("UTF-8").GetByteCount(SearchedSubject) + "}\r\n" + SearchedSubject, ImapX.Enums.MessageFetchMode.Headers).ToList();
                var messages = Client.Folders.Inbox.Search("UNSEEN", ImapX.Enums.MessageFetchMode.Headers).ToList();

                MaxCount += messages.Count + 3;

                Action("Сканирование писем", MaxCount, CurrCount++);

                int LastDateIndex = -1;

                SearchedSubject = "обновление " + SearchedSubject.ToLower();

                for (int i = 0; i < messages.Count; i++)
                {
                    if (messages[i].Subject.ToLower() == SearchedSubject && messages[i].Date > Date)
                    {
                        LastDateIndex = i;
                        Action(MaxCount, CurrCount++);
                    }
                    else
                    { Action(MaxCount, CurrCount++); }
                }

                if (LastDateIndex > -1)
                {
                    MaxCount++;

                    Action("Получение файла обновлений", MaxCount, CurrCount++);

                    messages[LastDateIndex].Download(ImapX.Enums.MessageFetchMode.Full);
                    //messages[i].Attachments[0].Download();

                    if (messages[LastDateIndex].Attachments.Length == 1)
                    {
                        Action("Файл обновлений обнаружен", MaxCount, CurrCount++);

                        lines.AppendLine(DateTime.Now.ToString() + " - обновление найдено");
                        Date = messages[LastDateIndex].Date.Value;

                        var str = messages[LastDateIndex].Attachments[0].GetTextData();

                        var Index = str.IndexOf(")IMAPX");
                        if (Index > -1) str = str.Substring(0, Index);

                        //messages[Mn].Attachments[0].Save(TempPath, FileName);
                        var Bytes = Convert.FromBase64String(str);

                        FilePath = Path.GetTempPath() + messages[LastDateIndex].Attachments[0].FileName;

                        using (var fs = new FileStream(FilePath, FileMode.Create))
                        { fs.Write(Bytes, 0, Bytes.Length); }
                    }
                    else
                    { Action("Файл обновлений не обнаружен", MaxCount, CurrCount++); }
                }
                else
                { Action("Обновления не обнаружены", MaxCount, CurrCount++); }

                Action("Закрытие почты", MaxCount, CurrCount++);
                lines.AppendLine(DateTime.Now.ToString() + " - закрытие почты");
                Client.Disconnect();

                Action("Почта закрыта", MaxCount, CurrCount++);

                return true;
            }
        }
    }
}
