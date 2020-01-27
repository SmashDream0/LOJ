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
using System.Threading;
using LaboratoryOnlineJournal;
using LaboratoryOnlineJournal.Synch;

/// <summary>Общие объекты управления, настройки программы и база данных</summary>
public static class data
{
    /// <summary>Параметры коммандной строки</summary>
    public struct CMD
    {
        /// <summary>Разрешено менять таблицы, если они не соответствуют ожиданиям</summary>
        public readonly static string AlowToChange = "alowtochange=";
        public readonly static string SettingsFile = "settingsfile=";
        public readonly static string SetIncrem = "setincrem=";
        public readonly static string DeleteMe = "deleteme=";
    }

    public static int LocationY = 50;
    public static int MinusLocationY = LocationY + 40;
    public static int ButtonsLocationY = 30;
    public static int ButtonsHeight = 22;
    public static int Divide = 3;
    public static int GridLocationY = ButtonsLocationY + ButtonsHeight + Divide;
    public static int MinimumHidedWidth = ButtonsHeight + Divide * 2;
    public static int MaximumMarkWidth = 300;

    public static double MinimumPercent = 0.65;

    public static byte PerMark = 4;
    public static byte PerPodr = 3;
    public static byte ForMark = 12;
    public static byte ForMethod = 5;
    public static byte ForHided = 1;

    public static float ForHidedP = 0.05f;

    public static DataBase T1;
    public static SynchPoolManager SynchPool;

    static uint _UserID;
    public static uint UserID
    {
        get { return _UserID; }
        set
        {
            if (PrgSettings != null)
            { _UserID = PrgSettings.Values[(int)Strings.LastUser].UInt = value; }
            else
            { _UserID = value; }
        }
    }

    /// <summary>Инкрементный множитель таблиц</summary>
    public static int Increm = -1;
    public static bool DeleteConf = false;
    public static string StName = "asettings";
    public static bool AllowModify;

    public static Settings PrgSettings;

    public static readonly string EmaleSubjectPart = "LOJ";
    public static readonly string EmaleSubject = "Обновление " + EmaleSubjectPart;
    public static readonly string SendEmaleSubject = EmaleSubject.Remove(EmaleSubject.Length - EmaleSubjectPart.Length-1, 1).ToLower();

    public enum Forms : byte { AdminPanel, Settings, Employe };
    public static DataBase.RemoteType DataSourceType { get { return (DataBase.RemoteType)PrgSettings.Values[(int)Strings.UseSQL].Int; } }

    public enum Strings : byte { UseSQL, SqlIp, SqlIp1, SqlIp2, SqlIpLast, SqlLogin, SqlPassword, DATABASE, SqlPort, DirectIMAPAdress, DirectIMAPPort, SMTPAdress, SMTPPort, DirectIMAPUseSSL, SMTPUseSSL, MailLogin, MailPass, LastUser, DirectMailLogin, DirectMailPass, DirectSMTPAdress, DirectSMTPPort, DirectSMTPCrypt, AutoCrypto, Changes };

    public enum UType : byte { None, Admin, Employe, MainEmploye, Union };
    public enum PnMean : byte { None, Nachalnic, Zam, ProcessEngineer, Other };
    //public enum Period : byte { None = 0, Once = 1, Week = 2, Month = 3, Quartal = 4, Year = 5 };
    public enum FDrctn : byte { None = 0, Outgo = 1, Income = 2 };
    public enum NType : byte { None = 0, Mark = 1, PodrV = 2, PodrK = 3, PodrAll = 4, Volume = 5 };
    public enum PSG : byte { None = 0, Default = 1, Income = 2, Outgo = 3 };
    public enum TResp : byte { None = 0, Material = 1, LaboratoryProtokol = 2, Sampler = 3 };
    public enum SGroup : byte
    {
        None = 0,
        /// <summary>Усредненный</summary>
        Middle1 = 1,
        /// <summary>Сводный</summary>
        Group1 = 2,
        /// <summary>Единичный</summary>
        NotGroup1 = 3,
        /// <summary>Атмосферный воздух</summary>
        Group2 = 5,
        /// <summary>Рабочая зона</summary>
        Group3 = 8,
        /// <summary>Колодец\\Шламонакопитель</summary>
        Middle4 = 10,
        /// <summary>Аква-аурат</summary>
        AquaAurat = 11,
        /// <summary>Коагулянт сульфат алюминия</summary>
        KOCA = 12,
        /// <summary>Токсичность1</summary>
        Toxicity1 = 13,

        /// <summary>Токсичность2</summary>
        Toxicity2 = 4,
        /// <summary>пустота 2, надо для индексной логики в Protocol_class</summary>
        none2 = 6,
        /// <summary>пустота 3, надо для индексной логики в Protocol_class</summary>
        none3 = 7,
        /// <summary>пустота 4, надо для индексной логики в Protocol_class</summary>
        none4 = 9,

    };
    public enum VarType : byte { None = 0, dbl = 1, i32 = 2, Bool = 3 };

    public static ColumnType User<ColumnType>(int ColumnIndex) { return T.User.Rows.Get_UnShow<ColumnType>(data.UserID, ColumnIndex); }

    public static ColumnType User<ColumnType>(int ColumnIndex, params int[] RelationPath) { return T.User.Rows.Get_UnShow<ColumnType>(data.UserID, ColumnIndex, RelationPath); }

    public static void User<ColumnType>(int ColumnIndex, ColumnType Value) { T.User.Rows.Set(data.UserID, ColumnIndex, Value); }

    /// <summary>Инкрементировать дату отправки</summary>
    public static void IncremUserSendDate()
    {
        if (data.User<int>(C.User.YMDSND) > 0 && ATMisc.GetYMDFromDateTime(DateTime.Now) >= data.User<int>(C.User.YMDSND))
        {
            var tmp = ATMisc.GetYMDFromDateTime(DateTime.Now) - data.User<int>(C.User.YMDSND);
            var nYMD = (tmp) % data.User<int>(C.User.Period);

            if (nYMD > 0)
            { tmp = (tmp - nYMD) + data.User<int>(C.User.Period); }
            else
            { tmp = tmp + data.User<int>(C.User.Period); }

            data.User<int>(C.User.YMDSND, data.User<int>(C.User.YMDSND) + tmp);
        }
    }
}

/// <summary>Таблицы</summary>
public static class T
{
    public static DataBase.ITable User, UType, Podr, PodrPpl, People, Prfssn, PnMean, EdType, OPType, Mark, MError, Sample, SM, SPoint, Adress, PType, SCause, PSG, Period, PMNorm, SMS, SPT, Area, Norm, NType, OLocation, Method/*, VGroup*/, MVolume, OType, Object, SPool, Prt, PrtS, OPT, PSGM, Resp, TResp, SGroup, BackGrd, PaPoS, UTable, VarType, TestCond, TCS;

    public static void Clear()
    {
        T.User = null;
        T.UType = null;
        T.Podr = null;
        T.PodrPpl = null;
        T.People = null;
        T.Prfssn = null;
        T.PnMean = null;
        T.EdType = null;
        T.OPType = null;
        T.Mark = null;
        T.MError = null;
        T.Sample = null;
        T.SM = null;
        T.SPoint = null;
        T.Adress = null;
        T.PType = null;
        T.SCause = null;
        T.PSG = null;
        T.Period = null;
        T.PMNorm = null;
        T.SMS = null;
        T.SPT = null;
        T.Area = null;
        T.Norm = null;
        T.NType = null;
        T.OLocation = null;
        T.Method = null;
        T.MVolume = null;
        T.OType = null;
        T.Object = null;
        T.SPool = null;
        T.Prt = null;
        T.PrtS = null;
        T.OPT = null;
        T.PSGM = null;
        T.Resp = null;
        T.TResp = null;
        T.SGroup = null;
        T.BackGrd = null;
        T.PaPoS = null;
        T.UTable = null;
        T.VarType = null;
        T.TestCond = null;
        T.TCS = null;
    }
}

/// <summary>Уникальные табличные представления</summary>
public static class G
{
    public static DataBase.ISTable User, UType, Podr, PodrPpl, People, Prfssn, PnMean, EdType, OPType, Mark, MError, Sample, SM, SMMiddle, SPoint, Adress, PType, SCause, PSG, Period, PMNorm, SMS, SPT, Area, Norm, NType, OLocation, Method/*, VGroup*/, MVolume, OType, Object, SPool, Prt, PrtS, OPT, PSGM, Resp, TResp, SGroup, BackGrd, PaPoS, UTable, VarType, TestCond, TCS;

    public static void Clear()
    {
        G.User = null;
        G.UType = null;
        G.Podr = null;
        G.PodrPpl = null;
        G.People = null;
        G.Prfssn = null;
        G.PnMean = null;
        G.EdType = null;
        G.OPType = null;
        G.Mark = null;
        G.MError = null;
        G.Sample = null;
        G.SM = null;
        G.SMMiddle = null;
        G.SPoint = null;
        G.Adress = null;
        G.PType = null;
        G.SCause = null;
        G.PSG = null;
        G.Period = null;
        G.PMNorm = null;
        G.SMS = null;
        G.SPT = null;
        G.Area = null;
        G.Norm = null;
        G.NType = null;
        G.OLocation = null;
        G.Method = null;
        G.MVolume = null;
        G.OType = null;
        G.Object = null;
        G.SPool = null;
        G.Prt = null;
        G.PrtS = null;
        G.OPT = null;
        G.PSGM = null;
        G.Resp = null;
        G.TResp = null;
        G.SGroup = null;
        G.BackGrd = null;
        G.PaPoS = null;
        G.UTable = null;
        G.VarType = null;
        G.TestCond = null;
        G.TCS = null;
    }
}

/// <summary>Колонки таблиц</summary>
public static class C
{
    public struct TestCond
    {
        /// <summary>Точка отбора</summary>
        public const byte SPoint = 0;
        /// <summary>Наименование</summary>
        public const byte Name = SPoint + 1;
        /// <summary>Единица измерений</summary>
        public const byte EdType = Name + 1;

