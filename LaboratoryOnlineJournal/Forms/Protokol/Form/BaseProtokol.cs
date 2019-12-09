using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPOI.HSSF.UserModel;
//using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;

namespace LaboratoryOnlineJournal
{
    public static partial class Misc
    {
        /// <summary>вывести в печатную форму протокол</summary>
        /// <param name="Protokol"></param>
        /// <param name="Open"></param>
        /// <returns></returns>
        public static bool OtchProtokol(Protokols_class.SGroup_class.Protokol_class Protokol, bool CreateNew = true, bool Open = true)
        {
            switch (Protokol.SGroup)
            {
                case data.SGroup.Group1:
                case data.SGroup.Middle1:
                case data.SGroup.NotGroup1:
                    return OtchProtokolType1(Protokol, CreateNew, Open);
                case data.SGroup.Group2:
                    return OtchProtokolType2(Protokol, CreateNew, Open);
                case data.SGroup.Group3:
                    return OtchProtokolType3(Protokol, CreateNew, Open);
                case data.SGroup.Middle4:
                    return OtchProtokolType4(Protokol, CreateNew, Open);
                case data.SGroup.AquaAurat:
                case data.SGroup.KOCA:
                    return OtchProtokolAquaAurat_KOCA(Protokol, CreateNew, Open);
                case data.SGroup.Toxicity1:
                case data.SGroup.Toxicity2:
                    return OtchProtokolToxicity1(Protokol, CreateNew, Open);
            }

            return false;
        }

        static string ProtokolFileName(Protokols_class.SGroup_class.Protokol_class Protokol)
        {
            string PreName;
            switch (Protokol.SGroup)
            {
                case data.SGroup.Group1:
                case data.SGroup.NotGroup1:
                case data.SGroup.Middle1:
                case data.SGroup.Middle4:
                    PreName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName);
                    break;
                case data.SGroup.Group2:
                case data.SGroup.Group3:
                case data.SGroup.AquaAurat:
                case data.SGroup.KOCA:
                case data.SGroup.Toxicity1:
                case data.SGroup.Toxicity2:
                    PreName = T.SGroup.Rows.Get<string>((uint)Protokol.SGroup, C.SGroup.ShrName);
                    break;
                default: throw new Exception("Неизвестный тип протокола");
            }
            int Month, Year;

            ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out Year, out Month);

            return "Протокол испытаний № " + Protokol.Number.ToString() + '-' + PreName + ' ' + Month.ToString() + '_' + Year.ToString() + ".xls";
        }

        static string ProtokolNumber(Protokols_class.SGroup_class.Protokol_class Protokol)
        {
            string PreName;
            switch (Protokol.SGroup)
            {
                case data.SGroup.Group1:
                case data.SGroup.NotGroup1:
                case data.SGroup.Middle1:
                case data.SGroup.Middle4:
                    PreName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName);
                    break;
                case data.SGroup.Group2:
                case data.SGroup.Group3:
                case data.SGroup.AquaAurat:
                case data.SGroup.KOCA:
                case data.SGroup.Toxicity1:
                case data.SGroup.Toxicity2:
                    PreName = T.SGroup.Rows.Get<string>((uint)Protokol.SGroup, C.SGroup.ShrName);
                    break;
                default: throw new Exception("Неизвестный тип протокола");
            }
            int Month, Year;

            ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out Year, out Month);

            return Protokol.Number.ToString() + '-' + PreName + ' ' + Month.ToString() + '/' + Year.ToString();
        }

        static CellExchange_Class GetProtokolsExchanges(NPOI.SS.UserModel.ISheet Sheet
          , int Year
          , string ProtokolNumber
          , string Object
          , string ObjectType
          , string ProbeType
          , string Location
          , string DateO
          , string DateP
          , string TimeO
          , string Sampler
          , string Cause
          , string Probes
          , string Day
          , string MonthName
          , string MonthNumber
          , string Methods
          , string PoPoS
          , string PodrFull
          , string PodrShort
          , string PodrAdress
          , int ActNumber)
        {
            var Exchanges = new CellExchange_Class(Sheet);
            Exchanges.AddExchange("{год}", Year.ToString(), 5);
            Exchanges.AddExchange("{Номер протокола}", ProtokolNumber, 5);
            Exchanges.AddExchange("{объект}", Object, 5);
            Exchanges.AddExchange("{Тип объекта}", ObjectType, 5);
            Exchanges.AddExchange("{тип пробы}", ProbeType, 5);
            Exchanges.AddExchange("{Место отбора проб}", Location, 5);
            Exchanges.AddExchange("{дата отбора}", DateO, 5);
            Exchanges.AddExchange("{время отбора}", TimeO, 5);
            Exchanges.AddExchange("{дата испытаний}", DateP, 5);
            Exchanges.AddExchange("{Пробоотборщик}", Sampler, 5);
            Exchanges.AddExchange("{Цель испытания}", Cause, 5);
            Exchanges.AddExchange("{Маркировка}", Probes, 5);
            Exchanges.AddExchange("{День месяца}", Day, 5);
            Exchanges.AddExchange("{Месяц}", MonthName, 5);
            Exchanges.AddExchange("{Месяц номер}", MonthNumber, 5);
            Exchanges.AddExchange("{Методы}", Methods, 1);
            Exchanges.AddExchange("{Подразделение краткое}", PodrShort, 1);
            Exchanges.AddExchange("{Подразделение полное}", PodrFull, 1);
            Exchanges.AddExchange("{Адрес подразделения}", PodrAdress, 1);
            Exchanges.AddExchange("{План и место}", PoPoS, 1);
            Exchanges.AddExchange("{Акт отбора}", (ActNumber == 0) ? "" : "№" + ActNumber.ToString() + "-" + PodrShort + "/" + Year.ToString() + " от " + DateO, 5);

            return Exchanges;
        }
    }
}