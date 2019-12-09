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
        public interface ITable
        {
            int Year { get; }
            int Month { get; }

            string PodrName { get; }
            string OName { get; }

            IRow this[int RowIndex] { get; }

            int RowCount { get; }
        }

        public interface IMark
        {
            string MarkName { get; }
            string EdTypeIn { get; }
            string EdTypeOut { get; }
            string Kode { get; }

            double Fact { get; }
            double BackGrd { get; }
            double Calc { get; }
            double Mass { get; }
        }

        public interface IRow
        {
            string Name { get; }

            double Mass { get; }

            IMark this[int MarkIndex] { get; }

            int MarkCount { get; }
        }
        
        public class Podrs_class
        {
            public Podrs_class(uint PodrID, PeriodType type, int YMStart, uint OTypeID)
            {
                //объединение фронта и фона: 
                //1.место сброса(куда). для текущего подразделения
                //2.настройки(фронт-применять фон, фон-является фоном). настраивается \/
                //3.группа фона(опционально). для всех подразделений

                this.YMStart = YMStart;
                this.type = type;

                this.Podr = T.Podr.CreateSubTable(false);
                var SMS = T.SMS.CreateSubTable(false);
                var SM = T.SM.CreateSubTable(false);
                this.MVolume = T.MVolume.CreateSubTable(false);
                var SPoint = T.SPoint.CreateSubTable(false);
                var Sample = T.Sample.CreateSubTable(false);
                var OType = T.OType.CreateSubTable(false);
                this.OLocation = T.OLocation.CreateSubTable(false);
                this.Mark = T.Mark.CreateSubTable(false);

                this.Mark.QUERRY().SHOW.WHERE.AC(C.Mark.Number).More.BV(0).AND.AC(C.Mark.OPType).More.BV<int>(0).DO();
                this.Mark.Sort((it1, it2) => Mark.Rows.Get<int>(it1, C.Mark.Number).CompareTo(Mark.Rows.Get<int>(it2, C.Mark.Number)));

                OType.QUERRY().SHOW.DO();

                Marks = new Mark_struct[Mark.Rows.Count];

                for (int i = 0; i < Mark.Rows.Count; i++)
                { Marks[i] = new Mark_struct(Mark.Rows.GetID(i)); }

                int DayFrom = ATMisc.GetYMDFromYM(YMStart) - 1;
                int DayTo;
                int Year, Month;
                ATMisc.GetYearMonthFromYM(YMStart, out Year, out Month);
                int MonthCount;


                var MVQ = MVolume.QUERRY().SHOW.WHERE.AC(C.MVolume.Volume).More.BV<double>(0);

                switch (type)
                {
                    case PeriodType.Month:
                        MonthCount = 1;
                        PeriodNumber = Month;
                        DayTo = ATMisc.GetYMDFromYM(YMStart + 1);
                        MVQ.AND.C(C.MVolume.YM, YMStart);
                        break;
                    case PeriodType.Quartal:
                        MonthCount = 3;
                        PeriodNumber = (Month - Month % 3) / 3;
                        if (Month % 3 > 0)
                        { PeriodNumber++; }
                        DayTo = ATMisc.GetYMDFromYM(YMStart + 3);
                        MVQ.AND.AC(C.MVolume.YM).More.BV(YMStart - 1).AND.AC(C.MVolume.YM).Less.BV(YMStart + 3);
                        break;
                    case PeriodType.Year:
                        MonthCount = 12;
                        PeriodNumber = Year;
                        DayFrom = ATMisc.GetYMDFromYM(YMStart) - 1;
                        DayTo = DayFrom + ATMisc.GetYMDFromYM(YMStart + 12);
                        MVQ.AND.AC(C.MVolume.YM).More.BV(YMStart - 1).AND.AC(C.MVolume.YM).Less.BV(YMStart + 12);
                        break;
                    default: throw new Exception("");
                }

                {
                    var SPQ = SPoint.QUERRY()
                                    .SHOW
                                    .WHERE
                                        .AC(C.SPoint.YMDS).Less.BV(DayTo)
                                        .AND.OB()
                                            .AC(C.SPoint.YMDE).More.BV(DayFrom)
                                            .OR.AC(C.SPoint.YMDE).EQUI.BV(0)
                                        .CB();

                    var SMSQ = SMS.QUERRY()
                                .SHOW
                                .WHERE
                                    .ARC(C.SMS.Mark, C.Mark.OPType).More.BV<uint>(0)
                                    .AND.OB()
                                        .ARC(C.SMS.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                                        .AND.OB()
                                            .ARC(C.SMS.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                            .OR.ARC(C.SMS.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                                        .CB();

                    var SMQ = SM.QUERRY()
                                .SHOW
                                .WHERE
                                    .ARC(C.SM.Mark, C.Mark.OPType).More.BV<uint>(0)
                                    .AND.OB()
                                        .ARC(C.SM.Sample, C.Sample.Number).More.BV<int>(0)
                                        .AND.ARC(C.SM.Sample, C.Sample.CYMD).More.BV(DayFrom)
                                        .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(DayTo)
                                        .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                                        .AND.OB()
                                            .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                            .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                                        .CB();

                    var SQ = Sample.QUERRY()
                                .SHOW
                                .WHERE
                                    .AC(C.Sample.Number).More.BV<int>(0)
                                    .AND.AC(C.Sample.CYMD).More.BV(DayFrom)
                                    .AND.AC(C.Sample.CYMD).Less.BV(DayTo)
                                    .AND.ARC(C.Sample.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                                    .AND.OB()
                                        .ARC(C.Sample.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                        .OR.ARC(C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                                    .CB();

                    var OLocTo = new List<uint>();
                    var OLocFrom = new List<uint>();

                    if (PodrID > 0)
                    {
                        Podr.QUERRY().SHOW.WHERE.ID(PodrID).DO();

                        SPQ.AND
                            .OB()
                                .AC(C.SPoint.BckGnd).EQUI.BV(true)
                                .AND.AC(C.SPoint.BackGrd).More.BV<uint>(0)
                                .OR.AC(C.SPoint.Podr).EQUI.BV(PodrID)
                                .AND.OB()
                                    .AC(C.SPoint.BckGnd).EQUI.BV(true)
                                    .OR.ARC(C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                    .AND.ARC(C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                                .CB()
                            .CB().DO();

                        SMSQ = SMSQ.AND
                            .OB()
                                .ARC(C.SMS.SPoint, C.SPoint.Podr).EQUI.BV(PodrID)
                                .AND.OB()
                                    .ARC(C.SMS.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                    .OR.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                    .AND.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                                .CB();
                        SMQ = SMQ.AND
                            .OB()
                                .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID)
                                .AND.OB()
                                    .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                    .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                    .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                                .CB();
                        SQ = SQ.AND
                            .OB()
                                .ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID)
                                .AND.OB()
                                    .ARC(C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                    .OR.ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                    .AND.ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                                .CB();

                        var BackGrd = new List<uint>();

                        for (int i = 0; i < SPoint.Rows.Count; i++)
                        {
                            if (SPoint.Rows.Get<bool>(i, C.SPoint.UsBckGnd))
                            {
                                var tmpPodrID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Podr);

                                if (tmpPodrID == PodrID)
                                {
                                    var BackGrdID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.BackGrd);
                                    var OLTID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationTo);

                                    if (BackGrdID > 0 && BackGrd.IndexOf(BackGrdID) < 0)
                                    { BackGrd.Add(BackGrdID); }

                                    if (OLocTo.IndexOf(OLTID) < 0)
                                    { OLocTo.Add(OLTID); }
                                }
                            }

                            if (SPoint.Rows.Get<bool>(i, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed))
                            {
                                var OLFID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationFrom);

                                if (OLocFrom.IndexOf(OLFID) < 0)
                                { OLocFrom.Add(OLFID); }
                            }
                        }

                        if (BackGrd.Count > 0)
                        {
                            SMSQ.OR
                                .ARC(C.SMS.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .AND.OB().ARC(C.SMS.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[0]);

                            SQ.OR
                                .ARC(C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .AND.OB().ARC(C.Sample.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[0]);
                            SMQ.OR
                                .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .AND.OB().ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[0]);

                            for (int i = 1; i < BackGrd.Count; i++)
                            {
                                SMSQ.OR.ARC(C.SMS.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[i]);
                                SQ.OR.ARC(C.Sample.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[i]);
                                SMQ.OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.BackGrd).EQUI.BV(BackGrd[i]);
                            }

                            SMSQ = SMSQ.CB();
                            SQ = SQ.CB();
                            SMQ = SMQ.CB();
                        }
                    }
                    else
                    {
                        {
                            var User = T.User.CreateSubTable(false);
                            User.QUERRY().SHOW.WHERE.AC(C.User.Podr).More.BV<uint>(0).AND.C(C.User.UType, (uint)data.UType.Employe).DO();

                            var PQ = Podr.QUERRY().SHOW.WHERE.ID(User.Rows.Get_UnShow<uint>(0, C.User.Podr));

                            for (int i = 1; i < User.Rows.Count; i++)
                            { PQ.OR.ID(User.Rows.Get_UnShow<uint>(i, C.User.Podr)); }

                            PQ.DO();
                        }

                        SPQ.AND
                            .OB()
                                .AC(C.SPoint.BckGnd).EQUI.BV(true)
                                .OR.ARC(C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                .AND.ARC(C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                            .CB().DO();

                        SMSQ = SMSQ.AND
                            .OB()
                                .ARC(C.SMS.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .OR.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                .AND.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                            .CB();
                        SMQ = SMQ.AND
                            .OB()
                                .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                            .CB();
                        SQ = SQ.AND
                            .OB()
                                .ARC(C.Sample.SPoint, C.SPoint.BckGnd).EQUI.BV(true)
                                .OR.ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                .AND.ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID)
                            .CB();

                        for (int i = 0; i < SPoint.Rows.Count; i++)
                        {
                            if (SPoint.Rows.Get<bool>(i, C.SPoint.UsBckGnd))
                            {
                                var tmpPodrID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Podr);

                                if (tmpPodrID == PodrID)
                                {
                                    var OLTID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationTo);

                                    if (OLocTo.IndexOf(OLTID) < 0)
                                    { OLocTo.Add(OLTID); }
                                }
                            }

                            if (SPoint.Rows.Get<bool>(i, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed))
                            {
                                var OLFID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationFrom);

                                if (OLocFrom.IndexOf(OLFID) < 0)
                                { OLocFrom.Add(OLFID); }
                            }
                        }
                    }

                    if (OLocTo.Count > 0)
                    {
                        SMSQ.AND.OB().ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[0]);
                        SQ.AND.OB().ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[0]);
                        SMQ.AND.OB().ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[0]);

                        for (int i = 1; i < OLocTo.Count; i++)
                        {
                            SMSQ.OR.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[i]);
                            SQ.OR.ARC(C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[i]);
                            SMQ.OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo).EQUI.BV(OLocTo[i]);
                        }

                        SMSQ = SMSQ.CB();
                        SQ = SQ.CB();
                        SMQ = SMQ.CB();
                    }

                    if (OLocFrom.Count > 0)
                    {
                        var OLQ = OLocation.QUERRY().SHOW.WHERE.ID(OLocFrom[0]);
                        MVQ.AND.OB().C(C.MVolume.OLocation, OLocFrom[0]);

                        for (int i = 1; i < OLocFrom.Count; i++)
                        {
                            MVQ.OR.C(C.MVolume.OLocation, OLocFrom[i]);
                            OLQ.OR.ID(OLocFrom[i]);
                        }

                        OLQ.DO();
                    }

                    SMSQ.DO();
                    SQ.DO();
                    SMQ.DO();
                    MVQ.DO();
                }

                {
                    var btVG = new ATMisc.BinaryTree<uint, List<Podr_class.VGroup_class>>();    //фон на выпуск(OLocationFrom/VGroup)

                    this.Podrs = new Podr_class[Podr.Rows.Count];

                    SPoint.Sort((it1, it2) =>
                    {
                        return SPoint.Rows.Get<bool>(it2, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).CompareTo(SPoint.Rows.Get<bool>(it1, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed));
                    });

                    for (int i = 0; i < SPoint.Rows.Count; i++)
                    {
                        var PID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Podr);
                        var OLTID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationTo);     //признак фона/фронта

                        if (PID == PodrID)
                        {
                            var PIndex = Podr.Rows.GetIndex(PID);

                            if (PIndex > -1)
                            {
                                var OLFID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationFrom);   //выпуск

                                if (Podrs[PIndex] == null)
                                { Podrs[PIndex] = new Podr_class(i, this, PID, OLFID); }

                                int VGIndex = OLocation.Rows.GetIndex(OLFID);

                                if (VGIndex > -1)
                                {
                                    this.Podrs[PIndex][VGIndex].SetSPoint(SPoint.Rows.GetID(i));

                                    var VList = btVG.GetValue(OLTID);

                                    if (VList == null)
                                    {
                                        VList = new List<Podr_class.VGroup_class>();

                                        btVG.Add(OLTID, VList);
                                    }

                                    VList.Add(Podrs[PIndex][VGIndex]);
                                }
                            }
                        }
                        else if (SPoint.Rows.Get<bool>(i, C.SPoint.UsBckGnd) && SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.BackGrd) > 0)
                        {
                            var VGroup = btVG.GetValue(OLTID);

                            if (VGroup != null)
                            { VGroup.ForEach((it) => it.SetSPoint(SPoint.Rows.GetID(i))); }
                        }
                    }
                }

                for (int i = 0; i < SMS.Rows.Count; i++)
                {
                    if (SPoint.Rows.GetIndex(SMS.Rows.Get_UnShow<uint>(i, C.SMS.SPoint)) > -1)
                    {
                        var SMSID = SMS.Rows.GetID(i);

                        for (int j = 0; j < this.Podrs.Length; j++)
                        {
                            if (Podrs[j].AddSMS(SMSID))
                            { break; }
                        }
                    }
                }

                for (int i = 0; i < SM.Rows.Count; i++)
                {
                    if (Sample.Rows.GetIndex(SM.Rows.Get_UnShow<uint>(i, C.SM.Sample)) > -1)
                    {
                        var SMID = SM.Rows.GetID(i);

                        for (int j = 0; j < this.Podrs.Length; j++)
                        { Podrs[j].AddSM(SMID); }
                    }
                }

                {
                    //фронт/фон номер месяца номер выпуска
                    var SampleCount = new int[2, this.Podrs.Length, MonthCount, OLocation.Rows.Count];

                    for (int i = 0; i < Sample.Rows.Count; i++)
                    {
                        var SPointID = Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint);

                        for (int j = 0; j < this.Podrs.Length; j++)
                        {
                            switch (this.Podrs[j].SPointIsIn(SPointID))
                            {
                                case Podr_class.WutUse.BackOverPodr:
                                    {
                                        var BackGrdID = Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint, C.SPoint.BackGrd);
                                        var OLOTID = Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo);
                                        int YM = ATMisc.GetYMFromYMD(Sample.Rows.Get<int>(i, C.Sample.CYMD));

                                        for (int k = 0; k < this.Podrs[j].Count; k++)
                                        {
                                            if (T.Object.Rows.Get_UnShow<uint>(this.Podrs[j][k].ObjectID, C.Object.OLocationTo) == OLOTID &&
                                                this.Podrs[j][k].BackGrdID == BackGrdID)
                                            { SampleCount[0, j, YM - YMStart, k]++; }
                                        }
                                        break;
                                    }
                                case Podr_class.WutUse.BackThisPodr:
                                    {
                                        var BackGrdID = Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint, C.SPoint.BackGrd);
                                        var OLOTID = Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo);
                                        int YM = ATMisc.GetYMFromYMD(Sample.Rows.Get<int>(i, C.Sample.CYMD));

                                        for (int k = 0; k < this.Podrs[j].Count; k++)
                                        {
                                            if (T.Object.Rows.Get_UnShow<uint>(this.Podrs[j][k].ObjectID, C.Object.OLocationTo) == OLOTID)
                                            { SampleCount[0, j, YM - YMStart, k]++; }
                                        }
                                        break;
                                    }
                                case Podr_class.WutUse.Front:
                                    {
                                        int YM = ATMisc.GetYMFromYMD(Sample.Rows.Get<int>(i, C.Sample.CYMD));

                                        var VGIndex = OLocation.Rows.GetIndex(Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom));

                                        if (VGIndex > -1)
                                        {
                                            SampleCount[1, j, YM - YMStart, VGIndex]++;
                                            goto NextSample;
                                        }
                                    }
                                    break;
                            }
                        }

                    NextSample: ;
                    }

                    for (int i = 0; i < this.Podrs.Length; i++)
                    {
                        for (int j = 0; j < this.Podrs[i].Count; j++)
                        {
                            if (this.Podrs[i][j].AllowToShow)
                            {
                                for (int k = 0; k < MVolume.Rows.Count; k++)
                                {
                                    if (MVolume.Rows.Get_UnShow<uint>(k, C.MVolume.OLocation) == this.Podrs[i][j].OLocationID)
                                    {
                                        this.Podrs[i][j].AddVolume(MVolume.Rows.Get<double>(k, C.MVolume.Volume));
                                    }
                                }

                                for (int m = 0; m < MonthCount; m++)
                                {
                                    this.Podrs[i][j].SetBackSampleCount(m, SampleCount[0, i, m, j]);
                                    this.Podrs[i][j].SetFrontSampleCount(m, SampleCount[1, i, m, j]);
                                }
                            }
                        }

                    }

                    var Podrs = new List<Podr_class>();

                    for (int i = 0; i < this.Podrs.Length; i++)
                    {
                        if (this.Podrs[i] != null)
                        {
                            this.Podrs[i].CheckVGroups();
                            Podrs.Add(this.Podrs[i]);
                        }
                    }

                    this.Podrs = Podrs.ToArray();
                }
            }

            public class Podr_class
            {
                public Podr_class(int Index, Podrs_class Parent, uint PodrID, uint OLocationID)
                {
                    this.Parent = Parent;
                    this.PodrID = PodrID;
                    this.Index = Index;
                    this.OLocationID = OLocationID;

                    this.VGroups = new VGroup_class[Parent.OLocation.Rows.Count];
                    for (int i = 0; i < VGroups.Length; i++)
                    { VGroups[i] = new VGroup_class(this, Parent.OLocation.Rows.GetID(i)); }
                }

                public class VGroup_class
                {
                    public VGroup_class(Podr_class Parent, uint OLocationID)
                    {
                        this.Parent = Parent;
                        this.OLocationID = OLocationID;
                        this.Marks = new Mark_class[Parent.Parent.Marks.Length];

                        for (int i = 0; i < Marks.Length; i++)
                        { Marks[i] = new Mark_class(this, Parent.Parent.Marks[i].MarkID); }
                    }
                    
                    public readonly Podr_class Parent;
                    public readonly uint OLocationID;

                    public class Mark_class
                    {
                        public Mark_class(VGroup_class Parent, uint MarkID)
                        {
                            this.Parent = Parent;
                            byte StartPeriodNumber = (byte)ATMisc.GetDateTimeFromYM(Parent.Parent.Parent.YMStart).Month;
                            switch (Parent.Parent.Parent.type)
                            {
                                case PeriodType.Month:
                                    this.Monthes = new MonthVolume_struct[1];
                                    break;
                                case PeriodType.Quartal:
                                    this.Monthes = new MonthVolume_struct[3];
                                    break;
                                case PeriodType.Year:
                                    this.Monthes = new MonthVolume_struct[12];
                                    break;
                            }

                            for (int i = 0; i < this.Monthes.Length; i++)
                            { this.Monthes[i] = new MonthVolume_struct(StartPeriodNumber++); }

                            this.MarkID = MarkID;
                        }

                        readonly VGroup_class Parent;
                        /// <summary>Среднее концентраций по месяцам</summary>
                        struct MonthVolume_struct
                        {
                            public MonthVolume_struct(byte MonthNumber)
                            {
                                this.MonthNumber = MonthNumber;
                                this._FVolumeSumm = 0;
                                this._FCount = 0;
                                this._BVolumeSumm = 0;
                                this._BCount = 0;
                            }

                            byte MonthNumber;
                            double _FVolumeSumm;
                            int _FCount;
                            double _BVolumeSumm;
                            int _BCount;

                            public int FCount { get { return _FCount; } internal set { _FCount = value; } }

                            public double FVolumeSumm { get { return _FVolumeSumm; } }

                            public int BCount { get { return _BCount; } internal set { _BCount = value; } }

                            public double BVolumeSumm { get { return _BVolumeSumm; } }

                            public void AddFVolume(double Volume)
                            {
                                _FVolumeSumm += Volume;
                                _FCount++;
                            }
                            public void AddBVolume(double Volume)
                            {
                                _BVolumeSumm += Volume;
                                _BCount++;
                            }

                            public void SetFCount(int Count)
                            {
                                if (Count < this._FCount)
                                { throw new Exception("Количество концентраций было больше, чем должно быть"); }

                                this._FCount = Count;
                            }
                            public void SetBCount(int Count)
                            {
                                if (Count < this._BCount)
                                { throw new Exception("Количество концентраций было больше, чем должно быть"); }

                                this._BCount = Count;
                            }

                            public double FMiddle { get { return FCount > 0 ? FVolumeSumm / FCount : 0; } }

                            public double BMiddle { get { return BCount > 0 ? BVolumeSumm / BCount : 0; } }

                            public override string ToString()
                            {
                                return ATMisc.GetMonthName1(MonthNumber) + ", Summ=" + FVolumeSumm.ToString() + '/' + BVolumeSumm.ToString() + ", MCount=" + _FCount.ToString() + '/' + _BCount.ToString() + ", Middle=" + FMiddle + '/' + BMiddle;
                            }
                        }

                        readonly MonthVolume_struct[] Monthes;

                        public readonly uint MarkID;

                        public void SetFSM(uint SMID)
                        {
                            var Amount = T.SM.Rows.Get<double>(SMID, C.SM.Amount);
                            var name = T.SM.Rows.Get<string>(SMID, C.SM.Mark);
                            int MonthIndex = ATMisc.GetDateTime(T.SM.Rows.Get<int>(SMID, C.SM.Sample, C.Sample.CYMD)).Month - 1;

                            switch (Parent.Parent.Parent.type)
                            {
                                case PeriodType.Month:
                                    MonthIndex = 0;
                                    break;
                                case PeriodType.Quartal:
                                    MonthIndex = MonthIndex - ((Parent.Parent.Parent.PeriodNumber - 1) * 3);
                                    break;
                                case PeriodType.Year:
                                    break;
                            }

                            switch ((data.VarType)T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark, C.Mark.VarType))
                            {
                                case data.VarType.Bool:
                                    Amount = (Amount > 0 ? 1 : 0);
                                    break;
                                case data.VarType.dbl:
                                    if (Amount > 0)
                                    {
                                        if (T.SM.Rows.Get<int>(SMID, C.SM.Mark, C.Mark.Round) > 0)
                                        { Amount = Math.Round(Amount, T.SM.Rows.Get<int>(SMID, C.SM.Mark, C.Mark.Round)); }

                                        if (Amount > 0)
                                        { break; }
                                    }

                                    return;
                                case data.VarType.i32:
                                    if (Amount > 0)
                                    {
                                        Amount = Math.Round(Amount);

                                        if (Amount > 0)
                                        { break; }
                                    }

                                    return;
                                default:
                                    throw new Exception("Не известный тип значения, показатель: " + T.SM.Rows.Get<string>(SMID, C.SM.Mark, C.Mark.Name) + ", концентрация: " + Amount);
                            }

                            Monthes[MonthIndex].AddFVolume(Amount);
                            var MIndex = Parent.Parent.Parent.Mark.Rows.GetIndex(MarkID);
                            Parent.Parent.Parent.Marks[MIndex].Enabled = true;

                            this.Parent.Parent.Parent.MarksCount++;
                            this.Parent.Parent.MarksCount++;
                            this.Parent.MarksCount++;
                        } // Sample Front? что за? Короч вроде выпуски

                        public void SetBSM(uint SMID)
                        {
                            var Amount = T.SM.Rows.Get<double>(SMID, C.SM.Amount);
                            var name = T.SM.Rows.Get<string>(SMID, C.SM.Mark);
                            int MonthIndex = ATMisc.GetDateTime(T.SM.Rows.Get<int>(SMID, C.SM.Sample, C.Sample.CYMD)).Month - 1;

                            switch (Parent.Parent.Parent.type)
                            {
                                case PeriodType.Month:
                                    MonthIndex = 0;
                                    break;
                                case PeriodType.Quartal:
                                    MonthIndex = MonthIndex - ((Parent.Parent.Parent.PeriodNumber - 1) * 3);
                                    break;
                                case PeriodType.Year:
                                    break;
                            }

                            switch ((data.VarType)T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark, C.Mark.VarType))
                            {
                                case data.VarType.Bool:
                                    Amount = (Amount > 0 ? 1 : 0);
                                    break;
                                case data.VarType.dbl:
                                    if (Amount > 0)
                                    {
                                        if (T.SM.Rows.Get<int>(SMID, C.SM.Mark, C.Mark.Round) > 0)
                                        { Amount = Math.Round(Amount, T.SM.Rows.Get<int>(SMID, C.SM.Mark, C.Mark.Round)); }

                                        if (Amount > 0)
                                        { break; }
                                    }

                                    return;
                                case data.VarType.i32:
                                    if (Amount > 0)
                                    {
                                        Amount = Math.Round(Amount);

                                        if (Amount > 0)
                                        { break; }
                                    }

                                    return;
                                default:
                                    throw new Exception("Не известный тип значения, показатель: " + T.SM.Rows.Get<string>(SMID, C.SM.Mark, C.Mark.Name) + ", концентрация: " + Amount);
                            }

                            Monthes[MonthIndex].AddBVolume(Amount);
                            var MIndex = Parent.Parent.Parent.Mark.Rows.GetIndex(MarkID);
                            Parent.Parent.Parent.Marks[MIndex].Enabled = true;

                            this.Parent.Parent.Parent.MarksCount++;
                            this.Parent.Parent.MarksCount++;
                            this.Parent.MarksCount++;
                        }

                        public void SetFSampleCount(int MonthIndex, int SampleCount)
                        {
                            if (T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.OPType, C.OPType.EdTypeT) > 0)
                            {
                                switch (VarType)
                                {
                                    case data.VarType.Bool:
                                        Monthes[MonthIndex].SetFCount(SampleCount);

                                        var MIndex = Parent.Parent.Parent.Mark.Rows.GetIndex(MarkID);
                                        Parent.Parent.Parent.Marks[MIndex].Enabled = true;

                                        this.Parent.Parent.Parent.MarksCount++;
                                        this.Parent.Parent.MarksCount++;
                                        this.Parent.MarksCount++;
                                        break;
                                }
                            }
                        }

                        public void SetBSampleCount(int MonthIndex, int SampleCount)
                        {
                            if (T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.OPType, C.OPType.EdTypeT) > 0)
                            {
                                switch (VarType)
                                {
                                    case data.VarType.Bool:
                                        Monthes[MonthIndex].SetBCount(SampleCount);

                                        var MIndex = Parent.Parent.Parent.Mark.Rows.GetIndex(MarkID);
                                        Parent.Parent.Parent.Marks[MIndex].Enabled = true;

                                        this.Parent.Parent.Parent.MarksCount++;
                                        this.Parent.Parent.MarksCount++;
                                        this.Parent.MarksCount++;
                                        break;
                                }
                            }
                        }

                        public string Name { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name); } }
                        public int Code { get { return T.Mark.Rows.Get<int>(MarkID, C.Mark.Code); } }
                        public string EdType { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.Name); } }
                        public string OPType { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.OPType, C.OPType.EdTypeT, C.EdType.Name); } }
                        public data.VarType VarType { get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.VarType); } }
                        public int Round { get { return T.Mark.Rows.Get<int>(MarkID, C.Mark.Round); } }

                        public bool ShowZero { get { return T.Mark.Rows.Get<bool>(MarkID, C.Mark.ShowZr); } }

                        public int FMCount
                        {
                            get
                            {
                                int Count = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                { Count += Monthes[i].FCount; }

                                return Count;
                            }
                        }

                        double FSumm
                        {
                            get
                            {
                                double Summ = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                { Summ += Monthes[i].FMiddle; }
                                //округлять не обязательно, т.к. концентрации попали туда уже обработанными
                                return Summ;
                            }
                        }

                        public double FMiddle
                        {
                            get
                            {
                                int Count = 0;
                                double Summ = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                {
                                    if (Monthes[i].FMiddle > 0)
                                    {
                                        Summ += Monthes[i].FMiddle;
                                        Count++;
                                    }
                                }

                                if (Round > 0)
                                { return Math.Round((Count > 0 ? Summ / Count : Summ), Round); }
                                else
                                { return (Count > 0 ? Summ / Count : Summ); }
                            }
                        }

                        public int BMCount
                        {
                            get
                            {
                                int Count = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                { Count += Monthes[i].BCount; }

                                return Count;
                            }
                        }

                        double BSumm
                        {
                            get
                            {
                                double Summ = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                { Summ += Monthes[i].BMiddle; }
                                //округлять не обязательно, т.к. концентрации попали туда уже обработанными
                                return Summ;
                            }
                        }

                        public double BMiddle
                        {
                            get
                            {
                                int Count = 0;
                                double Summ = 0;

                                for (int i = 0; i < Monthes.Length; i++)
                                {
                                    if (Monthes[i].BMiddle > 0)
                                    {
                                        Summ += Monthes[i].BMiddle;
                                        Count++;
                                    }
                                }

                                if (Round > 0)
                                { return Math.Round((Count > 0 ? Summ / Count : Summ), Round); }
                                else
                                { return (Count > 0 ? Summ / Count : Summ); }
                            }
                        }

                        public override string ToString()
                        {
                            return Name + "=" + FSumm.ToString() + "/" + BSumm.ToString() + ',' + FMCount.ToString() + '/' + BMCount.ToString() + ',' + FMiddle.ToString() + '/' + BMiddle.ToString();
                        }
                    }

                    Mark_class[] Marks;

                    void SetFSampleCount(int MonthIndex, int SampleCount)
                    {
                        for (int i = 0; i < Marks.Length; i++)
                        {
                            if (Parent.Parent.Marks[i].CanUse)
                            { Marks[i].SetFSampleCount(MonthIndex, SampleCount); }
                        }
                    }

                    void SetBSampleCount(int MonthIndex, int SampleCount)
                    {
                        for (int i = 0; i < Marks.Length; i++)
                        {
                            if (Parent.Parent.Marks[i].CanUse)
                            { Marks[i].SetBSampleCount(MonthIndex, SampleCount); }
                        }
                    }

                    public Mark_class this[int Index] { get { return Marks[Index]; } }

                    public Mark_class this[uint ID] { get { return Marks[Parent.Parent.Mark.Rows.GetIndex(ID)]; } }

                    public uint MarkID(int MarkIndex) { return Marks[MarkIndex].MarkID; }

                    public int Count { get { return Marks.Length; } }

                    /// <summary>Использовать фон других подразделений</summary>
                    public bool AllowMultyBack { get; internal set; }
                    /// <summary>количество занесенных значений</summary>
                    public int MarksCount { get; internal set; }

                    public void CheckMarks()
                    {
                        int DeleteCount = 0;

                        for (int i = this.Marks.Length-1; i >-1; i--)
                        {
                            if (this.Marks[i].BMCount + this.Marks[i].FMCount == 0)
                            {
                                if (this.Marks.Length - 1 > i)
                                { Array.Copy(this.Marks, i + 1, this.Marks, i, this.Marks.Length - i - 1); }
                                
                                DeleteCount++;
                            }
                        }

                        if (DeleteCount > 0)
                        { Array.Resize(ref this.Marks, this.Marks.Length - DeleteCount); }
                    }

                    public void SetFrontSampleCount(int MonthIndex, int SampleCount)
                    {
                        this.SetFSampleCount(MonthIndex, SampleCount);

                        this.MarksCount++;
                    }

                    public void SetBackSampleCount(int MonthIndex, int SampleCount)
                    {
                        this.SetFSampleCount(MonthIndex, SampleCount);

                        this.MarksCount++;
                    }

                    public void AddVolume(double Volume)
                    {
                        this.VolumeSumm += Volume;
                        VolumeCount++;
                    }

                    public int VolumeCount { get; internal set; }
                    public double VolumeSumm { get; internal set; }
                    public string VName { get { return T.OLocation.Rows.Get<string>(OLocationID, C.OLocation.Name); } }
                    public string VShortName { get { return T.OLocation.Rows.Get<string>(OLocationID, C.OLocation.ShrName); } }

                    public bool AllowToShow = false;
                    public uint ObjectID { get; internal set; }

                    public void SetSPoint(uint SPointID)
                    {
                        if (T.SPoint.Rows.Get<bool>(SPointID, C.SPoint.UsBckGnd))
                        {
                            this.AllowMultyBack = this.AllowMultyBack || T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.BackGrd) > 0;

                            this.ObjectID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object);

                            this.AllowToShow = true;

                            this.Parent.Parent.AllowBack = true;

                            if (BackGrdID == 0 || BackGrdID == T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.BackGrd))
                            { BackGrdID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.BackGrd); }
                            else
                            { throw new Exception("Уже установлена группа фона"); }
                        }
                        else if (T.SPoint.Rows.Get<bool>(SPointID, C.SPoint.BckGnd))
                        {
                            BackGrdID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.BackGrd);
                        }
                        else
                        {
                            this.ObjectID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object);

                            this.AllowToShow = true;
                        }
                    }

                    public uint BackGrdID = 0;

                    public override string ToString()
                    {
                        return VName + "=" + VolumeSumm.ToString();
                    }
                }

                public readonly int Index;
                readonly Podrs_class Parent;
                public readonly uint PodrID;
                public uint OLocationID;

                VGroup_class[] VGroups;

                public VGroup_class this[int Index] { get { return VGroups[Index]; } }

                /// <summary>Количество выпусков</summary>
                public int Count { get { return VGroups.Length; } }

                public int MarksCount { get; internal set; }

                public enum WutUse : byte { None, Front, BackThisPodr, BackOverPodr }

                public WutUse Check(uint MarkID, uint SPointID)
                {
                    if (Parent.Marks[Parent.Mark.Rows.GetIndex(MarkID)].CanUse && T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.OPType) > 0)
                    { return SPointIsIn(SPointID); }
                    else
                    { return WutUse.None; }
                }

                public WutUse SPointIsIn(uint SPointID)
                {
                    return (T.SPoint.Rows.Get<bool>(SPointID, C.SPoint.BckGnd) ?
                        (T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Podr) == this.PodrID ? WutUse.BackThisPodr : WutUse.BackOverPodr)
                        : WutUse.Front);
                }

                public bool AddSM(uint SMID)
                {
                    var MID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark);
                    var SPointID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint);

                    switch (Check(MID, SPointID))
                    {
                        case WutUse.BackOverPodr:
                            {
                                var MIndex = Parent.Mark.Rows.GetIndex(MID);
                                var BackGrdID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint, C.SPoint.BackGrd);
                                var OLOTID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo);

                                for (int i = 0; i < VGroups.Length; i++)
                                {
                                    if (T.Object.Rows.Get_UnShow<uint>(VGroups[i].ObjectID, C.Object.OLocationTo) == OLOTID &&
                                        VGroups[i].BackGrdID == BackGrdID)
                                    {
                                        VGroups[i][MIndex].SetBSM(SMID);
                                    }
                                }

                                return true;
                            }
                        case WutUse.BackThisPodr:
                            {
                                var MIndex = Parent.Mark.Rows.GetIndex(MID);
                                var BackGrdID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint, C.SPoint.BackGrd);
                                var OLOTID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo);

                                for (int i = 0; i < VGroups.Length; i++)
                                {
                                    if (T.Object.Rows.Get_UnShow<uint>(VGroups[i].ObjectID, C.Object.OLocationTo) == OLOTID)
                                    {
                                        VGroups[i][MIndex].SetBSM(SMID);
                                    }
                                }
                                return true;
                            }
                        case WutUse.Front:

                            var VGIndex = Parent.OLocation.Rows.GetIndex(T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom));

                            if (VGIndex > -1)
                            {
                                var MIndex = Parent.Mark.Rows.GetIndex(MID);

                                if (MIndex > -1)
                                {
                                    VGroups[VGIndex][MIndex].SetFSM(SMID);

                                    return true;
                                }
                            }
                            break;
                    }

                    return false;
                }

                public bool AddSMS(uint SMSID)
                {
                    uint MID = T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.Mark);

                    if (T.Mark.Rows.Get_UnShow<uint>(MID, C.Mark.OPType) > 0)
                    {
                        switch (SPointIsIn(T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint)))
                        {
                            case WutUse.BackOverPodr:
                                {
                                    var MIndex = Parent.Mark.Rows.GetIndex(MID);
                                    if (MIndex > -1)
                                    {
                                        var BackGrdID = T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.BackGrd);
                                        var OLOTID = T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationTo);

                                        for (int i = 0; i < VGroups.Length; i++)
                                        {
                                            if (T.Object.Rows.Get_UnShow<uint>(VGroups[i].ObjectID, C.Object.OLocationTo) == OLOTID &&
                                                VGroups[i].BackGrdID == BackGrdID)
                                            {
                                                Parent.Marks[MIndex].CanUse = true;
                                                break;
                                            }
                                        }

                                        return true;
                                    }

                                    break;
                                }
                            case WutUse.BackThisPodr:
                                {
                                    var MIndex = Parent.Mark.Rows.GetIndex(MID);
                                    if (MIndex > -1)
                                    {
                                        var BackGrdID = T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.BackGrd);
                                        var OLOTID = T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationTo);

                                        for (int i = 0; i < VGroups.Length; i++)
                                        {
                                            if (T.Object.Rows.Get_UnShow<uint>(VGroups[i].ObjectID, C.Object.OLocationFrom) == T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationFrom))
                                            {
                                                Parent.Marks[MIndex].CanUse = true;
                                                break;
                                            }
                                        }

                                        return true;
                                    }

                                    break;
                                }
                            case WutUse.Front:
                                {
                                    var VGIndex = Parent.OLocation.Rows.GetIndex(T.SMS.Rows.Get_UnShow<uint>(SMSID, C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationFrom));
                                    if (VGIndex > -1)
                                    {
                                        var MIndex = Parent.Mark.Rows.GetIndex(MID);

                                        if (MIndex > -1)
                                        {
                                            Parent.Marks[MIndex].CanUse = true;
                                            return true;
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    return false;
                }

                public void CheckVGroups()
                {
                    int DeleteCount = 0;

                    for (int i = VGroups.Length-1; i > -1; i--)
                    {
                        if (VGroups[i].AllowToShow && (VGroups[i].MarksCount > 0 || VGroups[i].VolumeCount > 0))
                        { VGroups[i].CheckMarks(); }
                        else
                        {
                            if (VGroups.Length - 1 > i)
                            { Array.Copy(VGroups, i + 1, VGroups, i, VGroups.Length - i - 1); }
                            DeleteCount++;
                        }
                    }

                    if (DeleteCount > 0)
                    { Array.Resize(ref VGroups, VGroups.Length - DeleteCount); }
                }

                public override string ToString()
                {
                    return T.Podr.Rows.Get<string>(PodrID, C.Podr.ShrName) + ", MC=" + MarksCount.ToString();
                }
            }

            struct Sample_struct
            {
                public Sample_struct(uint SID, int MarkCount)
                {
                    this.SID = SID;
                    this.MIDs = new uint[MarkCount];
                }

                public readonly uint SID;
                public readonly uint[] MIDs;
            }

            struct Mark_struct
            {
                public Mark_struct(uint MarkID)
                {
                    this.MarkID = MarkID;
                    this.Enabled = this.CanUse = false;
                }
                public readonly uint MarkID;
                public bool Enabled;
                public bool CanUse;

                public override string ToString()
                {
                    return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name) + '=' + CanUse.ToString() + '/' + Enabled.ToString();
                }
            }

            public enum PeriodType : byte { None, Month, Quartal, Year }

            readonly DataBase.ISTable Podr, Mark, MVolume, OLocation;
            readonly PeriodType type;
            readonly int YMStart;
            readonly int PeriodNumber;

            readonly Podr_class[] Podrs;
            readonly Mark_struct[] Marks;

            public bool AllowBack { get; internal set; }
            public int MarkCount { get { return Marks.Length; } }
            public int MarksCount { get; internal set; }

            public string PeriodName()
            {
                switch (type)
                {
                    case PeriodType.Month:
                        return ATMisc.GetMonthName1(PeriodNumber);
                    case PeriodType.Quartal:
                        return PeriodNumber.ToString() + " КВ";
                    case PeriodType.Year:
                        return PeriodNumber.ToString();
                }

                throw new Exception("Неизвестный тип " + type.ToString());
            }

            public bool ShowMark(int Index)
            {
                return Marks[Index].Enabled;
            }

            public uint MarkID(int Index)
            { return Marks[Index].MarkID; }

            public Podr_class this[int Index] { get { return Podrs[Index]; } }

            public int Count { get { return Podrs.Length; } }
        }
    }
}