        static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] AddInControls)
        {
            if (Values[Name] == null || ((string)Values[Name]).Length == 0)
            {
                MessageBox.Show("Поле \"" + T.TestCond.GetColumn(Name).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[EdType] == null || ((uint)Values[EdType]) == 0)
            {
                MessageBox.Show("Поле \"" + T.TestCond.GetColumn(EdType).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!VirtualMode && (Values[SPoint] == null || ((uint)Values[SPoint]) == 0))
            {
                MessageBox.Show("Поле \"" + T.TestCond.GetColumn(SPoint).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, ID, Width, Virtual);
            UEF.SetChecks(Checks);
            return UEF;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, Width, Virtual);
            UEF.SetChecks(Checks);
            return UEF;
        }
    }
    public struct TCS
    {
        /// <summary>Замер</summary>
        public const byte Sample = 0;
        /// <summary>Условие измерений</summary>
        public const byte TestCond = Sample + 1;
        /// <summary>Значение свойства</summary>
        public const byte Value = TestCond + 1;
        
        static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] AddInControls)
        {
            if (Values[Value] == null || ((string)Values[Value]).Length == 0)
            {
                MessageBox.Show("Поле \"" + T.TCS.GetColumn(Value).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!VirtualMode && (Values[Sample] == null || ((uint)Values[Sample]) == 0))
            {
                MessageBox.Show("Поле \"" + T.TCS.GetColumn(Sample).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[TestCond] == null || ((uint)Values[TestCond]) == 0)
            {
                MessageBox.Show("Поле \"" + T.TCS.GetColumn(TestCond).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, ID, Width, Virtual);
            UEF.SetChecks(Checks);
            return UEF;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, Width, Virtual);
            UEF.SetChecks(Checks);
            return UEF;
        }
    }
    /// <summary>Тип значения</summary>
    public struct VarType
    {
        /// <summary>Наиманование</summary>
        public const byte Name = 0;
    }
    /// <summary>Таблица выгрузки</summary>
    public struct UTable
    {
        /// <summary>Наиманование</summary>
        public const byte Name = 0;
        /// <summary>Разрешение на добавление</summary>
        public const byte Add = Name + 1;
        /// <summary>Разрешение на изменение</summary>
        public const byte Update = Add + 1;
        /// <summary>Разрешение на удаление</summary>
        public const byte Delete = Update + 1;
        /// <summary>Использование</summary>
        public const byte Use = Delete + 1;
    }

    public struct BackGrd
    {
        /// <summary>Наиманование</summary>
        public const byte Name = 0;
    }
    /// <summary>План и место отбора образцов</summary>
    public struct PaPoS
    {
        /// <summary>Наиманование</summary>
        public const byte Name = 0;
    }
    public struct SGroup
    {
        /// <summary>Наиманование</summary>
        public const byte Name = 0;
        /// <summary>Наиманование</summary>
        public const byte ShrName = Name + 1;
    }

    /// <summary>Метод для группы складов, а так же ответственный за него</summary>
    public struct PSGM
    {
        /// <summary>Месяц начала использования</summary>
        public const byte YM = 0;
        /// <summary>Группа складов</summary>
        public const byte PSG = YM + 1;
        /// <summary>Имя метода</summary>
        public const byte Method = PSG + 1;
        /// <summary>Ответственный</summary>
        public const byte People = Method + 1;
    }
    /// <summary>Ответственность сотрудника подразделения</summary>
    public struct Resp
    {
        /// <summary>Сотрудник подразделения</summary>
        public const byte PodrPpl = 0;
        /// <summary>Тип ответственности</summary>
        public const byte TResp = PodrPpl + 1;
    }
    public struct TResp
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
    }

    /// <summary>Тип объекта</summary>
    public struct OType
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
    }
    /// <summary>Пользователь</summary>
    public struct User
    {
        /// <summary>Имя учетной записи</summary>
        public const byte Login = 0;
        /// <summary>Пароль</summary>
        public const byte Pass = Login + 1;
        /// <summary>Тип учетной записи</summary>
        public const byte UType = Pass + 1;
        /// <summary>Имя компьютера</summary>
        public const byte PCName = UType + 1;
        /// <summary>Имя пользователя учетки компьютера</summary>
        public const byte PCUser = PCName + 1;
        /// <summary>Используется-ли учетная запись</summary>
        public const byte IsHere = PCUser + 1;
        /// <summary>Электронная почта</summary>
        public const byte Mail = IsHere + 1;
        /// <summary>Текущй период</summary>
        public const byte YM = Mail + 1;
        /// <summary>Принадлежность к подразделению</summary>
        public const byte Podr = YM + 1;
        /// <summary>Точность расчетов</summary>
        public const byte Round = Podr + 1;

        /// <summary>Ширина колонки точки отбора 1</summary>
        public const byte SPC1 = Round + 1;
        /// <summary>Ширина колонки точки отбора 2</summary>
        public const byte SPC2 = SPC1 + 1;
        /// <summary>Ширина колонки точки отбора 3</summary>
        public const byte SPC3 = SPC2 + 1;
        /// <summary>Ширина колонки точки отбора 4</summary>
        public const byte SPC4 = SPC3 + 1;
        /// <summary>Ширина колонки точки отбора 5</summary>
        public const byte SPC5 = SPC4 + 1;
        /// <summary>Ширина колонки точки отбора 6</summary>
        public const byte SPC6 = SPC5 + 1;

        /// <summary>Ширина колонки замера 1</summary>
        public const byte SC1 = SPC6 + 1;
        /// <summary>Ширина колонки замера 2</summary>
        public const byte SC2 = SC1 + 1;
        /// <summary>Ширина колонки замера 3</summary>
        public const byte SC3 = SC2 + 1;
        /// <summary>Ширина колонки замера 4</summary>
        public const byte SC4 = SC3 + 1;
        /// <summary>Ширина колонки замера 5</summary>
        public const byte SC5 = SC4 + 1;
        /// <summary>Ширина колонки замера 6</summary>
        public const byte SC6 = SC5 + 1;
        /// <summary>Ширина колонок замера - показатели</summary>
        public const byte SCM = SC6 + 1;

        /// <summary>Высота записей точек отбора</summary>
        public const byte SpntH = SCM + 1;

        /// <summary>Высота записей замеров</summary>
        public const byte SplH = SpntH + 1;

        /// <summary>Дистанция разделения</summary>
        public const byte SplDist = SplH + 1;

        /// <summary>открыто</summary>
        public const byte ok1 = SplDist + 1;

        /// <summary>закрыто</summary>
        public const byte ck1 = ok1 + 1;

        /// <summary>Создавать новый протокол</summary>
        public const byte CNP = ck1 + 1;

        /// <summary>День отправки</summary>
        public const byte YMDSND = CNP + 1;
        /// <summary>Период отправки</summary>
        public const byte Period = YMDSND + 1;
        /// <summary>Разрешение на отправку/приём</summary>
        public const byte AlowToGSU = Period + 1;
        /// <summary>Обновлять всем/только центру</summary>
        public const byte UAll = AlowToGSU + 1;
        /// <summary>Период поиска обновлений</summary>
        public const byte UP = UAll + 1;

        /// <summary>Разрешено-ли использовать учетную запись</summary>
        public const byte Enabled = UP + 1;
        /// <summary>Описание причины блокировки</summary>
        public const byte Cause = Enabled + 1;

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, ID, Width, Virtual);

            var Enabled = T.User.Rows.Get_UnShow<uint>(data.UserID, C.User.UType) == (uint)data.UType.Admin;

            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Login));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Pass, true, null, true));
            UEF.AddControl(new Edit_Form.RelationCombo_class(UEF, C.User.UType, Enabled));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Podr, Enabled));
            UEF.AddControl(new Edit_Form.MonthesInt32_class(UEF, C.User.YM, Enabled));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Mail));
            UEF.AddControl(new Edit_Form.DaysInt32_class(UEF, C.User.YMDSND, false));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Period));

            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.AlowToGSU, Enabled));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.UAll, Enabled));

            if (Enabled)
            {
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.ok1));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.ck1));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.UP));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.PCName));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.PCUser));
            }

            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.IsHere, Enabled));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Enabled, Enabled));

            return UEF;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var UEF = new Edit_Form(SubTable, Width, Virtual);

            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Login));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Pass));
            UEF.AddControl(new Edit_Form.RelationCombo_class(UEF, C.User.UType));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Podr));
            UEF.AddControl(new Edit_Form.MonthesInt32_class(UEF, C.User.YM, false));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Mail));

            UEF.AddControl(new Edit_Form.DaysInt32_class(UEF, C.User.YMDSND, false));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Period));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.AlowToGSU));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.UAll));

            if (T.User.Rows.Get_UnShow<uint>(data.UserID, C.User.UType) == (uint)data.UType.Admin)
            {
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.ok1));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.ck1));
                UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.UP));
            }

            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.IsHere));
            UEF.AddControl(new Edit_Form.Inputs_class(UEF, C.User.Enabled));

            return UEF;
        }
    }
    /// <summary>Протокол испытаний</summary>
    public struct Prt
    {
        /// <summary>Месяц</summary>
        public const byte YM = 0;
        /// <summary>Номер протокола</summary>
        public const byte Time = YM + 1;
        /// <summary>Номер протокола</summary>
        public const byte Number = Time + 1;
        /// <summary>Объединение</summary>
        public const byte Union = Number + 1;
        /// <summary>Место</summary>
        public const byte OLocation = Union + 1;
        /// <summary>Подразделение</summary>
        public const byte Podr = OLocation + 1;
        /// <summary>Район</summary>
        public const byte Area = Podr + 1;
        /// <summary>Объект</summary>
        public const byte Object = Area + 1;
        /// <summary>Количество замеров</summary>
        public const byte SCount = Object + 1;
        /// <summary>Группировка</summary>
        public const byte SGroup = SCount + 1;
        /// <summary>План и место отбора</summary>
        public const byte PaPoS = SGroup + 1;
        /// <summary>Акт отбора</summary>
        public const byte Taos = PaPoS + 1;
        /// <summary>День</summary>
        public const byte Day = Taos + 1;
    }

    public struct PrtS
    {
        /// <summary>Протокол</summary>
        public const byte Prt = 0;
        /// <summary>Замер</summary>
        public const byte Sample = Prt + 1;
    }

    public struct Area
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
    }
    /// <summary>Показание выпуска</summary>
    public struct MVolume
    {
        /// <summary>Место</summary>
        public const byte OLocation = 0;
        /// <summary>Месяц</summary>
        public const byte YM = OLocation + 1;
        /// <summary>Объём</summary>
        public const byte Volume = YM + 1;

        public static Form GetEdit(DataBase.ISTable Table, uint ID, int Width, bool Virtual)
        {
            var MVEF = new Edit_Form(Table, ID, Width, Virtual);

            MVEF.AddControl(C.MVolume.OLocation);
            MVEF.AddControl(new Edit_Form.MonthesInt32_class(MVEF, C.MVolume.YM, false));
            MVEF.AddControl(C.MVolume.Volume);

            return MVEF;
        }

        public static Form GetEdit(DataBase.ISTable Table, int Width, bool Virtual)
        {
            var MVEF = new Edit_Form(Table, Width, Virtual);

            MVEF.AddControl(C.MVolume.OLocation);
            MVEF.AddControl(new Edit_Form.MonthesInt32_class(MVEF, C.MVolume.YM, false));
            MVEF.AddControl(C.MVolume.Volume);

            return MVEF;
        }
    }

    public struct MsrdNorm
    {
        public const byte Name = 0;
        public const byte Norm = Name + 1;
    }

    /// <summary>Метод</summary>
    public struct Method
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Норма</summary>
        public const byte Norm = Name + 1;
        /// <summary>Показатель</summary>
        public const byte Mark = Norm + 1;
    }

    /// <summary>Показатель к точке отбора</summary>
    public struct SMS
    {
        /// <summary>Точка отбора</summary>
        public const byte SPoint = 0;
        /// <summary>Показатель</summary>
        public const byte Mark = SPoint + 1;


    }
    /// <summary>Группа складов</summary>
    public struct PSG
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
    }
    /// <summary>Направление потока</summary>
    public struct FDrctn
    {
        public const byte Name = 0;
    }
    /// <summary>Точки отбора</summary>
    public struct SPoint
    {
        /// <summary>Номер</summary>
        public const byte Number = 0;
        /// <summary>Положение</summary>
        public const byte Name = Number + 1;
        /// <summary>Район</summary>
        public const byte Area = Name + 1;
        /// <summary>Точка отбора является фоновой</summary>
        public const byte BckGnd = Area + 1;
        /// <summary>Применять фон к этой точке отбора</summary>
        public const byte UsBckGnd = BckGnd + 1;
        /// <summary>Тип пробы</summary>
        public const byte PType = UsBckGnd + 1;
        /// <summary>Объект</summary>
        public const byte Object = PType + 1;
        /// <summary>Подразделение</summary>
        public const byte Podr = Object + 1;
        /// <summary>Начало периода</summary>
        public const byte YMDS = Podr + 1;
        /// <summary>Окончание периода</summary>
        public const byte YMDE = YMDS + 1;
        /// <summary>Игнорировать список показателей</summary>
        public const byte IMLst = YMDE + 1;
        /// <summary>Тип протокола</summary>
        public const byte SGroup = IMLst + 1;
        /// <summary>Номер группировки</summary>
        public const byte SGNum = SGroup + 1;
        /// <summary>Группа фона этой точки отбора</summary>
        public const byte BackGrd = SGNum + 1;
        /// <summary>Объединено</summary>
        public const byte Union = BackGrd + 1;
        /// <summary>План и место отбора</summary>
        public const byte PaPoS = Union + 1;

        public static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] IAICChecks)
        {
            if (Values[Name] == null || ((string)Values[Name]).Length == 0)
            {
                MessageBox.Show("Значение поля \"" + T.SPoint.GetColumn(Name).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            /*if (Values[CountpP] == null || ((int)Values[CountpP]) == 0)
            {
                MessageBox.Show("Поле " + T.SPoint.GetColumn(CountpP).AlterName + " не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }*/

            if (Values[Podr] == null || ((uint)Values[Podr]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.SPoint.GetColumn(Podr).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[Object] == null || ((uint)Values[Object]) == 0)
            {
                MessageBox.Show("Поле " + T.SPoint.GetColumn(Object).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[UsBckGnd] != null && Values[BckGnd] != null && (bool)Values[UsBckGnd] && (bool)Values[BckGnd])
            {
                MessageBox.Show("Нельзя одновременно задействовать настройки \"" + T.SPoint.GetColumn(UsBckGnd).AlterName + "\" и \"" + T.SPoint.GetColumn(BckGnd).AlterName + "\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[BackGrd] != null && ((uint)Values[BackGrd]) != 0 && Values[UsBckGnd] == null && !(bool)Values[UsBckGnd] && Values[BckGnd] == null && !(bool)Values[BckGnd])
            {
                MessageBox.Show("При активированной настройке " + T.SPoint.GetColumn(BackGrd).AlterName + "\" должно бы быть отмечено " + T.SPoint.GetColumn(UsBckGnd).AlterName + " или " + T.SPoint.GetColumn(BckGnd).AlterName, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[PaPoS] == null || ((uint)Values[PaPoS]) == 0)
            {
                MessageBox.Show("Поле " + T.SPoint.GetColumn(PaPoS).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[SGroup] == null || ((uint)Values[SGroup]) == 0)
            {
                MessageBox.Show("Необходимо указать тип группировки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (T.Object.Rows.Get<bool>((uint)Values[Object], C.Object.OLocationFrom, C.OLocation.Volumed))
            {
                if (ID == 0)
                {
                    if ((bool)G.SPoint.QUERRY().EXIST.WHERE.C(C.SPoint.Object, ((uint)Values[Object])).DO()[0].Value)
                    {
                        MessageBox.Show("Указанный объект исследования уже используется в другой точке отбора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    Values[Number] = G.SPoint.QUERRY().COUNT.DO()[0].Value;
                }
                else
                {
                    if ((bool)G.SPoint.QUERRY().EXIST.WHERE.C(C.SPoint.Object, ((uint)Values[Object])).AND.NOT.ID(ID).DO()[0].Value)
                    {
                        MessageBox.Show("Указанный объект исследования уже используется в другой точке отбора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }

            if ((data.PSG)T.Podr.Rows.Get_UnShow<uint>((uint)Values[Podr], C.Podr.PSG) == data.PSG.Income && (data.NType)T.Object.Rows.Get_UnShow<uint>((uint)Values[Object], C.Object.Norm, C.Norm.NType) == data.NType.PodrK)
            {
                MessageBox.Show("Норма типа \"" + T.NType.Rows.Get<string>((uint)data.NType.PodrK, C.NType.Name) + "\" не соответствует группе подразделений \"" + T.PSG.Rows.Get<string>((uint)data.PSG.Income, C.PSG.Name) + "\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if ((data.PSG)T.Podr.Rows.Get_UnShow<uint>((uint)Values[Podr], C.Podr.PSG) == data.PSG.Outgo && (data.NType)T.Object.Rows.Get_UnShow<uint>((uint)Values[Object], C.Object.Norm, C.Norm.NType) == data.NType.PodrV)
            {
                MessageBox.Show("Норма типа \"" + T.NType.Rows.Get<string>((uint)data.NType.PodrV, C.NType.Name) + "\" не соответствует группе подразделений \"" + T.PSG.Rows.Get<string>((uint)data.PSG.Outgo, C.PSG.Name) + "\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (IAICChecks.Length != 2)
            { throw new Exception("Не верное количество связанных блоков"); }

            Edit_Form.IAddInComponent AI = null, TC = null;

            for (int i = 0; i < IAICChecks.Length; i++ )
            {
                if (IAICChecks[i].Table.Parent == T.SMS)
                { AI = IAICChecks[i]; }
                else if (IAICChecks[i].Table.Parent == T.TestCond)
                { TC = IAICChecks[i]; }
            }

            if (AI == null)
            { throw new Exception("Не обнаружена табличная часть \"" + T.SMS.AlterName + "\""); }

            if (TC == null)
            { throw new Exception("Не обнаружена табличная часть \"" + T.TestCond.AlterName + "\""); }

            for (int i = 0; i < AI.Count; i++)
            {
                var MID = (uint)AI.GetValue(i, C.SMS.Mark);

                var Norm = RCache.Marks[MID].GetNorm(T.Object.Rows.Get_UnShow<uint>((uint)Values[Object], C.Object.Norm));

                if (Norm.MethodName.Length == 0)
                {
                    MessageBox.Show("У показателя \"" + RCache.Marks[MID].Name + "\", нет метода в норме \"" + T.Object.Rows.Get<string>((uint)Values[Object], C.Object.Norm, C.Norm.Name) + "\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                /*if (Norm.Ranges.Count == 0)
                {
                    MessageBox.Show("У показателя \"" + RCache.Marks[MID].Name + "\", нет точности в норме \"" + T.Object.Rows.Get<string>((uint)Values[Object], C.Object.Norm, C.Norm.Name) + "\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }*/

                for (int j = i + 1; j < AI.Count; j++)
                { 
                    if (MID == (uint)AI.GetValue(j, C.SMS.Mark))
                    {
                        MessageBox.Show("Показатели не должны дублироваться", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }

            return true;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var SPED = new Edit_Form(SubTable, Width, Virtual);
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Number, false, (int)SubTable.QUERRY().COUNT.DO()[0].Value));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Name));

            {
                var ymds = new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDS, false);
                var ymde = new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDE, true);
                //var period = new Edit_Form.RelationCombo_class(SPED, C.SPoint.Period);
                //var ymd = new YMDP_class(SPED, ymds, ymde, period);

                //SPED.AddControl(ymd);
                SPED.AddControl(ymds);
                SPED.AddControl(ymde);
                //SPED.AddControl(period);
            }

            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Area));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Object));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BckGnd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.UsBckGnd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.SGroup));
            //SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.VGroup));
            //SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.CountpP));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Podr));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.IMLst));

            SPED.AddSubEdit(G.SMS);

            return SPED;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var SPED = new Edit_Form(SubTable, ID, Width, Virtual);
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Number, false, (int)SubTable.QUERRY().COUNT.DO()[0].Value));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Name));

            {
                var ymds = new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDS, false);
                var ymde = new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDE, true);
                //var period=new Edit_Form.RelationCombo_class(SPED, C.SPoint.Period);
                //var ymd = new YMDP_class(SPED, ymds, ymde, period);

                //SPED.AddControl(ymd);
                SPED.AddControl(ymds);
                SPED.AddControl(ymde);
                //SPED.AddControl(period);
            }

            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Area));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Object));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BckGnd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.UsBckGnd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.SGroup));
            //SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.VGroup));
            //SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.CountpP));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Podr));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.IMLst));

            SPED.AddSubEdit(G.SMS);

            return SPED;
        }
    }

    /// <summary>Место</summary>
    public struct OLocation
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Краткое наименование</summary>
        public const byte ShrName = Name + 1;
        /// <summary>Использовать объём</summary>
        public const byte Volumed = ShrName + 1;
    }
    /// <summary>Объект</summary>
    public struct Object
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Тип объекта</summary>
        public const byte OType = Name + 1;
        /// <summary>Источник, первичен, при определении норматива и выпуска</summary>
        public const byte OLocationFrom = OType + 1;
        /// <summary>Сброс, важен для определения фона</summary>
        public const byte OLocationTo = OLocationFrom + 1;
        /// <summary>Норматив</summary>
        public const byte Norm = OLocationTo + 1;
    }
    /// <summary>Норма к подразделению</summary>
    public struct PMNorm
    {
        /// <summary>Норма</summary>
        public const byte Norm = 0;
        /// <summary>Показатель</summary>
        public const byte Mark = Norm + 1;
        /// <summary>Подразделение</summary>
        public const byte Podr = Mark + 1;
        /// <summary>Подразделение</summary>
        public const byte OLocation = Podr + 1;
        /// <summary>От</summary>
        public const byte LFrom = OLocation + 1;
        /// <summary>До</summary>
        public const byte LTo = LFrom + 1;
        /// <summary>Использовать фон</summary>
        //public const byte BckGrnd = 5;
    }
    /// <summary>Склады</summary>
    public struct Podr
    {
        /// <summary>Группа</summary>
        public const byte PSG = 0;
        /// <summary>Краткое наименование</summary>
        public const byte ShrName = PSG + 1;
        /// <summary>Полное наименование</summary>
        public const byte FllName = ShrName + 1;
        /// <summary>Адрес</summary>
        public const byte Contact = FllName + 1;
        /// <summary>Подразделение-связь(в лево)</summary>
        public const byte PFrom = Contact + 1;
        /// <summary>Почта</summary>
        public const byte ShowP = PFrom + 1;
        /// <summary>Положение в окне по ширине</summary>
        public const byte Xloc = ShowP + 1;
        /// <summary>Положение в окне по высоте</summary>
        public const byte Yloc = Xloc + 1;

        static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] AddInControls)
        {
            if (Values[C.Podr.FllName] == null || ((string)Values[C.Podr.FllName]).Length == 0)
            {
                MessageBox.Show("Полное наименование не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[C.Podr.ShrName] == null || ((string)Values[C.Podr.ShrName]).Length == 0)
            {
                MessageBox.Show("Краткое наименование не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public static Edit_Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            //полное наименование
            //краткое наименование
            //
            //
            //

            var EFP = new Edit_Form(SubTable, Width, Virtual);

            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.ShrName));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.FllName));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.Contact));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.ShowP));
            EFP.AddControl(new Edit_Form.RelationCombo_class(EFP, C.Podr.PSG));

            EFP.SetChecks(Checks);

            EFP.AddSubEdit(G.PodrPpl, 200, null
                , new Edit_Form.SubEdit_class.Columns_struct[]
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.Prfssn)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name1)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name2)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name3)
                    });

            return EFP;
        }
        public static Edit_Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            //полное наименование
            //краткое наименование
            //
            //
            //
            var EFP = new Edit_Form(SubTable, ID, Width, Virtual);

            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.ShrName));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.FllName));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.Contact));
            EFP.AddControl(new Edit_Form.Inputs_class(EFP, C.Podr.ShowP));
            EFP.AddControl(new Edit_Form.RelationCombo_class(EFP, C.Podr.PSG));

            EFP.SetChecks(Checks);

            EFP.AddSubEdit(G.PodrPpl, 200, null
                , new Edit_Form.SubEdit_class.Columns_struct[]
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.Prfssn)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name1)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name2)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.PodrPpl.People, C.People.name3)
                    });

            return EFP;
        }
    }
    /// <summary>Сотрудники в подразделении</summary>
    public struct PodrPpl
    {
        /// <summary>Подразделение</summary>
        public const byte Podr = 0;
        /// <summary>Сотрудник</summary>
        public const byte People = Podr + 1;

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var PED = new Edit_Form(SubTable, ID, Width, Virtual);

            PED.AddControl(new Edit_Form.Inputs_class(PED, C.PodrPpl.People));
            PED.AddControl(new Edit_Form.Inputs_class(PED, C.PodrPpl.Podr, (data.UType)data.User<uint>(C.User.UType) == data.UType.Admin));

            PED.AddSubEdit(G.Resp);

            return PED;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var PED = new Edit_Form(SubTable, Width, Virtual);

            PED.AddControl(new Edit_Form.Inputs_class(PED, C.PodrPpl.People));
            PED.AddControl(new Edit_Form.Inputs_class(PED, C.PodrPpl.Podr, (data.UType)data.User<uint>(C.User.UType) == data.UType.Admin, data.User<uint>(C.User.Podr)));

            PED.AddSubEdit(G.Resp);

            return PED;
        }
    }
    /// <summary>Профессия</summary>
    public struct Prfssn
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Значение</summary>
        public const byte PnMean = Name + 1;
    }
    /// <summary>Сотрудники</summary>
    public struct People
    {
        /// <summary>Фамилия</summary>
        public const byte name1 = 0;
        /// <summary>Имя</summary>
        public const byte name2 = name1 + 1;
        /// <summary>Отчество</summary>
        public const byte name3 = name2 + 1;
        /// <summary>Профессия</summary>
        public const byte Prfssn = name3 + 1;
    }
    /// <summary>Замер</summary>
    public struct Sample
    {
        /// <summary>Точка отбора</summary>
        public const byte SPoint = 0;
        /// <summary>Номер</summary>
        public const byte Number = SPoint + 1;
        /// <summary>Порядок создания(по сути порядок сортировки)</summary>
        public const byte Loc = Number + 1;
        /// <summary>День замера</summary>
        public const byte CYMD = Loc + 1;
        /// <summary>День анализа</summary>
        public const byte AYMD = CYMD + 1;
        /// <summary>Пробоотборщик</summary>
        public const byte Resp = AYMD + 1;
        /// <summary>Причина отбора</summary>
        public const byte SCause = Resp + 1;

        public static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] AddInControls)
        {
            if (Values[AYMD] == null || (int)Values[AYMD] == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(AYMD).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[CYMD] == null || (int)Values[CYMD] == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(CYMD).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[SPoint] == null || ((uint)Values[SPoint]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(SPoint).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[Number] != null && (int)Values[Number] != 0) //если номер пустой, то замер не учитывается
            {
                /*MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(Number).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;*/

                {

                    int StartDay = ATMisc.GetYMDFromYM(ATMisc.GetYMFromYMD((int)Values[AYMD])) - 1
                      , EndDay = ATMisc.GetYMDFromYM(ATMisc.GetYMFromYMD((int)Values[AYMD]) + 1);

                    int COUNT = 0;

                    if (T.SPoint.Rows.Get<bool>((uint)Values[SPoint], C.SPoint.Union))
                    {
                        COUNT = (int)G.Sample.QUERRY()
                              .COUNT.WHERE
                              .AC(C.Sample.CYMD).Less.BV(EndDay)
                              .AND.AC(C.Sample.CYMD).More.BV(StartDay)
                              .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                              .AND.C(C.Sample.Number, (int)Values[Number]).DO()[0].Value;
                    }
                    else
                    {
                        COUNT = (int)G.Sample.QUERRY()
                            .COUNT.WHERE
                            .AC(C.Sample.CYMD).Less.BV(EndDay)
                            .AND.AC(C.Sample.CYMD).More.BV(StartDay)
                            .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(T.SPoint.Rows.Get_UnShow<uint>((uint)Values[SPoint], C.SPoint.Podr))
                            .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                            .AND.C(C.Sample.Number, (int)Values[Number]).DO()[0].Value;
                    }

                    int OldNumber = T.Sample.Rows.Get<int>(ID, C.Sample.Number);

                    if ((ID > 0
                        &&
                        ((int)Values[Number] == OldNumber && COUNT > 1 ||
                        (int)Values[Number] != OldNumber && COUNT > 0))
                        ||
                        ID == 0 && COUNT > 0)
                    {
                        MessageBox.Show("Указанный номер уже используется в отборе этого месяца", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            if (Values[CYMD] == null || ((int)Values[CYMD]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(CYMD).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[AYMD] == null || ((int)Values[AYMD]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(AYMD).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDS) > ((int)Values[CYMD]) ||
                T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDE) > 0 && T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDE) < ((int)Values[CYMD]))
            {
                MessageBox.Show("Дата отбора проб должна попадать в диапазон использования точки отбора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDS) > ((int)Values[AYMD]) ||
                T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDE) > 0 && T.SPoint.Rows.Get<int>((uint)Values[SPoint], C.SPoint.YMDE) < ((int)Values[AYMD]))
            {
                MessageBox.Show("Дата проведения анализа должна попадать в диапазон использования точки отбора", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[Resp] == null || ((uint)Values[Resp]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(Resp).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (T.Resp.Rows.Get_UnShow<uint>(((uint)Values[Resp]), C.Resp.TResp) != (uint)data.TResp.Sampler)
            {
                MessageBox.Show("В поле \"" + T.Sample.GetColumn(Resp).AlterName + "\" указан сотрудник, с не правильной ответственностью", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[SCause] == null || ((uint)Values[SCause]) == 0)
            {
                MessageBox.Show("Значение поля \"" + T.Sample.GetColumn(SCause).AlterName + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            /*if (AddInControls.Length != 2)
            { throw new Exception("Не верное количество табличных частей"); }

            if (AddInControls.Length == 1)
            {
                for (int i = 0; i < AddInControls[0].Count; i++)
                {
                    var Value = AddInControls[0].GetValue(i, C.TCS.Value);

                    if (Value == null || ((string)Value).Length == 0)
                    {
                        MessageBox.Show("Поле \"" + T.TCS.GetColumn(C.TCS.Value).AlterName + "\" свойства \"" + T.TestCond.Rows.Get<string>((uint)AddInControls[0].GetValue(i, C.TCS.TestCond), C.TestCond.Name)  + "\" не должно быть пустым", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }*/

            return true;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var MEF = new Edit_Form(SubTable, Width, Virtual);

            MEF.AddControl(new Edit_Form.Inputs_class(MEF, SPoint));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Loc));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Number));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, CYMD));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, AYMD));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, Resp, G.Resp));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, SCause));

            MEF.SetChecks(Checks);
            //MEF.AddSubEdit(G.SM);
            MEF.AddSubEdit(new TCSSubEdit_class(0, true, ((data.UType)data.User<uint>(C.User.UType) == data.UType.Admin) ? AutoForm.ShowType.Admin : AutoForm.ShowType.User));
            MEF.AddSubEdit(G.SM);

            return MEF;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var MEF = new Edit_Form(SubTable, ID, Width, Virtual);

            MEF.AddControl(new Edit_Form.Inputs_class(MEF, SPoint));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Loc));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Number));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, CYMD));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, AYMD));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, Resp, G.Resp));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, SCause));

            MEF.SetChecks(Checks);
            //MEF.AddSubEdit(G.SM);
            MEF.AddSubEdit(new TCSSubEdit_class(T.Sample.Rows.Get_UnShow<uint>(ID, C.Sample.SPoint), true, ((data.UType)data.User<uint>(C.User.UType) == data.UType.Admin) ? AutoForm.ShowType.Admin : AutoForm.ShowType.User));
            MEF.AddSubEdit(G.SM);

            return MEF;
        }

        public class TCSSubEdit_class:Edit_Form.SubEdit_class
        {
            public TCSSubEdit_class(uint SPointID, bool AlowToEdit, AutoForm.ShowType ShowType)
                : base(AlowToEdit, ShowType, T.Sample, G.TCS, 200, false, null, new Columns_struct[]
                {
                    new Columns_struct(C.TCS.TestCond, C.TestCond.Name),
                    new Columns_struct(C.TCS.Value)
                })
            {
                this.TestCond = T.TestCond.CreateSubTable(false);
                this.AlowToEdit = AlowToEdit;

                this.InitialAction = (ID) =>
                {
                    if (this.ShowItems.Count > 0)
                    {
                        this.Table.QUERRY().SHOW.WHERE.C(C.TCS.Sample, this.ID).DO();

                        for (int i = 0; i < this.Table.Rows.Count; i++)
                        {
                            var TCIndex = this.TestCond.Rows.GetIndex(this.Table.Rows.Get_UnShow<uint>(i, C.TCS.TestCond));

                            if (TCIndex > -1)
                            {
                                this.AllItems[TCIndex] = this.ShowItems[TCIndex] = new Item_class(this, this.Table.Rows.GetID(i));
                            }
                        }
                    }

                    return false;
                };

                this.Grid.ReadOnly = false;
                this.Delete_button.Enabled = false;
                this.Add_button.Enabled = false;

                this.Grid.CellBeginEdit += Grid_CellBeginEdit;
                this.Grid.CellValuePushed += Grid_CellValuePushed;
                this.Grid.CellDoubleClick -= this.Grid_CellDoubleClick;

                this.SetSPID(SPointID);
            }

            public void SetSPID(uint SPointID)
            {
                if (SPointID > 0)
                {
                    this.TestCond.QUERRY().SHOW.WHERE.C(C.TestCond.SPoint, SPointID).DO();

                    for (int i = 0; i < TestCond.Rows.Count; i++)
                    {
                        var Values = new object[T.TestCond.Columns.Count];
                        Values[C.TCS.TestCond] = TestCond.Rows.GetID(i);

                        var Item = new Item_class(this, Values);
                        this.ShowItems.Add(Item);
                        this.AllItems.Add(Item);
                    }
                }
                else
                { this.Enabled = false; }

                this.SPointID = SPointID;
            }

            public void SetSID(uint SampleID)
            {
                if (this.ShowItems.Count > 0)
                {
                    this.Table.QUERRY().SHOW.WHERE.C(C.TCS.Sample, SampleID).DO();

                    for (int i = 0; i < this.Table.Rows.Count; i++)
                    {
                        var TCIndex = this.TestCond.Rows.GetIndex(this.Table.Rows.Get_UnShow<uint>(i, C.TCS.TestCond));

                        if (TCIndex > -1)
                        { this.ShowItems[TCIndex].SetValue(C.TCS.Value, this.Table.Rows.Get<string>(i, C.TCS.Value)); }
                    }
                }
            }

            DataBase.ISTable TestCond;
            uint SPointID;
            bool AlowToEdit;

            struct gridColumns
            {
                public const byte TestCond = 0;
                public const byte Value = TestCond + 1;
            }

            private void Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
            {
                if (ShowItems[e.RowIndex].ID > 0 || AlowToEdit)
                {
                    switch (e.ColumnIndex)
                    {
                        case gridColumns.Value:
                            e.Cancel = false;
                            return;
                    }
                }

                e.Cancel = true;
            }
            
            private void Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
            {
                if (this.Enabled)
                {
                    switch (e.ColumnIndex)
                    {
                        case gridColumns.Value:
                            if (e.Value != null)
                            {
                                var Value = (string)e.Value;

                                if (Value.Length > T.TCS.GetColumn(C.TCS.Value).Length)
                                { Value = Value.Substring(0, T.TCS.GetColumn(C.TCS.Value).Length); }

                                ShowItems[e.RowIndex].SetValue(C.TCS.Value, Value); 
                            }
                            break;
                    }
                }
            }
        }
    }
    /// <summary>Показатели</summary>
    public struct Mark
    {
        /// <summary>Кодовый номер</summary>
        public const byte Code = 0;
        /// <summary>Наименование</summary>
        public const byte Name = Code + 1;
        /// <summary>Округление</summary>
        public const byte Round = Name + 1;
        /// <summary>Экспотенциальный вывод</summary>
        public const byte Exp = Round + 1;
        /// <summary>Тип значения</summary>
        public const byte VarType = Exp + 1;
        /// <summary>Единица измерений ввода</summary>
        public const byte EdType = VarType + 1;
        /// <summary>Единица измерений вывода</summary>
        public const byte OPType = EdType + 1;
        /// <summary>Порядковый номер</summary>
        public const byte Number = OPType + 1;
        /// <summary>Показывать всегда, даже если нет значения</summary>
        public const byte ShowZr = Number + 1;

        public static bool Checks(bool VirtualMode, uint ID, object[] Values, Edit_Form.IAddInComponent[] AddInControls)
        {
            if (Values[Name] == null || ((string)Values[Name]).Length == 0)
            {
                MessageBox.Show("Наименование не должно быть пустым.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (Values[Number] == null || ((byte)Values[Number]) == 0)
            {
                MessageBox.Show("Нужно указать номер.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if ((ID == 0 && (int)G.Mark.QUERRY().COUNT.WHERE.C(C.Mark.Number, (byte)Values[Number]).DO()[0].Value > 0) ||
                     (ID > 0 && (int)G.Mark.QUERRY().COUNT.WHERE.C(C.Mark.Number, (byte)Values[Number]).DO()[0].Value > 1))
            {
                MessageBox.Show("Номер не уникален.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Values[EdType] == null || ((uint)Values[EdType]) == 0)
            {
                MessageBox.Show("Нужно выбрать единицу измерений.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, int Width, bool Virtual)
        {
            var MEF = new Edit_Form(SubTable, Width, Virtual);

            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Code));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Name));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, VarType));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, EdType));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, OPType));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Number));

            MEF.SetChecks(Checks);

            MEF.AddSubEdit(G.MError);

            return MEF;
        }

        public static Form GetEdit(DataBase.ISTable SubTable, uint ID, int Width, bool Virtual)
        {
            var MEF = new Edit_Form(SubTable, ID, Width, Virtual);

            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Code));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Name));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, VarType));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, EdType));
            MEF.AddControl(new Edit_Form.RelationCombo_class(MEF, OPType));
            MEF.AddControl(new Edit_Form.Inputs_class(MEF, Number));

            MEF.SetChecks(Checks);

            MEF.AddSubEdit(G.MError);

            return MEF;
        }
    }
    /// <summary>Точность</summary>
    public struct MError
    {
        /// <summary>Показатель</summary>
        public const byte Norm = 0;
        /// <summary>Показатель</summary>
        public const byte Mark = Norm + 1;
        /// <summary>Начало диапазона</summary>
        public const byte From = Mark + 1;
        /// <summary>Окончание диапазона</summary>
        public const byte To = From + 1;
        /// <summary>Значение символизирующее точность</summary>
        public const byte Volume = To + 1;
        /// <summary>Это процент</summary>
        public const byte Percent = Volume + 1;
    }
    /// <summary>Тип пробы</summary>
    public struct PType
    {
        public const byte Name = 0;
    }
    /// <summary>Причина отбора</summary>
    public struct SCause
    {
        public const byte Name = 0;
    }
    /// <summary>Замеры к показателям</summary>
    public struct SM
    {
        /// <summary>Замер</summary>
        public const byte Sample = 0;
        /// <summary>Показатель</summary>
        public const byte Mark = Sample + 1;
        /// <summary>Значение показателя</summary>
        public const byte Amount = Mark + 1;
    }
    /// <summary>Направление потока</summary>
    public struct SFT
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Направление</summary>
        public const byte FDrctn = Name + 1;
    }

    public struct NType
    {
        public const byte Name = 0;
    }

    public struct PnMean
    {
        public const byte Name = 0;
    }
    /// <summary>Норма</summary>
    public struct Norm
    {
        //
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Действует от</summary>
        public const byte DFrom = Name + 1;
        /// <summary>Действует до</summary>
        public const byte DTo = DFrom + 1;
        /// <summary>Тип принадлежности</summary>
        public const byte NType = DTo + 1;
        /// <summary>Использовать</summary>
        public const byte Enabled = NType + 1;
        /// <summary>Показать</summary>
        public const byte Show = Enabled + 1;
    }
    /// <summary>Тип точки отбора</summary>
    public struct CSPT
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Тип</summary>
        public const byte SPT = Name + 1;
    }
    /// <summary>единица измерений</summary>
    public struct EdType
    {
        /// <summary>Наименование</summary>
        public const byte Name = 0;
        /// <summary>Эквивалент нуля</summary>
        public const byte MZero = Name + 1;
    }
    /// <summary>единица измерений вывода</summary>
    public struct OPType
    {
        /// <summary>Множитель</summary>
        public const byte Multy = 0;
        /// <summary>Единица измерений-ввод</summary>
        public const byte EdTypeF = Multy + 1;
        /// <summary>Единица измерений-вывод</summary>
        public const byte EdTypeT = EdTypeF + 1;
    }
    /// <summary>пул синхронизаций</summary>
    public struct SPool
    {
        /// <summary>Автор</summary>
        public const byte AUser = 0;
        /// <summary>Отправлятель</summary>
        public const byte SUser = AUser + 1;
        /// <summary>Создано локально</summary>
        public const byte Local = SUser + 1;
        /// <summary>Дата создания</summary>
        public const byte Date = Local + 1;
    }
}

/// <summary>Общие объекты с данными</summary>
public static class RCache
{
    public class Marks_class
    {
        public Marks_class()
        {
            this.Podr = T.Podr.CreateSubTable();
            this.OLocation = T.OLocation.CreateSubTable();
            this.Mark = T.Mark.CreateSubTable();
            this.MError = T.MError.CreateSubTable();
            this.PMNorm = T.PMNorm.CreateSubTable();
            this.Method = T.Method.CreateSubTable();
            this.Norm = T.Norm.CreateSubTable();
            Update();
        }

        readonly DataBase.ISTable Podr, OLocation, Mark, MError, PMNorm, Method, Norm;

        public void Update()
        {
            this.Mark.QUERRY().SHOW.DO();

            Marks = new Mark_class[this.Mark.Rows.Count];

            int SetCount = 0;

            for (int i = 0; i < this.Mark.Rows.Count; i++)
            {
                int ColIndex = this.Mark.Rows.Get<int>(i, C.Mark.Number) - 1;
                if (ColIndex > -1 && ColIndex < this.Marks.Length)
                {
                    if (Marks[ColIndex] != null && this.Marks[ColIndex].ID != this.Mark.Rows.GetID(i))
                    {
                        throw new Exception("Показатели " + this.Marks[ColIndex].Name + " и " + this.Mark.Rows.Get<string>(i, C.Mark.Name) + " имеют одинаковые значения колонки Mark.Number - " + this.Mark.Rows.Get<string>(i, C.Mark.Number));
                    }
                    var UIntTemp = this.Mark.Rows.GetID(i);
                    this.Marks[ColIndex] = new Mark_class(this, this.Mark.Rows.GetID(i));
                    SetCount++;
                }
                else
                {
                    throw new Exception("Показатель " + this.Mark.Rows.Get<string>(i, C.Mark.Name) + " имеет не верный номер(Mark.Number) - " + this.Mark.Rows.Get<string>(i, C.Mark.Number) + ", который должен быть больше 0 и меньше " + (this.Mark.Rows.Count + 1).ToString());
                }
            }

            ReloadNorm();

            for (int i = 0; i < Marks.Length; i++)
            {
                if (Marks[i] == null) throw new Exception("Номера колонок показателей заполняют не все значения");
            }
        }

        public void ReloadNorm()
        {
            PMNorm.QUERRY().SHOW.DO();
            Method.QUERRY().SHOW.DO();

            Norms = new Norms_struct();
            Norms.Init(this);

            for (int i = 0; i < PMNorm.Rows.Count; i++)
            {
                var NIndex = this.Norm.Rows.GetIndex(PMNorm.Rows.Get_UnShow<uint>(i, C.PMNorm.Norm));
                if (NIndex > -1)
                {
                    var MarkIndex = this.Mark.Rows.GetIndex(PMNorm.Rows.Get_UnShow<uint>(i, C.PMNorm.Mark));
                    if (MarkIndex > -1)
                    {
                        var MarkCol = PMNorm.Rows.Get<int>(i, C.PMNorm.Mark, C.Mark.Number) - 1;

                        if (MarkCol > -1)
                        {
                            if (NIndex > -1)
                            {
                                switch ((data.NType)PMNorm.Rows.Get_UnShow<uint>(i, C.PMNorm.Norm, C.Norm.NType))
                                {
                                    case data.NType.Mark:
                                        Marks[MarkCol].Norms[NIndex] = new Mark_class.Norm_class.MarkNorm_class(Marks[MarkCol], PMNorm.Rows.Get_UnShow<uint>(i, C.PMNorm.Norm), PMNorm.Rows.GetID(i));
                                        break;
                                    case data.NType.PodrAll:
                                        var StationIndex = Norms.GetPodrAllIndex(PMNorm.Rows.GetID(i));
                                        if (StationIndex > -1)
                                            Marks[MarkCol].Norms[NIndex].SetStation(StationIndex, new Mark_class.Norm_class.StationNorm_class.PodrNorm_class((Mark_class.Norm_class)Marks[MarkCol].Norms[NIndex], PMNorm.Rows.GetID(i)));
                                        break;
                                    case data.NType.PodrK:
                                        var StationKIndex = Norms.GetPodrKIndex(PMNorm.Rows.GetID(i));
                                        if (StationKIndex > -1)
                                            Marks[MarkCol].Norms[NIndex].SetStation(StationKIndex, new Mark_class.Norm_class.StationNorm_class.PodrNorm_class((Mark_class.Norm_class)Marks[MarkCol].Norms[NIndex], PMNorm.Rows.GetID(i)));
                                        break;
                                    case data.NType.PodrV:
                                        var StationVIndex = Norms.GetPodrVIndex(PMNorm.Rows.GetID(i));
                                        if (StationVIndex > -1)
                                            Marks[MarkCol].Norms[NIndex].SetStation(StationVIndex, new Mark_class.Norm_class.StationNorm_class.PodrNorm_class((Mark_class.Norm_class)Marks[MarkCol].Norms[NIndex], PMNorm.Rows.GetID(i)));
                                        break;
                                    case data.NType.Volume:
                                        var VolumeIndex = Norms.GetVolumeIndex(PMNorm.Rows.GetID(i));

                                        if (VolumeIndex > -1)
                                            Marks[MarkCol].Norms[NIndex].SetVolume(VolumeIndex, new Mark_class.Norm_class.OLocationNorm_class((Mark_class.Norm_class)Marks[MarkCol].Norms[NIndex], PMNorm.Rows.GetID(i)));
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            ReloadErrorRanges();

            for (int i = 0; i < Method.Rows.Count; i++)
            {
                var MarkIndex = this.Mark.Rows.GetIndex(Method.Rows.Get_UnShow<uint>(i, C.Method.Mark));
                if (MarkIndex > -1)
                {
                    var MarkCol = Method.Rows.Get<int>(i, C.Method.Mark, C.Mark.Number) - 1;

                    if (MarkCol > -1)
                    {
                        int NIndex = Norms.GetNormIndex(Method.Rows.Get_UnShow<uint>(i, C.Method.Norm));
                        if (NIndex > -1)
                        { ((Mark_class.Norm_class)Marks[MarkCol].Norms[NIndex]).MethodID = Method.Rows.GetID(i); }
                    }
                }
            }
        }

        public void ReloadErrorRanges()
        {
            MError.QUERRY().SHOW.DO();
            for (int i = 0; i < Marks.Length; i++)
            {
                for (int j = 0; j < Norms.NormMarkCount; j++)
                { Marks[i].Norms[j].Ranges.Clear(); }
            }

            for (int i = 0; i < MError.Rows.Count; i++)
            {
                var MarkIndex = Mark.Rows.GetIndex(MError.Rows.Get_UnShow<uint>(i, C.MError.Mark));

                if (MarkIndex > -1)
                {
                    MarkIndex = MError.Rows.Get_UnShow<int>(i, C.MError.Mark, C.Mark.Number) - 1;
                    if (MarkIndex > -1)
                    {
                        int NIndex = Norms.GetNormIndex(MError.Rows.Get_UnShow<uint>(i, C.MError.Norm));
                        if (NIndex > -1)
                        { Marks[MarkIndex].Norms[NIndex].Ranges.Add(new Mark_class.Ranges_struct(Marks[MarkIndex], MError.Rows.GetID(i))); }
                    }
                }
            }

            for (int i = 0; i < Marks.Length; i++)
            {
                for (int j = 0; j < Norms.NormMarkCount; j++)
                { Marks[i].Norms[j].Ranges.Sort((it1, it2) => { return it1.From.CompareTo(it2.From); }); }
            }
        }

        public Norms_struct Norms;

        public struct Norms_struct
        {
            public void Init(Marks_class Parent)
            {
                this.Parent = Parent;

                Parent.Norm.QUERRY().SHOW.DO();
                Parent.Podr.QUERRY().SHOW.WHERE.AC(C.Podr.PSG).More.BV<uint>(1).DO();
                Parent.OLocation.QUERRY().SHOW.WHERE.C(C.OLocation.Volumed, true).DO();

                AllPodrNorm = new NormPodr_struct[Parent.Norm.Rows.Count, Parent.Podr.Rows.Count];
                LimitedPodrNorm = new NormPodr_struct[Parent.Norm.Rows.Count, Parent.Podr.Rows.Count];
                VolumeNorm = new NormVolume_struct[Parent.Norm.Rows.Count, Parent.OLocation.Rows.Count];

                Norms = new Norm_struct[Parent.Norm.Rows.Count];
                PodrAllIDs = new uint[Parent.Podr.Rows.Count];

                {
                    var PodrKIDsList = new List<uint>(Parent.Podr.Rows.Count);
                    var PodrVIDsList = new List<uint>(Parent.Podr.Rows.Count);

                    for (int i = 0; i < Parent.Podr.Rows.Count; i++)
                    {
                        PodrAllIDs[i] = Parent.Podr.Rows.GetID(i);

                        switch ((data.PSG)Parent.Podr.Rows.Get_UnShow<uint>(i, C.Podr.PSG))
                        {
                            case data.PSG.Income:
                                PodrVIDsList.Add(PodrAllIDs[i]);
                                break;
                            case data.PSG.Outgo:
                                PodrKIDsList.Add(PodrAllIDs[i]);
                                break;
                            default: throw new Exception("Не допустимый тип PSG");
                        }
                    }

                    PodrKIDs = PodrKIDsList.ToArray();
                    PodrVIDs = PodrVIDsList.ToArray();
                }

                {
                    var VolumeIDsList = new List<uint>(Parent.OLocation.Rows.Count);

                    for (int i = 0; i < Parent.OLocation.Rows.Count; i++)
                    { VolumeIDsList.Add(Parent.OLocation.Rows.GetID(i)); }

                    this.VolumeIDs = VolumeIDsList.ToArray();
                }
                for (int i = 0; i < Parent.Marks.Length; i++)
                { Parent.Marks[i].Norms = new Mark_class.INorm[Norms.Length]; }

                for (int i = 0; i < Parent.Norm.Rows.Count; i++)
                {
                    var NormID = Parent.Norm.Rows.GetID(i);
                    Norms[i] = new Norm_struct(Parent, NormID);

                    switch (Norms[i].NType)
                    {
                        case data.NType.Mark:
                            //нужно для фиксирования верного типа NType. Mark записывается в Norms[]

                            for (int j = 0; j < PodrAllIDs.Length; j++) //зачищаю
                            { LimitedPodrNorm[i, j] = new NormPodr_struct(Parent, NormID, 0, -1); }

                            for (int j = 0; j < PodrAllIDs.Length; j++) //зачищаю
                            { AllPodrNorm[i, j] = new NormPodr_struct(Parent, NormID, 0, -1); }
                            break;
                        case data.NType.PodrAll:
                            for (int j = 0; j < PodrAllIDs.Length; j++)
                            { AllPodrNorm[i, j] = new NormPodr_struct(Parent, NormID, PodrAllIDs[j], j); }
                            break;
                        case data.NType.PodrK:
                            int PodrKCount = 0;
                            for (int j = 0; j < PodrKIDs.Length; j++)
                            {
                                int Index = Parent.Podr.Rows.GetIndex(PodrKIDs[j]);
                                LimitedPodrNorm[i, Index] = new NormPodr_struct(Parent, NormID, PodrKIDs[j], PodrKCount++);
                            }

                            for (int j = 0; j < PodrVIDs.Length; j++) //зачищаю
                            {
                                int Index = Parent.Podr.Rows.GetIndex(PodrVIDs[j]);
                                LimitedPodrNorm[i, Index] = new NormPodr_struct(Parent, NormID, 0, -1);
                            }
                            break;
                        case data.NType.PodrV:
                            int PodrVCount = 0;
                            for (int j = 0; j < PodrKIDs.Length; j++) //зачищаю
                            {
                                int Index = Parent.Podr.Rows.GetIndex(PodrKIDs[j]);
                                LimitedPodrNorm[i, Index] = new NormPodr_struct(Parent, NormID, 0, -1);
                            }

                            for (int j = 0; j < PodrVIDs.Length; j++)
                            {
                                int Index = Parent.Podr.Rows.GetIndex(PodrVIDs[j]);
                                LimitedPodrNorm[i, Index] = new NormPodr_struct(Parent, NormID, PodrVIDs[j], PodrVCount++);
                            }
                            break;
                        case data.NType.Volume:
                            for (int j = 0; j < VolumeIDs.Length; j++)
                            { VolumeNorm[i, j] = new NormVolume_struct(Parent, NormID, VolumeIDs[j]); }

                            for (int j = 0; j < PodrAllIDs.Length; j++) //зачищаю
                            { LimitedPodrNorm[i, j] = new NormPodr_struct(Parent, NormID, 0, -1); }

                            for (int j = 0; j < PodrAllIDs.Length; j++) //зачищаю
                            { AllPodrNorm[i, j] = new NormPodr_struct(Parent, NormID, 0, -1); }
                            break;
                        default: throw new Exception("Не известный тип NType");
                    }

                    for (int j = 0; j < Parent.Marks.Length; j++)
                    {
                        var Mark = Parent.Marks[j];

                        switch (Norms[i].NType)
                        {
                            case data.NType.Mark:
                                Mark.Norms[i] = new Mark_class.Norm_class.MarkNorm_class(Mark, Norms[i].NormID);
                                break;
                            case data.NType.PodrAll:
                            case data.NType.PodrV:
                            case data.NType.PodrK:
                                Mark.Norms[i] = new Mark_class.Norm_class.StationNorm_class(Mark, Norms[i].NormID);
                                break;
                            case data.NType.Volume:
                                Mark.Norms[i] = new Mark_class.Norm_class.VolumeNorm_class(Mark, Norms[i].NormID);
                                break;
                        }
                    }
                }
            }

            struct Norm_struct
            {
                public Norm_struct(Marks_class Parent, uint NormID)
                {
                    this.Parent = Parent;
                    this.NormID = NormID;

                    switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                    {
                        case data.NType.Mark:
                            Parent.Norms.NormMarkCount++;
                            break;
                        case data.NType.PodrAll:
                            Parent.Norms.NormPAllCount++;
                            break;
                        case data.NType.PodrK:
                            Parent.Norms.NormPKCount++;
                            break;
                        case data.NType.PodrV:
                            Parent.Norms.NormPVCount++;
                            break;
                        case data.NType.Volume:
                            Parent.Norms.NormVolumeCount++;
                            break;
                        default: throw new Exception("Не известный тип NType");
                    }
                }

                Marks_class Parent;
                public readonly uint NormID;
                public data.NType NType { get { return (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType); } }

                public override string ToString()
                {
                    return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + ", NType=" + T.Norm.Rows.Get<string>(NormID, C.Norm.NType);
                }
            }
            struct NormPodr_struct
            {
                public NormPodr_struct(Marks_class Parent, uint NormID, uint PodrID, int Index)
                {
                    this.Parent = Parent;
                    this.PodrID = PodrID;
                    this.NormID = NormID;
                    this.PodrIndex = Index;
                }

                Marks_class Parent;
                public readonly uint PodrID;
                public readonly uint NormID;
                public readonly int PodrIndex;
                public data.NType NType { get { return (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType); } }

                public override string ToString()
                {
                    return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + " - \"" + T.Podr.Rows.Get<string>(PodrID, C.Podr.ShrName) + "\"(" + PodrIndex.ToString() + ")";
                }
            }
            struct NormVolume_struct
            {
                public NormVolume_struct(Marks_class Parent, uint NormID, uint VolumeID)
                {
                    this.Parent = Parent;
                    this.VolumeID = VolumeID;
                    this.NormID = NormID;
                }

                Marks_class Parent;
                public readonly uint VolumeID;
                public readonly uint NormID;
                public data.NType NType { get { return (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType); } }

                public override string ToString()
                {
                    return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + " - \"" + T.OLocation.Rows.Get<string>(VolumeID, C.OLocation.ShrName) + "\"";
                }
            }

            Marks_class Parent;
            /// <summary>NType=PodrAll</summary>
            NormPodr_struct[,] AllPodrNorm;
            /// <summary>NType=PodrV || PodrK</summary>
            NormPodr_struct[,] LimitedPodrNorm;
            /// <summary>NType=Volume</summary>
            NormVolume_struct[,] VolumeNorm;
            /// <summary>NType=Mark</summary>
            Norm_struct[] Norms;

            uint[] PodrAllIDs;
            uint[] PodrKIDs;
            uint[] PodrVIDs;
            uint[] VolumeIDs;

            /// <summary>Количество норм по подразделениям</summary>
            public int NormPodrCount { get { return NormPAllCount + NormPVCount + NormPKCount; } }
            /// <summary>Количество подразделений для PodrAll</summary>
            public int PodrAllCount { get { return PodrAllIDs.Length; } }
            /// <summary>Количество подразделений для PodrV</summary>
            public int PodrVCount { get { return PodrVIDs.Length; } }
            /// <summary>Количество подразделений для PodrK</summary>
            public int PodrKCount { get { return PodrKIDs.Length; } }
            /// <summary>Количество подразделений для PodrK</summary>
            public int AllVolumesCount { get { return VolumeIDs.Length; } }

            /// <summary>Количество норм по типу PodrAll</summary>
            public int NormPAllCount { get; internal set; }
            /// <summary>Количество норм по типу PodrV</summary>
            public int NormPVCount { get; internal set; }
            /// <summary>Количество норм по типу PodrK</summary>
            public int NormPKCount { get; internal set; }
            /// <summary>Количество норм по типу Mark</summary>
            public int NormMarkCount { get; internal set; }
            /// <summary>Количество норм по типу Volume</summary>
            public int NormVolumeCount { get; internal set; }
            /// <summary>Количество норм</summary>
            public int NormCount { get { return Norms.Length; } }

            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrIndex(uint NormID, uint PodrID)
            {
                var NIndex = Parent.Norm.Rows.GetIndex(NormID);
                var PIndex = Parent.Podr.Rows.GetIndex(PodrID);

                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.PodrAll:
                        return AllPodrNorm[NIndex, PIndex].PodrIndex;
                    case data.NType.PodrK:
                        return LimitedPodrNorm[NIndex, PIndex].PodrIndex;
                    case data.NType.PodrV:
                        return LimitedPodrNorm[NIndex, PIndex].PodrIndex;
                    default: return -1;
                }
            }
            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrAllIndex(uint NormID, uint PodrID)
            {
                var NIndex = Parent.Norm.Rows.GetIndex(NormID);
                var PIndex = Parent.Podr.Rows.GetIndex(PodrID);

                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.PodrAll:
                        return AllPodrNorm[NIndex, PIndex].PodrIndex;
                    default: return -1;
                }
            }
            /// <summary>Получить номер подразделения водопровода</summary>
            public int GetPodrVIndex(uint NormID, uint PodrID)
            {
                var NIndex = Parent.Norm.Rows.GetIndex(NormID);
                var PIndex = Parent.Podr.Rows.GetIndex(PodrID);

                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.PodrV:
                        return LimitedPodrNorm[NIndex, PIndex].PodrIndex;
                    default: return -1;
                }
            }
            /// <summary>Получить номер подразделения канализации</summary>
            public int GetPodrKIndex(uint NormID, uint PodrID)
            {
                var NIndex = Parent.Norm.Rows.GetIndex(NormID);
                var PIndex = Parent.Podr.Rows.GetIndex(PodrID);

                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.PodrK:
                        return LimitedPodrNorm[NIndex, PIndex].PodrIndex;
                    default: return -1;
                }
            }
            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrIndex(uint PMNormID)
            { return GetPodrIndex(T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Norm), T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Podr)); }
            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrAllIndex(uint PMNormID)
            { return GetPodrAllIndex(T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Norm), T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Podr)); }
            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrVIndex(uint PMNormID)
            { return GetPodrVIndex(T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Norm), T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Podr)); }
            /// <summary>Получить номер подразделения в соответствии с типом нормы</summary>
            public int GetPodrKIndex(uint PMNormID)
            { return GetPodrKIndex(T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Norm), T.PMNorm.Rows.Get_UnShow<uint>(PMNormID, C.PMNorm.Podr)); }

            /// <summary>Получить номер выпуска в соответствии с типом нормы</summary>
            public int GetVolumeIndex(uint NormID, uint OLocationID)
            {
                if ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType) == data.NType.Volume)
                { return Parent.OLocation.Rows.GetIndex(OLocationID); }
                else
                { return -1; }
            }
            /// <summary>Получить номер выпуска в соответствии с типом нормы</summary>
            public int GetVolumeIndex(uint VNID)
            {
                return GetVolumeIndex(T.PMNorm.Rows.Get_UnShow<uint>(VNID, C.PMNorm.Norm), T.PMNorm.Rows.Get_UnShow<uint>(VNID, C.PMNorm.OLocation));
            }

            /// <summary>Получить ID подразделения из его номера в соответствии с типом нормы PodrAll</summary>
            public uint GetPodrIDFromPodrAll(int PodrNumber)
            { return PodrAllIDs[PodrNumber]; }
            /// <summary>Получить ID выпуска из его номера в соответствии с типом нормы Volue</summary>
            public uint GetVolumeID(int OLocationNumber)
            { return VolumeIDs[OLocationNumber]; }
            /// <summary>Получить ID подразделения из его номера в соответствии с типом нормы PodrK</summary>
            public uint GetPodrIDFromPodrK(int PodrNumber)
            { return PodrKIDs[PodrNumber]; }
            /// <summary>Получить ID подразделения из его номера в соответствии с типом нормы PodrV</summary>
            public uint GetPodrIDFromPodrV(int PodrNumber)
            { return PodrVIDs[PodrNumber]; }
            /// <summary>получить ID подразделения из его номера в соответствии с типом нормы</summary>
            public uint GetPodrID(uint NormID, int PodrNumber)
            {
                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.Mark:
                        return 0;
                    case data.NType.PodrAll:
                        return PodrAllIDs[PodrNumber];
                    case data.NType.PodrK:
                        return PodrKIDs[PodrNumber];
                    case data.NType.PodrV:
                        return PodrVIDs[PodrNumber];
                    default: throw new Exception("Неизвестный тип нормы");
                }
            }
            /// <summary>Получить ID выпуска из его номера в соответствии с типом нормы Volue</summary>
            public uint GetVolumeID(uint NormID, int VolumeNumber)
            {
                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.Volume:
                        return VolumeIDs[VolumeNumber];
                    default: throw new Exception("Неизвестный тип нормы");
                }
            }
            /// <summary>Получить номер нормы</summary>
            public int GetNormIndex(uint NormID)
            {
                return Parent.Norm.Rows.GetIndex(NormID);
            }

            /// <summary>Получить количество элементов для нормы в соответствии с её типом</summary>
            public int GetSubElementsCount(uint NormID)
            {
                switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                {
                    case data.NType.PodrAll:
                        return PodrAllIDs.Length;
                    case data.NType.PodrV:
                        return PodrVIDs.Length;
                    case data.NType.PodrK:
                        return PodrKIDs.Length;
                    case data.NType.Volume:
                        return VolumeIDs.Length;
                    case data.NType.Mark:
                        return 0;
                    default: throw new Exception("Неизвестный тип нормы");
                }
            }
        }

        public class Mark_class
        {
            public Mark_class(Marks_class Parent, uint MarkID)
            {
                this.ID = MarkID;
                this.Parent = Parent;
            }

            public struct Ranges_struct
            {
                public Ranges_struct(Mark_class Parent, uint MErrorID)
                {
                    this.Parent = Parent;
                    this.ID = MErrorID;
                }
                Mark_class Parent;
                public readonly uint ID;

                public double From { get { return T.MError.Rows.Get<double>(ID, C.MError.From); } }
                public double To { get { return T.MError.Rows.Get<double>(ID, C.MError.To); } }
                public bool Percent { get { return T.MError.Rows.Get<bool>(ID, C.MError.Percent); } }
                public double Volume { get { return T.MError.Rows.Get<double>(ID, C.MError.Volume); } }

                public override string ToString()
                {
                    return Parent.Parent.MError.Rows.Get<string>(ID, C.MError.Mark, C.Mark.Name) + ", " + Parent.Parent.MError.Rows.Get<string>(ID, C.MError.Norm, C.Norm.Name) + ":" + From.ToString() + "-" + To.ToString() + "/" + Percent.ToString();
                }
            }

            public interface INorm
            {
                Mark_class Mark { get; }
                uint NormID { get; }
                string Name { get; }

                string MethodName { get; set; }

                List<Ranges_struct> Ranges { get; }

                double InputFrom { get; }
                double InputTo { get; }

                bool CheckVolume(double Volume);

                data.NType NType { get; }

                Norm_class.PodrNorm_class Range { get; }
                string InputRange { get; }
                int SCount { get; }

                Norm_class.PodrNorm_class Station(int StationIndex);
                Norm_class.OLocationNorm_class Volume(int VolumeIndex);
                void SetStation(int StationIndex, Norm_class.PodrNorm_class PN);
                void SetVolume(int VolumeIndex, Norm_class.OLocationNorm_class VN);
            }

            public class Norm_class
            {
                public Norm_class(Mark_class Parent, uint NormID)
                {
                    this.Parent = Parent;
                    this.NormID = NormID;
                }

                public class PodrNorm_class
                {
                    public PodrNorm_class(Norm_class Parent, uint PNID)
                    {
                        this.Parent = Parent;
                        this.ID = PNID;
                        this.PodrID = T.PMNorm.Rows.Get_UnShow<uint>(ID, C.PMNorm.Podr);
                        this._From = T.PMNorm.Rows.Get<double>(ID, C.PMNorm.LFrom);
                        this._To = T.PMNorm.Rows.Get<double>(ID, C.PMNorm.LTo);
                        //this._Ground = T.PMNorm.Rows.Get<bool>(ID, C.PMNorm.BckGrnd);
                    }

                    public PodrNorm_class(Norm_class Parent, uint PodrID, uint NormID)
                    {
                        this.Parent = Parent;
                        this.ID = 0;
                        this._From = 0;
                        this._To = 0;
                        this.PodrID = PodrID;
                        this._Ground = false;
                    }

                    uint ID;
                    uint PodrID;
                    double _From;
                    double _To;
                    bool _Ground;
                    Norm_class Parent;

                    public uint ID_Station
                    {
                        get { return PodrID; }
                    }
                    public string StationName
                    {
                        get { return T.Podr.Rows.Get<string>(PodrID, C.Podr.ShrName); }
                    }

                    /*public bool Ground
                    {
                        get { return _Ground; }
                        set
                        {
                            if (ID == 0)
                            {
                                if ((bool)PMNorm.QUERRY().ADD
                                        .C(C.PMNorm.BckGrnd, value)
                                        .C(C.PMNorm.LFrom, _From)
                                        .C(C.PMNorm.LTo, _To)
                                        .C(C.PMNorm.Mark, Parent.Parent.ID)
                                        .C(C.PMNorm.Norm, Parent.NormID)
                                        .C(C.PMNorm.Podr, PodrID)
                                    .DO()[0].Value)
                                    ID = PMNorm.Rows.GetID(PMNorm.Rows.Count - 1);
                            }
                            else
                            {
                                PMNorm.Rows.Set(ID, C.PMNorm.BckGrnd, value);
                            }
                            _Ground = value;
                        }
                    }*/
                    public double From
                    {
                        get { return _From; }
                    }
                    public double To
                    {
                        get { return _To; }
                    }

                    public string FullRange
                    {
                        get { return _From.ToString() + "-" + _To.ToString(); }
                    }

                    public string Range
                    {
                        get
                        {
                            if (_From + _To == 0)
                                return Parent.Parent.MeanZero;
                            else
                                return GetRange(_From, _To);
                        }
                        set
                        {
                            double From, To;
                            SetRanges(out From, out To, value);
                            if (ID == 0)
                            {
                                if ((bool)Parent.Parent.Parent.PMNorm.QUERRY().ADD
                                    //.C(C.PMNorm.BckGrnd, _Ground)
                                           .C(C.PMNorm.LFrom, From)
                                           .C(C.PMNorm.LTo, To)
                                           .C(C.PMNorm.Mark, Parent.Parent.ID)
                                           .C(C.PMNorm.Norm, Parent.NormID)
                                           .C(C.PMNorm.Podr, PodrID)
                                       .DO()[0].Value)
                                {
                                    _To = To;
                                    _From = From;

                                    ID = Parent.Parent.Parent.PMNorm.Rows.GetID(Parent.Parent.Parent.PMNorm.Rows.Count - 1);
                                }
                            }
                            else
                            {
                                _To = To;
                                _From = From;

                                Parent.Parent.Parent.PMNorm.QUERRY().SET.C(C.PMNorm.LFrom, From).C(C.PMNorm.LTo, To).WHERE.ID(ID).DO();
                            }
                        }
                    }

                    public override string ToString()
                    {
                        return T.PMNorm.Rows.Get<string>(ID, C.PMNorm.Mark, C.Mark.Name) + "\\" + StationName + "\\" + Range;
                    }
                }
                public class OLocationNorm_class
                {
                    public OLocationNorm_class(Norm_class Parent, uint PMNID)
                    {
                        this.Parent = Parent;
                        this.ID = PMNID;
                        this.VolumeID = T.PMNorm.Rows.Get_UnShow<uint>(ID, C.PMNorm.OLocation);
                        this._From = T.PMNorm.Rows.Get<double>(ID, C.PMNorm.LFrom);
                        this._To = T.PMNorm.Rows.Get<double>(ID, C.PMNorm.LTo);
                        //this._Ground = T.PMNorm.Rows.Get<bool>(ID, C.PMNorm.BckGrnd);
                    }

                    public OLocationNorm_class(Norm_class Parent, uint VolumeID, uint NormID)
                    {
                        this.Parent = Parent;
                        this.ID = 0;
                        this._From = 0;
                        this._To = 0;
                        this.VolumeID = VolumeID;
                        this._Ground = false;
                    }

                    uint ID;
                    uint VolumeID;
                    double _From;
                    double _To;
                    bool _Ground;
                    Norm_class Parent;

                    public uint ID_OLocation
                    {
                        get { return VolumeID; }
                    }
                    public string VolumeName
                    {
                        get { return T.OLocation.Rows.Get<string>(VolumeID, C.OLocation.ShrName); }
                    }
                    public double From
                    {
                        get { return _From; }
                    }
                    public double To
                    {
                        get { return _To; }
                    }

                    public string FullRange
                    {
                        get { return _From.ToString() + "-" + _To.ToString(); }
                    }

                    public string Range
                    {
                        get
                        {
                            if (_From + _To == 0)
                                return Parent.Parent.MeanZero;
                            else
                                return GetRange(_From, _To);
                        }
                        set
                        {
                            double From, To;
                            SetRanges(out From, out To, value);
                            if (ID == 0)
                            {
                                if ((bool)Parent.Parent.Parent.PMNorm.QUERRY().ADD
                                    //.C(C.PMNorm.BckGrnd, _Ground)
                                           .C(C.PMNorm.LFrom, From)
                                           .C(C.PMNorm.LTo, To)
                                           .C(C.PMNorm.Mark, Parent.Parent.ID)
                                           .C(C.PMNorm.Norm, Parent.NormID)
                                           .C(C.PMNorm.OLocation, VolumeID)
                                       .DO()[0].Value)
                                {
                                    _To = To;
                                    _From = From;

                                    ID = Parent.Parent.Parent.PMNorm.Rows.GetID(Parent.Parent.Parent.PMNorm.Rows.Count - 1);
                                }
                            }
                            else
                            {
                                _To = To;
                                _From = From;

                                Parent.Parent.Parent.PMNorm.QUERRY().SET.C(C.PMNorm.LFrom, From).C(C.PMNorm.LTo, To).WHERE.ID(ID).DO();
                            }
                        }
                    }

                    public override string ToString()
                    {
                        return T.PMNorm.Rows.Get<string>(ID, C.PMNorm.Mark, C.Mark.Name) + "\\" + VolumeName + "\\" + Range;
                    }
                }
                public class StationNorm_class : Norm_class, INorm
                {
                    public StationNorm_class(Mark_class Parent, uint SNormID)
                        : base(Parent, SNormID)
                    {
                        switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(base.NormID, C.Norm.NType))
                        {
                            case data.NType.PodrAll:
                                this.Stations = new PodrNorm_class[Parent.Parent.Norms.PodrAllCount];

                                for (int i = 0; i < Stations.Length; i++)
                                    Stations[i] = new PodrNorm_class(this, Parent.Parent.Norms.GetPodrIDFromPodrAll(i), this.NormID);
                                break;
                            case data.NType.PodrK:
                                this.Stations = new PodrNorm_class[Parent.Parent.Norms.PodrKCount];

                                for (int i = 0; i < Stations.Length; i++)
                                    Stations[i] = new PodrNorm_class(this, Parent.Parent.Norms.GetPodrIDFromPodrK(i), this.NormID);
                                break;
                            case data.NType.PodrV:
                                this.Stations = new PodrNorm_class[Parent.Parent.Norms.PodrVCount];

                                for (int i = 0; i < Stations.Length; i++)
                                    Stations[i] = new PodrNorm_class(this, Parent.Parent.Norms.GetPodrIDFromPodrV(i), this.NormID);
                                break;
                        }
                    }

                    public PodrNorm_class[] Stations;

                    public int SCount { get { return Stations.Length; } }

                    public Norm_class.PodrNorm_class this[int StationIndex]
                    {
                        get { return Stations[StationIndex]; }
                        set { Stations[StationIndex] = value; }
                    }
                    public Norm_class.PodrNorm_class Station(int StationIndex)
                    { return Stations[StationIndex]; }
                    public void SetStation(int StationIndex, Norm_class.PodrNorm_class SN)
                    { Stations[StationIndex] = SN; }

                    public Norm_class.OLocationNorm_class Volume(int VolumenIndex)
                    { throw new Exception("Норма для выпуска недопустима"); }
                    public void SetVolume(int VolumenIndex, Norm_class.OLocationNorm_class VN)
                    { throw new Exception("Норма для выпуска недопустима"); }

                    public PodrNorm_class Range { get { throw new Exception("Норматив не содержит диапазон для показателя"); } }

                    public override string ToString()
                    {
                        return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + ", SCount=" + Stations.Length + ", RCount=" + Ranges.Count;
                    }
                }
                public class VolumeNorm_class : Norm_class, INorm
                {
                    public VolumeNorm_class(Mark_class Parent, uint NormID)
                        : base(Parent, NormID)
                    {
                        this.Volumes = new OLocationNorm_class[Parent.Parent.Norms.AllVolumesCount];

                        for (int i = 0; i < this.Volumes.Length; i++)
                            this.Volumes[i] = new OLocationNorm_class(this, Parent.Parent.Norms.GetVolumeID(i), this.NormID);
                    }

                    public OLocationNorm_class[] Volumes;

                    public int SCount { get { return Volumes.Length; } }

                    public Norm_class.OLocationNorm_class this[int StationIndex]
                    {
                        get { return Volumes[StationIndex]; }
                        set { Volumes[StationIndex] = value; }
                    }
                    public Norm_class.PodrNorm_class Station(int StationIndex)
                    { throw new Exception("Нет станций для этой нормы"); }
                    public void SetStation(int StationIndex, Norm_class.PodrNorm_class SN)
                    { throw new Exception("Нельзя тут заносить норму подразделения"); }

                    public Norm_class.OLocationNorm_class Volume(int VolumenIndex)
                    { return Volumes[VolumenIndex]; }
                    public void SetVolume(int VolumenIndex, Norm_class.OLocationNorm_class VN)
                    { Volumes[VolumenIndex] = VN; }

                    public PodrNorm_class Range { get { throw new Exception("Норматив не содержит диапазон для показателя"); } }

                    public override string ToString()
                    {
                        return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + ", SCount=" + Volumes.Length + ", RCount=" + Ranges.Count;
                    }
                }
                public class MarkNorm_class : Norm_class, INorm
                {
                    public MarkNorm_class(Mark_class Parent, uint NormID)
                        : base(Parent, NormID)
                    { Range = new PodrNorm_class(this, 0, base.NormID); }
                    public MarkNorm_class(Mark_class Parent, uint NormID, uint MNormID)
                        : base(Parent, NormID)
                    { Range = new PodrNorm_class(this, MNormID); }

                    public int SCount { get { throw new Exception("Нет станций для этой нормы"); } }
                    public Norm_class.PodrNorm_class Station(int StationIndex)
                    { throw new Exception("Нет станций для этой нормы"); }
                    public void SetStation(int StationIndex, Norm_class.PodrNorm_class SN)
                    { throw new Exception("Нельзя тут заносить норму подразделения"); }

                    public Norm_class.OLocationNorm_class Volume(int VolumenIndex)
                    { throw new Exception("Норма для выпуска недопустима"); }
                    public void SetVolume(int VolumenIndex, Norm_class.OLocationNorm_class VN)
                    { throw new Exception("Норма для выпуска недопустима"); }

                    public PodrNorm_class Range { get; internal set; }

                    public override string ToString()
                    {
                        return T.Norm.Rows.Get<string>(NormID, C.Norm.Name) + ", RCount=" + Ranges.Count + ", " + Parent.Name + "\\" + Range.ToString();
                    }
                }

                public Mark_class Mark { get { return Parent; } }
                public readonly Mark_class Parent;

                public uint MethodID;
                public uint NormID { get; internal set; }
                public string Name { get { return T.Norm.Rows.Get<string>(NormID, C.Norm.Name); } }

                public string MethodName
                {
                    get
                    {
                        if (MethodID == 0)
                        { return ""; }
                        else
                        { return T.Method.Rows.Get<string>(MethodID, C.Method.Name); }
                    }
                    set
                    {
                        if (MethodID == 0)
                        {
                            if ((bool)Parent.Parent.Method.QUERRY().ADD.C(C.Method.Mark, Parent.ID).C(C.Method.Norm, NormID).C(C.Method.Name, value).DO()[0].Value)
                            { this.MethodID = Parent.Parent.Method.Rows.GetID(Parent.Parent.Method.Rows.Count - 1); }
                        }
                        else
                        {
                            if (value == null)
                            { T.Method.Rows.Set(MethodID, C.Method.Name, ""); }
                            else
                            { T.Method.Rows.Set(MethodID, C.Method.Name, value); }
                        }
                    }
                }

                public data.NType NType { get { return (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType); } }

                readonly List<Ranges_struct> _Ranges = new List<Ranges_struct>();
                public List<Ranges_struct> Ranges { get { return _Ranges; } }

                /// <summary>Получить точность измерений</summary>
                /// <param name="Volume">Значение относительно которого нужно найти точность измерений</param>
                /// <returns>Значение точности</returns>
                public double GetAcurracy(double Volume)
                {
                    if (CheckVolume(Volume))
                    {
                        for (int i = 0; i < Ranges.Count; i++)
                        {
                            if (Ranges[i].From <= Volume && Ranges[i].To > Volume)
                            {
                                if (Ranges[i].Percent)
                                { return Volume * Ranges[i].Volume; }
                                else
                                { return Ranges[i].Volume; }
                            }
                        }
                    }

                    return 0;
                }
                /// <summary>Проверить входит ли значение в диапазон</summary>
                /// <param name="Volume">Проверяемое значение</param>
                /// <returns>Вердикт</returns>
                public bool CheckVolume(double Volume)
                {
                    if (Ranges.Count == 0) return true;

                    var ImputFrom = Ranges[0].From;
                    var ImputTo = Ranges[Ranges.Count - 1].To;

                    if (Volume != 0 && ImputFrom + ImputTo > 0)
                    {
                        if (ImputFrom > Volume || ImputTo < Volume)
                        {
                            //MessageBox.Show("Значение " + Volume + " вне пределах допустимого диапазона(" + ImputFrom + "-" + ImputTo + ") для показателя \"" + this.Parent.Name + "\", ввод невозможен.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    return true;
                }

                public double InputFrom
                {
                    get
                    {
                        if (Ranges.Count == 0)
                        { return double.MinValue; }
                        else
                        { return Ranges[0].From; }
                    }
                }

                public double InputTo
                {
                    get
                    {
                        if (Ranges.Count == 0)
                        { return double.MaxValue; }
                        else
                        { return Ranges[Ranges.Count - 1].To; }
                    }
                }

                public string InputRange { get { return InputFrom.ToString() + "-" + InputTo.ToString(); } }
            }

            public int Number
            {
                get { return T.Mark.Rows.Get<byte>(ID, C.Mark.Number); }
                set
                {
                    if (value != Number)
                    {
                        var M = RCache.Marks.Marks[value - 1];
                        RCache.Marks.Marks[value - 1] = this;
                        RCache.Marks.Marks[Number - 1] = M;

                        T.Mark.Rows.Set(M.ID, C.Mark.Number, (byte)this.Number);
                        T.Mark.Rows.Set(this.ID, C.Mark.Number, (byte)value);
                    }
                }
            }
            public byte Round
            {
                get { return T.Mark.Rows.Get<byte>(ID, C.Mark.Round); }
                set { T.Mark.Rows.Set(ID, C.Mark.Round, value); }
            }
            public bool Exp
            {
                get { return T.Mark.Rows.Get<bool>(ID, C.Mark.Exp); }
                set { T.Mark.Rows.Set(ID, C.Mark.Exp, value); }
            }
            public double GetRound(double Volume)
            {
                byte Round = Misc.Round;

                if (this.Round > 0)
                { Round = this.Round; }

                return Math.Round(Volume, Round);
            }
            public string GetRoundedVolume(double Volume)
            {
                if (Volume == 0)
                { return this.MeanZero; }

                byte Round = Misc.Round;

                if (this.Round > 0)
                { Round = this.Round; }

                if (Exp && (Volume > 1000 || Volume < 0.0001))
                { return Volume.ToString("e" + Round.ToString()); }
                else
                {
                    if (Round == 0)
                    { return Volume.ToString("0"); }
                    else
                    {
                        var ret = "0.";

                        for (int i = 0; i < Round; i++)
                        { ret += "#"; }

                        var Ret = Volume.ToString(ret);
                        if (Ret == "0")
                        { return this.MeanZero; }
                        else
                        { return Ret; }
                    }
                }
            }
            public string GetVolume(double Volume)
            {
                if (Exp && (Volume > 1000 || Volume < 0.0001))
                { return Volume.ToString("e"); }
                else
                { return Volume.ToString(); }
            }
            public string Name
            {
                get { return T.Mark.Rows.Get<string>(ID, C.Mark.Name); }
                set { T.Mark.Rows.Set(this.ID, C.Mark.Name, value); }
            }
            public int Code
            {
                get { return T.Mark.Rows.Get<int>(ID, C.Mark.Code); }
                set { T.Mark.Rows.Set(this.ID, C.Mark.Code, value); }
            }
            public data.VarType VarType
            {
                get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(ID, C.Mark.VarType); }
                set { T.Mark.Rows.Set(this.ID, C.Mark.VarType, (uint)value); }
            }
            public string EdType { get { return T.Mark.Rows.Get<string>(ID, C.Mark.EdType, C.EdType.Name); } }
            public string OPType { get { return T.Mark.Rows.Get<string>(ID, C.Mark.OPType, C.OPType.EdTypeT, C.EdType.Name); } }
            /// <summary>Эквивалент ноля</summary>
            public string MeanZero { get { return T.Mark.Rows.Get<string>(ID, C.Mark.EdType, C.EdType.MZero); } }
            /// <summary>Эквивалент ноля</summary>
            public bool ShowZero
            {
                get { return T.Mark.Rows.Get<bool>(ID, C.Mark.ShowZr); }
                set { T.Mark.Rows.Set(ID, C.Mark.ShowZr, value); }
            }
            /// <summary>Множитель вывода</summary>
            public double Multyply { get { return T.Mark.Rows.Get<double>(ID, C.Mark.OPType, C.OPType.Multy); } }
            public double GetResultValue(double Value) { return Value * Multyply; }

            public uint EdTypeID
            {
                get { return T.Mark.Rows.Get_UnShow<uint>(ID, C.Mark.EdType); }
                set { T.Mark.Rows.Set(ID, C.Mark.EdType, value); }
            }
            public uint OPTypeID
            {
                get { return T.Mark.Rows.Get_UnShow<uint>(ID, C.Mark.OPType); }
                set { T.Mark.Rows.Set(ID, C.Mark.OPType, value); }
            }

            public static string GetRange(double From, double To)
            {
                if (To > From)
                {
                    if (From == 0)
                    { return To.ToString(); }
                    else
                    { return From.ToString() + "-" + To.ToString(); }
                }
                else if (To < From)
                {
                    if (To == 0)
                    { return From.ToString(); }
                    else
                    { return From.ToString() + "-" + To.ToString(); }
                }
                else
                    return "";
            }
            public static void SetRanges(out double From, out double To, string Range)
            {
                if (Range == null || Range.Length == 0)
                { From = To = 0; }
                else
                {
                    for (int i = 0; i < Range.Length; i++)
                    {
                        if (Range[i] == '-')
                        {
                            From = Convert.ToDouble(DataBase.NoAbc_Double_Static(Range.Substring(0, i)));
                            To = Convert.ToDouble(DataBase.NoAbc_Double_Static(Range.Substring(i + 1, Range.Length - i - 1)));

                            goto Finded;
                        }
                    }

                    From = Convert.ToDouble(DataBase.NoAbc_Double_Static(Range));
                    To = 0;
                Finded: ;
                    if (From > To)
                    {
                        var tmp = To;
                        To = From;
                        From = tmp;
                    }
                }
            }

            public readonly uint ID;

            Marks_class Parent;

            /// <summary>Не использовать с наружи</summary>
            public INorm[] Norms;

            public INorm GetNorm(uint NormID) { return Norms[Parent.Norm.Rows.GetIndex(NormID)]; }
            public INorm GetNorm(int NormIndex) { return Norms[NormIndex]; }

            public override string ToString()
            {
                return (T.Mark.Rows.Get<byte>(ID, C.Mark.Number) - 1).ToString() + ")" + T.Mark.Rows.Get<string>(ID, C.Mark.Name) + ", NormsCount=" + Norms.Length.ToString();
            }
        }

        public int GetMarkIndex(uint ID)
        {
            return this.Mark.Rows.Get<int>(ID, C.Mark.Number) - 1;
        }

        Mark_class[] Marks;

        /// <summary>Получаю показатель по индексу(Index)</summary>
        public Mark_class this[int Index]
        { get { return Marks[Index]; } }
        /// <summary>Получаю показатель по идентификатору. Идентификатор конвертируется в (Mark.Number - 1), а не в Index</summary>
        public Mark_class this[uint ID] 
        { 
            get 
            {
                var Index = T.Mark.Rows.Get<byte>(ID, C.Mark.Number) - 1;

                if (Index > -1)
                { return Marks[Index]; }

                return null;
            }
        }

        public int Count { get { return Marks.Length; } }

        public override string ToString()
        {
            return "MCount=" + Marks.Length.ToString();
        }
    }
    public class PSG_class
    {
        public PSG_class()
        {
            PSGM = T.PSGM.CreateSubTable();
        }

        DataBase.ISTable PSGM;

        int YM;

        public void Reload(int YM)
        {
            this.YM = YM;

            PSGIDI = ReloadIncome();
            PSGIDO = ReloadOutgo();

            PSGM.QUERRY()
                .SHOW
                .WHERE
                    .ID(PSGIDO)
                    .OR.ID(PSGIDI)
                .DO();
        }

        uint ReloadIncome()
        {
            return (uint)PSGM.QUERRY()
                .GET.ID()
                .Max(C.PSGM.YM)
                .By(C.PSGM.PSG)
                .WHERE
                    .AC(C.PSGM.YM).Less.BV(YM + 1)
                    .AND.C(C.PSGM.PSG, (uint)data.PSG.Income)
                .DO()[0].Value;
        }

        uint ReloadOutgo()
        {
            return (uint)PSGM.QUERRY()
                .GET.ID()
                .Max(C.PSGM.YM)
                .By(C.PSGM.PSG)
                .WHERE
                    .AC(C.PSGM.YM).Less.BV(YM + 1)
                    .AND.C(C.PSGM.PSG, (uint)data.PSG.Outgo)
                .DO()[0].Value;
        }

        uint PSGIDO, PSGIDI;

        public string GetMethodName(uint PodrID)
        {
            return GetMethodName((data.PSG)T.Podr.Rows.Get_UnShow<uint>(PodrID, C.Podr.PSG));
        }
        public string GetMethodName(data.PSG psg)
        {
            switch (psg)
            {
                case data.PSG.Income:
                    if (PSGIDI == 0)
                    { return ""; }
                    else
                    { return T.PSGM.Rows.Get<string>(PSGIDI, C.PSGM.Method); }
                case data.PSG.Outgo:
                    if (PSGIDI == 0)
                    { return ""; }
                    else
                    { return T.PSGM.Rows.Get<string>(PSGIDO, C.PSGM.Method); }
                default: return "";
            }
        }

        public uint GetPeopleID(data.PSG psg)
        {
            switch (psg)
            {
                case data.PSG.Income:
                    if (PSGIDI == 0)
                    { return 0; }
                    else
                    { return T.PSGM.Rows.Get_UnShow<uint>(PSGIDI, C.PSGM.People); }
                case data.PSG.Outgo:
                    if (PSGIDO == 0)
                    { return 0; }
                    else
                    { return T.PSGM.Rows.Get_UnShow<uint>(PSGIDO, C.PSGM.People); }
                default: return 0;
            }
        }

        public void DeleteIncome()
        {
            if (PSGIDI > 0)
            {
                PSGM.Rows.Delete(PSGIDI);
                PSGIDI = ReloadIncome();
            }
        }

        public void DeleteOutgo()
        {
            if (PSGIDO > 0)
            {
                PSGM.Rows.Delete(PSGIDO);
                PSGIDO = ReloadOutgo();
            }
        }

        public void SetMethod(data.PSG psg, string MethodName, uint PeopleID)
        {
            if (GetMethodName(psg) != MethodName || GetPeopleID(psg) != PeopleID)
            {
                switch (psg)
                {
                    case data.PSG.Income:
                        if (GetIncomeID() == 0)
                        {
                            PSGM.QUERRY()
                                .ADD
                                    .C(C.PSGM.Method, MethodName)
                                    .C(C.PSGM.YM, Employe_Form.SPoints.YM)
                                    .C(C.PSGM.PSG, (uint)psg)
                                    .C(C.PSGM.People, PeopleID)
                                .DO();
                            PSGIDI = PSGM.Rows.GetID(PSGM.Rows.Count - 1);
                        }
                        else
                        {
                            PSGM.QUERRY()
                                .SET
                                    .C(C.PSGM.Method, MethodName)
                                    .C(C.PSGM.People, PeopleID)
                                .WHERE.ID(PSGIDI)
                                .DO();
                        }
                        break;
                    case data.PSG.Outgo:
                        if (GetOutgoID() == 0)
                        {
                            PSGM.QUERRY()
                                .ADD
                                    .C(C.PSGM.Method, MethodName)
                                    .C(C.PSGM.YM, Employe_Form.SPoints.YM)
                                    .C(C.PSGM.PSG, (uint)psg)
                                    .C(C.PSGM.People, PeopleID)
                                .DO();
                            PSGIDO = PSGM.Rows.GetID(PSGM.Rows.Count - 1);
                        }
                        else
                        {
                            PSGM.QUERRY()
                                .SET
                                    .C(C.PSGM.Method, MethodName)
                                    .C(C.PSGM.People, PeopleID)
                                .WHERE.ID(PSGIDO)
                                .DO();
                        }
                        break;
                }
            }
        }

        public uint GetIncomeID()
        {
            if (PSGIDI > 0)
            {
                if (T.PSGM.Rows.Get<int>(PSGIDI, C.PSGM.YM) == Employe_Form.SPoints.YM)
                { return PSGIDI; }
            }
            return 0;
        }

        public uint GetOutgoID()
        {
            if (PSGIDO > 0)
            {
                if (T.PSGM.Rows.Get<int>(PSGIDO, C.PSGM.YM) == Employe_Form.SPoints.YM)
                { return PSGIDO; }
            }
            return 0;
        }

        public uint GetID(data.PSG psg)
        {
            switch (psg)
            { 
                case data.PSG.Income:
                    return GetIncomeID();
                case data.PSG.Outgo:
                    return GetOutgoID();
            }
            return 0;
        }

        public DateTime DateOfCreateIncome()
        {
            if (PSGIDI == 0)
                return new DateTime();
            else
                return ATMisc.GetDateTimeFromYM(T.PSGM.Rows.Get<int>(PSGIDI, C.PSGM.YM));
        }

        public DateTime DateOfCreateOutgo()
        {
            if (PSGIDO == 0)
                return new DateTime();
            else
                return ATMisc.GetDateTimeFromYM(T.PSGM.Rows.Get<int>(PSGIDO, C.PSGM.YM));
        }

        public override string ToString()
        {
            return "PSGIDI=" + PSGIDI.ToString() + ", PSGIDO=" + PSGIDO.ToString();
        }
    }
    public class Volumes_class
    {
        public Volumes_class()
        {
            MVolume = T.MVolume.CreateSubTable();
            OLocation = T.OLocation.CreateSubTable();
        }

        struct Volume_struct
        {
            public Volume_struct(Volumes_class Parent, uint VGroupID, uint MVolumeID)
            {
                this.Parent = Parent;
                this.VGroupID = VGroupID;
                this.MVolumeID = MVolumeID;
            }
            Volumes_class Parent;
            uint MVolumeID;
            public readonly uint VGroupID;
            public double Volume
            {
                get
                {
                    if (MVolumeID == 0)
                        return 0;
                    else
                        return T.MVolume.Rows.Get<double>(MVolumeID, C.MVolume.Volume);
                }
                set
                {
                    if (MVolumeID == 0)
                    {
                        if ((bool)Parent.MVolume.QUERRY()
                                .ADD
                                    .C(C.MVolume.Volume, value)
                                    .C(C.MVolume.YM, Employe_Form.SPoints.YM)
                                    .C(C.MVolume.OLocation, VGroupID)
                                .DO()[0].Value)
                        {
                            this.MVolumeID = Parent.MVolume.Rows.GetID(Parent.MVolume.Rows.Count - 1);
                        }
                    }
                    else
                    {
                        T.MVolume.Rows.Set(MVolumeID, C.MVolume.Volume, value);
                    }
                }
            }
        }

        DataBase.ISTable MVolume, OLocation;

        public void Reload(int YM)
        {
            OLocation.QUERRY().SHOW.WHERE.C(C.OLocation.Volumed, true).DO();
            MVolume.QUERRY().SHOW.WHERE.C(C.MVolume.YM, YM).DO();

            this.Volumes = new Volume_struct[OLocation.Rows.Count];

            for (int i = 0; i < Volumes.Length; i++)
                Volumes[i] = new Volume_struct(this, GetVGroupID(i), 0);

            for (int i = 0; i < MVolume.Rows.Count; i++)
            {
                var VGIndex = OLocation.Rows.GetIndex(MVolume.Rows.Get_UnShow<uint>(i, C.MVolume.OLocation));

                if (VGIndex > -1)
                { Volumes[VGIndex] = new Volume_struct(this, MVolume.Rows.Get_UnShow<uint>(i, C.MVolume.OLocation), MVolume.Rows.GetID(i)); }
            }
        }

        Volume_struct[] Volumes;

        public int Count { get { return OLocation.Rows.Count; } }

        public string GetName(int Index) { return OLocation.Rows.Get<string>(Index, C.OLocation.Name); }
        public string GetShortName(int Index) { return OLocation.Rows.Get<string>(Index, C.OLocation.ShrName); }
        public double GetVolume(int Index) { return Volumes[Index].Volume; }

        public double GetVolume(uint SampleID)
        {
            var GVIndex = OLocation.Rows.GetIndex(T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom));
            return (GVIndex > -1 ? Volumes[GVIndex].Volume : 0);
        }

        public void SetVolume(int Index, double Volume)
        {
            Volumes[Index].Volume = Volume;
        }

        public uint GetVGroupID(int Index) { return OLocation.Rows.GetID(Index); }

        public int GetIndex(uint VGroupID) { return OLocation.Rows.GetIndex(VGroupID); }

        public override string ToString()
        {
            return Count.ToString();
        }
    }
    public class Podrs_class
    {
        public Podrs_class()
        {
            this.Podr = T.Podr.CreateSubTable();
            //this.Podr.AfterShowQuerry -= AfterShow;
            this.Podr.AfterShowQuerry += AfterShow;
            //this.Podr.AfterSort -= Sort;
            this.Podr.AfterSort += Sort;
            this.Podr.QUERRY().SHOW.DO();
        }

        DataBase.ISTable Podr;

        public Podr_class this[int Index] { get { return Podrs[Index]; } }
        public Podr_class this[uint ID]
        {
            get
            {
                int Index = this.GetIndex(ID);

                if (Index > -1)
                { return Podrs_orig[Index]; }
                else
                { return null; }
            }
        }
        public int Count { get { return Podrs.Length; } }

        public int GetIndex(uint ID) { return this.Podr.Rows.GetIndex(ID); }
        public class Podr_class
        {
            public Podr_class(Podrs_class Parent, uint ID)
            {
                this.Parent = Parent;
                this.Row = Parent.Podr.Rows.Get_Row(ID);
            }

            Podrs_class Parent;
            public Podr_class From;
            public List<Podr_class> To = new List<Podr_class>();
            public int Index { get { return Row.Index; } }

            readonly DataBase.ISRecord Row;
            public uint ID { get { return Row.ID; } }
            public data.PSG PSG { get { return (data.PSG)T.Podr.Rows.Get_UnShow<uint>(ID, C.Podr.PSG); } }
            public string FullName { get { return T.Podr.Rows.Get<string>(ID, C.Podr.FllName); } }
            public string ShortName { get { return T.Podr.Rows.Get<string>(ID, C.Podr.ShrName); } }
            public List<PodrPpl_class> Ppls = new List<PodrPpl_class>();

            public class PodrPpl_class
            {
                public PodrPpl_class(uint ID)
                {
                    this.Row = G.PodrPpl.Rows.Get_Row(ID);
                }
                DataBase.ISRecord Row;
                public uint PodrPpl { get { return Row.ID; } }
                public uint PeopleID { get { return T.PodrPpl.Rows.Get_UnShow<uint>(Row.ID, C.PodrPpl.People); } }
                public uint ProfessionID { get { return T.PodrPpl.Rows.Get_UnShow<uint>(Row.ID, C.PodrPpl.People, C.People.Prfssn); } }
                public string Profession { get { return T.PodrPpl.Rows.Get<string>(Row.ID, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.Name); } }
                public data.PnMean PnMeanID { get { return (data.PnMean)T.PodrPpl.Rows.Get_UnShow<uint>(Row.ID, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.PnMean); } }
                public string PeopleName { get { return Misc.GetShortFIO(T.PodrPpl.Rows.Get_UnShow<uint>(Row.ID, C.PodrPpl.People)); } }
                public int PodrPplIndex { get { return Row.Index; } }
                public int PeopleIndex { get { return G.People.Rows.GetIndex(PeopleID); } }

                public override string ToString()
                {
                    return PeopleName + ' ' + Profession + "(" + PnMeanID.ToString() + ")";
                }
            }

            public int MainPplIndex
            {
                get
                {
                    for (int i = 0; i < this.Ppls.Count; i++)
                    {
                        if (this.Ppls[i].PnMeanID == data.PnMean.Nachalnic)
                        { return i; }
                    }

                    return -1;
                }
            }

            public override string ToString()
            {
                return ShortName + "(" + Ppls.Count.ToString() + ")" + ", From=" + (this.From != null).ToString() + ", To=" + To.Count.ToString();
            }
        }
        Podr_class[] Podrs;
        Podr_class[] Podrs_orig;
        void AfterShow(DataBase.ISTable SubTable)
        {
            G.Prfssn.QUERRY().SHOW.DO();
            G.People.QUERRY().SHOW.DO();
            G.PodrPpl.QUERRY().SHOW.DO();

            Podrs = new Podr_class[this.Podr.Rows.Count];
            Podrs_orig = new Podr_class[this.Podr.Rows.Count];

            for (int i = 0; i < Podrs.Length; i++)
            {
                var newPodr = new Podr_class(this, this.Podr.Rows.GetID(i));
                Podrs[i] = newPodr;
                Podrs_orig[i] = newPodr;
            }

            for (int i = 0; i < Podrs.Length; i++)
            {
                var PodrIndex = this.Podr.Rows.GetIndex(this.Podr.Rows.Get_UnShow<uint>(Podrs[i].ID, C.Podr.PFrom));
                if (PodrIndex > -1)
                {
                    Podrs[i].From = Podrs[PodrIndex];
                    Podrs[PodrIndex].To.Add(Podrs[i]);
                }
            }

            for (int i = 0; i < G.PodrPpl.Rows.Count; i++)
            {
                var PodrIndex = this.Podr.Rows.GetIndex(G.PodrPpl.Rows.Get_UnShow<uint>(i, C.PodrPpl.Podr));
                if (PodrIndex < 0) continue;

                var NewPpl = new Podr_class.PodrPpl_class(G.PodrPpl.Rows.GetID(i));
                if (NewPpl.PnMeanID == data.PnMean.Nachalnic)
                    Podrs[PodrIndex].Ppls.Insert(0, NewPpl);
                else
                {
                    if (NewPpl.PnMeanID == data.PnMean.Zam && Podrs[PodrIndex].Ppls.Count > 0 && Podrs[PodrIndex].Ppls[0].PnMeanID == data.PnMean.Nachalnic)
                    { Podrs[PodrIndex].Ppls.Insert(1, NewPpl); }
                    else
                    { Podrs[PodrIndex].Ppls.Add(NewPpl); }
                }
            }

            Array.Sort(Podrs
                , (r1, r2) =>
                {
                    if (r1.From == null)
                    {
                        if (r2.From == null)
                        { return r2.To.Count.CompareTo(r1.To.Count); }
                        else
                        {
                            if (r1.To.Count == 0)
                            { return 1; }
                            else
                            { return -1; }
                        }
                    }
                    else if (r2.From == null)
                    {
                        if (r2.To.Count == 0)
                        { return -1; }
                        else
                        { return 1; }
                    }
                    else
                    { return r2.To.Count.CompareTo(r1.To.Count); }
                });
        }
        void Sort(DataBase.ISTable SubTable) { Array.Sort(Podrs, (it1, it2) => { return it1.Index.CompareTo(it2.Index); }); }
    }

    /// <summary>Показатели, со всеми данными для них</summary>
    public static Marks_class Marks;
    /// <summary>Методы для групп складов</summary>
    public static PSG_class PSG;
    /// <summary>Методы для месячных объёмов</summary>
    public static Volumes_class Volumes;
    /// <summary>Подраделения</summary>
    public static Podrs_class Podrs;
}
