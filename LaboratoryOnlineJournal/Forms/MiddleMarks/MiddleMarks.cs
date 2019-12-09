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
        /*public class MiddleMarks_class
        {
            public MiddleMarks_class()
            {
                SM = T.SM.CreateSubTable(false);
                SMS = T.SMS.CreateSubTable(false);
                SPoint = T.SPoint.CreateSubTable(false);
                MVolume = T.MVolume.CreateSubTable(false);
            }

            struct Summ_struct
            {
                public Summ_struct(double Summ, int Count)
                {
                    this._Summ = Summ;
                    this._Count = Count;
                }

                double _Summ;
                int _Count;
                public double Summ { get { return _Summ; } }
                public int Count { get { return _Count; } }

                public void Set(double Summ, int Count)
                {
                    this._Summ = Summ;
                    this._Count = Count;
                }
                public void Add(double Summ)
                {
                    if (Summ > 0)
                    {
                        this._Summ += Summ;
                        this._Count++;
                    }
                }

                public double Middle
                {
                    get
                    {
                        if (Count > 0)
                        { return Summ / Count; }
                        else
                        { return 0; }
                    }
                }

                public override string ToString()
                {
                    return "Summ=" + Summ.ToString() + ", Count=" + Count.ToString() + ", Middle=" + Middle.ToString();
                }
            }
            public struct Mark_struct
            {
                public Mark_struct(MiddleMarks_class Parent, uint MarkID)
                {
                    this.Parent = Parent;
                    this.MarkID = MarkID;
                    this._AllowToShow = false;
                }

                MiddleMarks_class Parent;

                int Index { get { return RCache.Marks[MarkID].Number - 1; } }
                uint MarkID;
                public string MarkName { get { return RCache.Marks[MarkID].Name; } }
                public string EdType { get { return RCache.Marks[MarkID].EdType; } }
                public string OPType { get { return RCache.Marks[MarkID].OPType; } }
                public int MarkCode { get { return RCache.Marks[MarkID].Code; } }

                public int GetMonthMarkCount(int VGIndex, byte MonthNumber)
                {
                    switch (MonthNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11: break;
                        default: throw new Exception("Не верный номер месяца: " + MonthNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    return Parent.Summs[Index, MonthNumber, VGIndex].Count;
                }
                public double GetMonthSumm(int VGIndex, byte MonthNumber)
                {
                    switch (MonthNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11: break;
                        default: throw new Exception("Не верный номер месяца: " + MonthNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    return Parent.Summs[Index, MonthNumber, VGIndex].Summ;
                }
                public double GetMiddleMonthSumm(int VGIndex, byte MonthNumber)
                {
                    switch (MonthNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11: break;
                        default: throw new Exception("Не верный номер месяца: " + MonthNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    return Parent.Summs[Index, MonthNumber, VGIndex].Middle;
                }
                public int GetQuartalMarkCount(int VGIndex, byte QuartalNumber)
                {
                    switch (QuartalNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            break;
                        default: throw new Exception("Не верный номер квартала: " + QuartalNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    int MCount = 0;

                    for (int i = 0; i < 3; i++)
                    { MCount += Parent.Summs[Index, QuartalNumber * 3 + i, VGIndex].Count; }

                    return MCount;
                }
                public double GetMiddleQuartalSumm(int VGIndex, byte QuartalNumber)
                {
                    switch (QuartalNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            break;
                        default: throw new Exception("Не верный номер квартала: " + QuartalNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    int MCount = 0;
                    double MSumm = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        if (Parent.Summs[Index, QuartalNumber * 3 + i, VGIndex].Middle > 0)
                        {
                            MSumm += Parent.Summs[Index, QuartalNumber * 3 + i, VGIndex].Middle;
                            MCount++;
                        }
                    }

                    if (MCount > 0)
                    { return MSumm / MCount; }
                    else
                    { return 0; }
                }
                public double GetQuartalSumm(int VGIndex, byte QuartalNumber)
                {
                    switch (QuartalNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            break;
                        default: throw new Exception("Не верный номер квартала: " + QuartalNumber.ToString());
                    }
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    double MSumm = 0;

                    for (int i = 0; i < 3; i++)
                    { MSumm += Parent.Summs[Index, QuartalNumber * 3 + i, VGIndex].Summ; }

                    return MSumm;
                }
                public double YearSumm(int VGIndex)
                {
                    if (Parent.VGroup[VGIndex] > 0)
                    { VGIndex = RCache.Volumes.GetIndex(Parent.VGroup[VGIndex]); }
                    else
                    { VGIndex = RCache.Volumes.Count; }

                    int MCount = 0;
                    double MSumm = 0;

                    for (int i = 0; i < 12; i++)
                    {
                        if (Parent.Summs[Index, i, VGIndex].Middle > 0)
                        {
                            MSumm += Parent.Summs[Index, i, VGIndex].Middle;
                            MCount++;
                        }
                    }

                    if (MCount > 0)
                    { return MSumm / MCount; }
                    else
                    { return 0; }
                }

                public string MonthToolTip(int VGIndex, byte MonthNumber)
                {
                    var MCount = GetMonthMarkCount(VGIndex, MonthNumber);
                    if (MCount > 0)
                    { return GetMonthSumm(VGIndex, MonthNumber).ToString() + "/" + MCount + "=" + GetMiddleMonthSumm(VGIndex, MonthNumber).ToString(); }
                    else
                    { return ""; }
                }

                public string QuartalToolTip(int VGIndex, byte QNumber)
                {
                    var MCount = GetQuartalMarkCount(VGIndex, QNumber);
                    if (MCount > 0)
                    { return GetQuartalSumm(VGIndex, QNumber).ToString() + "/" + MCount.ToString() + "=" + GetMiddleQuartalSumm(VGIndex, QNumber).ToString(); }
                    else
                    { return ""; }
                }

                bool _AllowToShow;
                public bool AllowToShow
                {
                    get { return _AllowToShow; }// && RCache.Marks[MarkID].OPTypeID > 0; }
                    internal set { _AllowToShow = value; }
                }

                public override string ToString()
                {
                    return MarkName;
                }
            }

            Mark_struct[] Marks;
            Summ_struct[, ,] Summs;
            uint[] VGroup;
            DataBase.ISTable SM, SMS, SPoint, MVolume;
            double[,] Volumes;

            public Mark_struct this[int Index]
            {
                get
                {
                    return Marks[Index];
                }
                set
                {
                    Marks[Index] = value;
                }
            }
            public int Length { get { return Marks.Length; } }
            public int VGCount { get { return VGroup.Length; } }
            public string VGName(int Index)
            {
                if (this.VGroup[Index] == 0)
                    return "-";
                else
                    return RCache.Volumes.GetName(RCache.Volumes.GetIndex(this.VGroup[Index]));
            }
            public string VGShortName(int Index)
            {
                if (this.VGroup[Index] == 0)
                    return "Прочие";
                else
                    return RCache.Volumes.GetShortName(RCache.Volumes.GetIndex(this.VGroup[Index]));
            }

            public uint VGID(int Index)
            {
                return this.VGroup[Index];
            }

            public void Reload(int Year, uint PodrID, uint OID)
            {
                this.ShowYear = Year;
                this.PodrID = PodrID;
                this.UseVGroup = UseVGroup;

                Marks = new Mark_struct[RCache.Marks.Count];
                Summs = new Summ_struct[Marks.Length, 12, RCache.Volumes.Count + 1];//последний ряд для показателей, которые небыли отнесены к группам
                Volumes = new double[12, RCache.Volumes.Count + 1];

                int DayFrom = ATMisc.GetYMDFromYearMonth(ShowYear, 1) - 1;
                int DayTo = ATMisc.GetYMDFromYearMonth(ShowYear + 1, 1);

                var SMQ = this.SM.QUERRY()
                        .SHOW
                            .WHERE
                            .AC(C.SM.Amount).More.BV<double>(0)
                            .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Union).EQUI.BV(data.User<uint>(C.User.UType) == (uint)data.UType.Union)
                            .AND.ARC(C.SM.Sample, C.Sample.CYMD).More.BV(DayFrom)
                            .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(DayTo)
                            .AND.ARC(C.SM.Sample, C.Sample.Number).More.BV(0)

                var SMSQ = this.SMS.QUERRY()
                         .SHOW
                             .WHERE
                             .ARC(C.SMS.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                             .AND.OB()
                                 .ARC(C.SMS.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                 .OR.ARC(C.SMS.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                             .CB()
                             .AND.ARC(C.SMS.SPoint, C.SPoint.Union).EQUI.BV(data.User<uint>(C.User.UType) == (uint)data.UType.Union);

                var SPID = this.SPoint.QUERRY()
                         .SHOW
                             .WHERE
                             .AC(C.SPoint.YMDS).Less.BV(DayTo)
                             .AND.OB()
                                 .AC(C.SPoint.YMDE).More.BV(DayFrom)
                                 .OR.AC(C.SPoint.YMDE).EQUI.BV(0)
                            .CB().AND.C(C.SPoint.Union, data.User<uint>(C.User.UType) == (uint)data.UType.Union);

                var VGMID = this.MVolume.QUERRY().SHOW.WHERE.AC(C.MVolume.YM).More.BV(Year * 12 - 1).AND.AC(C.MVolume.YM).Less.BV((Year + 1) * 12);

                if (UseVGroup)
                {
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true);
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true);
                    SPID.AND.ARC(C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true);
                }

                if (PodrID > 0)
                {
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID);
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Podr).EQUI.BV(PodrID);
                    SPID.AND.AC(C.SPoint.Podr).EQUI.BV(PodrID);
                }
                if (OID > 0)
                {
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object).EQUI.BV(OID);
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Object).EQUI.BV(OID);
                    SPID.AND.AC(C.SPoint.Object).EQUI.BV(OID);
                }


                SMQ.DO();
                SMSQ.DO();
                SPID.DO();

                for (int i = 0; i < this.SM.Rows.Count; i++)
                {
                    //показатель
                    var MID = this.SM.Rows.Get_UnShow<uint>(i, C.SM.Mark);
                    var SMID = this.SM.Rows.GetID(i);
                    var MIndex = RCache.Marks.GetMarkIndex(MID);

                    if (MIndex > -1)
                    {
                        MIndex = RCache.Marks[this.SM.Rows.Get_UnShow<uint>(i, C.SM.Mark)].Number - 1;

                        //месяц
                        int Month = ATMisc.GetMonthFromYMD(this.SM.Rows.Get_UnShow<int>(i, C.SM.Sample, C.Sample.CYMD)) - 1;

                        var VGIndex = RCache.Volumes.GetIndex(this.SM.Rows.Get_UnShow<uint>(i, C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom));

                        if (VGIndex < 0)
                        { VGIndex = RCache.Volumes.Count; }

                        Summs[MIndex, Month, VGIndex].Add(this.SM.Rows.Get_UnShow<double>(i, C.SM.Amount));
                    }
                }

                for (int i = 0; i < this.SMS.Rows.Count; i++)
                {
                    var MIndex = RCache.Marks.GetMarkIndex(this.SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark));

                    if (MIndex > -1)
                    { Marks[MIndex].AllowToShow = true; }
                }

                for (int i = 0; i < RCache.Marks.Count; i++)
                { Marks[i] = new Mark_struct(this, RCache.Marks[i].ID); }

                {
                    var VGroup = new List<uint>(RCache.Volumes.Count + 1);
                    var newOIDs = new List<uint>();

                    if (!UseVGroup)
                    { VGroup.Add(0); }

                    for (int i = 0; i < SPoint.Rows.Count; i++)
                    {
                        var MList = SPoint.Rows.Get<bool>(i, C.SPoint.IMLst);

                        for (int j = 0; j < Marks.Length; j++)
                        { Marks[j].AllowToShow = MList; }

                        if (SPoint.Rows.Get<bool>(i, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed))
                        { VGroup.Add(SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationFrom)); }

                        var newOID = SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object);

                        for (int j = 0; j < newOIDs.Count; j++)
                        {
                            if (newOIDs[j] == newOID)
                            { goto NextStep; }
                        }

                        newOIDs.Add(newOID);
                    NextStep: ;
                    }

                    this.OIDs = newOIDs.ToArray();
                    this.VGroup = VGroup.ToArray();

                    if (this.VGroup.Length > 0)
                    {
                        VGMID.AND.OB().C(C.MVolume.OLocation, this.VGroup[0]);
                        for (int i = 1; i < this.VGroup.Length; i++)
                        { VGMID.OR.C(C.MVolume.OLocation, this.VGroup[i]); }
                        VGMID.CB().DO();
                    }
                    else
                    {
                        MVolume.QUERRY().HIDE.DO();
                    }
                    if (!UseVGroup)
                    { Array.Sort(this.VGroup, 1, this.VGroup.Length - 1); }
                    else
                    { Array.Sort(this.VGroup); }
                }
                for (int i = 0; i < this.SMS.Rows.Count; i++)
                {
                    var MIndex = RCache.Marks.GetMarkIndex(this.SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark));

                    if (MIndex > -1)
                    { Marks[MIndex].AllowToShow = true; }
                }

                for (int i = 0; i < this.MVolume.Rows.Count; i++)
                {
                    var MVIndex = RCache.Volumes.GetIndex(this.MVolume.Rows.Get_UnShow<uint>(i, C.MVolume.OLocation));

                    if (MVIndex > -1)
                    {
                        int MonthIndex = this.MVolume.Rows.Get<int>(i, C.MVolume.YM) - (Year * 12);
                        Volumes[MonthIndex, MVIndex] = this.MVolume.Rows.Get<double>(i, C.MVolume.Volume);
                    }
                }
            }

            double MonthVolume(int VGIndex, byte MonthNumber)
            {
                if (VGroup[VGIndex] > 0)
                { VGIndex = RCache.Volumes.GetIndex(this.VGroup[VGIndex]); }
                else
                { VGIndex = RCache.Volumes.Count; }

                return this.Volumes[MonthNumber, VGIndex];
            }

            double QuartalVolume(int VGIndex, byte QNumber)
            {
                if (VGroup[VGIndex] > 0)
                { VGIndex = RCache.Volumes.GetIndex(this.VGroup[VGIndex]); }
                else
                { VGIndex = RCache.Volumes.Count; }

                return Volumes[QNumber * 3, VGIndex] + Volumes[QNumber * 3 + 1, VGIndex] + Volumes[QNumber * 3 + 2, VGIndex];
            }

            public int ShowYear { get; internal set; }
            public uint PodrID { get; internal set; }
            public uint[] OIDs { get; internal set; }
            public bool UseVGroup { get; internal set; }
        }*/

        public unsafe class MiddleMarks_class
        {
            public MiddleMarks_class(int Year, uint PodrID, uint ObjectID)
            {
                this.Year = Year;
                this.PodrID = PodrID;
                this.ObjectID = ObjectID;

                var SM = T.SM.CreateSubTable(false);
                var SMS = T.SMS.CreateSubTable(false);
                var Sample = T.Sample.CreateSubTable(false);
                var SPoint = T.SPoint.CreateSubTable(false);

                var Mark = T.Mark.CreateSubTable(false);

                int DayFrom = ATMisc.GetYMDFromYearMonth(Year, 1) - 1;
                int DayTo = ATMisc.GetYMDFromYearMonth(Year + 1, 1);
                var Union = data.User<uint>(C.User.UType) == (uint)data.UType.Union;

                var SMQ = SM.QUERRY()
                        .SHOW
                            .WHERE
                            .AC(C.SM.Amount).More.BV<double>(0)
                            .AND.ARC(C.SM.Sample, C.Sample.CYMD).More.BV(DayFrom)
                            .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(DayTo)
                            .AND.ARC(C.SM.Sample, C.Sample.Number).More.BV(0)
                            .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                            .AND
                            .OB()
                                .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                            .CB()
                            .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Union).EQUI.BV(Union);

                var SMSQ = SMS.QUERRY()
                         .SHOW
                             .WHERE
                             .ARC(C.SMS.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                             .AND
                             .OB()
                                 .ARC(C.SMS.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                 .OR.ARC(C.SMS.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                             .CB()
                             .AND.ARC(C.SMS.SPoint, C.SPoint.Union).EQUI.BV(Union);

                var SPQ = SPoint.QUERRY()
                         .SHOW
                             .WHERE
                             .AC(C.SPoint.YMDS).Less.BV(DayTo)
                             .AND
                             .OB()
                                 .AC(C.SPoint.YMDE).More.BV(DayFrom)
                                 .OR.AC(C.SPoint.YMDE).EQUI.BV(0)
                             .CB()
                             .AND.C(C.SPoint.Union, Union);

                var SQ = Sample.QUERRY()
                         .SHOW
                             .WHERE
                             .AC(C.Sample.Number).More.BV<int>(0)
                             .AND.ARC(C.Sample.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                             .AND
                             .OB()
                                 .ARC(C.Sample.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                 .OR.ARC(C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                             .CB()
                             .AND.AC(C.Sample.CYMD).More.BV(DayFrom)
                             .AND.AC(C.Sample.CYMD).Less.BV(DayTo)
                             .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(Union);

                if (PodrID > 0)
                {
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID);
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Podr).EQUI.BV(PodrID);
                    SPQ.AND.AC(C.SPoint.Podr).EQUI.BV(PodrID);
                    SQ.AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID);
                }

                if (ObjectID > 0)
                {
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object).EQUI.BV(ObjectID);
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Object).EQUI.BV(ObjectID);
                    SPQ.AND.AC(C.SPoint.Object).EQUI.BV(ObjectID);
                    SQ.AND.ARC(C.Sample.SPoint, C.SPoint.Object).EQUI.BV(ObjectID);
                }


                SMQ.DO();
                SMSQ.DO();
                SPQ.DO();
                SQ.DO();

                Mark.QUERRY().SHOW.WHERE.AC(C.Mark.Number).More.BV(0).DO();

                var MarkPerSP = new bool[SPoint.Rows.Count, Mark.Rows.Count];
                var SPSampleCount = new int[SPoint.Rows.Count, 12];
                var MarkIndexes = new int[Mark.Rows.Count];
                for (int i = 0; i < MarkIndexes.Length; i++)
                { MarkIndexes[i] = -1; }

                for (int i = 0; i < SMS.Rows.Count; i++)    //получаю список доступных показателей, по спискам показателей для точек отбора
                {
                    var SPRI = SPoint.Rows.GetIndex(SMS.Rows.Get_UnShow<uint>(i, C.SMS.SPoint));
                    var MID = SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark);

                    if (SPRI > -1)
                    {
                        var MRI = Mark.Rows.GetIndex(SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark));

                        if (MRI > -1)
                        {
                            MarkPerSP[SPRI, MRI] = true;
                            MarkIndexes[MRI] = 1;
                        }
                    }
                }

                for (int i = 0; i < SPoint.Rows.Count; i++) //получаю показатели по настройке точек отбора "Игнорировать список показателей"
                {
                    if (SPoint.Rows.Get<bool>(i, C.SPoint.IMLst))
                    {
                        for (int j = 0; j < MarkIndexes.Length; j++)
                        {
                            MarkIndexes[j] = 1;
                            MarkPerSP[i, j] = true;
                        }
                    }
                }

                {   //определяю список показателей вывода
                    int count = 0;

                    for (int i = 0; i < MarkIndexes.Length; i++)
                    {
                        if (MarkIndexes[i] > 0)
                        { MarkIndexes[i] = count++; }
                    }

                    Marks = new Mark_struct[count];
                }

                //определяю показатели, которые в любом случае надо выводить
                for (int i = 0; i < MarkIndexes.Length; i++)
                {
                    if (MarkIndexes[i] > -1 &&
                      ((data.VarType)Mark.Rows.Get_UnShow<uint>(i, C.Mark.VarType) == data.VarType.Bool || Mark.Rows.Get<bool>(i, C.Mark.ShowZr))) //булевый тип или настройка показателя
                    { Marks[MarkIndexes[i]] = new Mark_struct(Mark.Rows.GetID(i)); }
                }

                //считаю кол-во замеров по месяцам и точкам отборов
                for (int i = 0; i < Sample.Rows.Count; i++)
                {
                    if (Sample.Rows.Get<int>(i, C.Sample.Number) > 0)
                    {
                        var SPRI = SPoint.Rows.GetIndex(Sample.Rows.Get_UnShow<uint>(i, C.Sample.SPoint));

                        if (SPRI > -1)
                        {
                            int tmpYear, tmpMonth, tmpDay;

                            ATMisc.GetYearMonthDayFromYMD(Sample.Rows.Get<int>(i, C.Sample.CYMD), out tmpYear, out tmpMonth, out tmpDay);

                            SPSampleCount[SPRI, tmpMonth - 1]++;
                        }
                    }
                }

                //заношу концентрации
                for (int i = 0; i < SM.Rows.Count; i++)
                {
                    var SMID = SM.Rows.GetID(i);

                    var SPRI = SPoint.Rows.GetIndex(SM.Rows.Get_UnShow<uint>(i, C.SM.Sample, C.Sample.SPoint));

                    var MID = SM.Rows.Get_UnShow<uint>(i, C.SM.Mark);

                    if (SPRI > -1)
                    {
                        var SRI = Sample.Rows.GetIndex(SM.Rows.Get_UnShow<uint>(i, C.SM.Sample));

                        if (SRI > -1)
                        {
                            var MIndex = Mark.Rows.GetIndex(MID);

                            if (MIndex > -1 && MarkPerSP[SPRI, MIndex])
                            {
                                MIndex = MarkIndexes[MIndex];

                                if (!Marks[MIndex].Existed)
                                { Marks[MIndex] = new Mark_struct(MID); }

                                int tmpYear, tmpMonth, tmpDay;

                                ATMisc.GetYearMonthDayFromYMD(SM.Rows.Get<int>(i, C.SM.Sample, C.Sample.CYMD), out tmpYear, out tmpMonth, out tmpDay);

                                Marks[MIndex].AddAmount(tmpMonth - 1, (data.VarType)SM.Rows.Get_UnShow<uint>(i, C.SM.Mark, C.Mark.VarType), SM.Rows.Get<double>(i, C.SM.Amount));
                            }
                        }
                    }
                }

                //указываю кол-во замеров по месяцам и убираю не использованые показатели
                if (Marks.Length > 0)
                {
                    for (int i = 0; i < MarkIndexes.Length; i++)
                    {
                        if (MarkIndexes[i] > -1 && Marks[MarkIndexes[i]].Existed)
                        {
                            var SMRepMonth = new int[12];

                            for (int j = 0; j < SPoint.Rows.Count; j++)
                            {
                                if (MarkPerSP[j, i])
                                {
                                    for (int m = 0; m < 12; m++)    //месяцы
                                    { SMRepMonth[m] += SPSampleCount[j, m]; }
                                }
                            }

                            for (int j = 0; j < SMRepMonth.Length; j++)
                            { Marks[MarkIndexes[i]].SetMarksCount(SMRepMonth[j], j); }
                        }
                    }

                    int RemoveCount = 0;

                    for (int i = Marks.Length - 2; i > -1; i--)
                    {
                        if (!Marks[i].Existed)
                        {
                            Array.Copy(Marks, i + 1, Marks, i, Marks.Length - i - 1);
                            RemoveCount++;
                        }
                    }

                    if (!Marks[Marks.Length - 1].Existed)
                    { RemoveCount++; }

                    Array.Resize(ref Marks, Marks.Length - RemoveCount);
                }

                Array.Sort(Marks, (it1, it2) => it1.Number.CompareTo(it2.Number));
            }

            struct Object_struct
            {
                public Object_struct(uint ObjectID, int MIndex)
                {
                    this.ObjectID = ObjectID;
                    this.MIndex = MIndex;
                }

                uint ObjectID;
                public readonly int MIndex;

                public string Name { get { return T.Object.Rows.Get<string>(ObjectID, C.Object.Name); } }

                public override string ToString()
                {
                    return Name.ToString();
                }
            }

            struct Mark_struct
            {
                public Mark_struct(uint MarkID)
                {
                    this.MarkID = MarkID;

                    this.Monthes = new Month_struct[12];

                    this.Enabled = true;
                }

                public readonly uint MarkID;
                public bool Enabled;

                public bool Existed { get { return MarkID > 0; } }
                public string Name { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name); } }
                public string EdName { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.Name); } }
                public string MZero { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.MZero); } }
                public int Number { get { return T.Mark.Rows.Get<int>(MarkID, C.Mark.Number); } }
                public int Round { get { return T.Mark.Rows.Get<int>(MarkID, C.Mark.Round); } }

                public data.VarType VarType { get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.VarType); } }

                readonly Month_struct[] Monthes;

                public void SetMarksCount(int Count, int MonthIndex)
                {
                    switch (VarType)
                    {
                        case data.VarType.Bool:
                            Monthes[MonthIndex].SetCount(Count);
                            break;
                        case data.VarType.dbl:
                            break;
                        case data.VarType.i32:
                            break;
                    }
                }

                public double Quartal(int QuartalIndex)
                {
                    double Summ;

                    switch (QuartalIndex)
                    {
                        case 0:
                            Summ = (Month(0) + Month(1) + Month(2)); 
                            break;
                        case 1:
                            Summ = (Month(3) + Month(4) + Month(5)); 
                            break;
                        case 2:
                            Summ = (Month(6) + Month(7) + Month(8));
                            break;
                        case 3:
                            Summ = (Month(9) + Month(10) + Month(11));
                            break;
                        default: throw new IndexOutOfRangeException();
                    }

                    Summ /= 3;

                    switch (VarType)
                    { 
                        case data.VarType.Bool:
                            return Summ;
                        case data.VarType.dbl:
                            if (Round > 0)
                            { return Math.Round(Summ, Round); }
                            else
                            { return Summ; }
                        case data.VarType.i32:
                            return Math.Round(Summ);
                        default:
                            throw new Exception("Не известный тип");
                    }
                }

                public double Month(int MonthIndex)
                {
                    switch (VarType)
                    {
                        case data.VarType.Bool:
                            return Monthes[MonthIndex].Middle; 
                        case data.VarType.dbl:
                            if (Round > 0)
                            { return Math.Round(Monthes[MonthIndex].Middle, Round); }
                            else
                            { return Monthes[MonthIndex].Middle; }
                        case data.VarType.i32:
                            return Math.Round(Monthes[MonthIndex].Middle);
                        default:
                            throw new Exception("Не известный тип");
                    }
                }

                public string MonthDescription(int MonthIndex)
                { return Month(MonthIndex).ToString() + 'x' + Monthes[MonthIndex].AddCount.ToString(); }

                public string QuartalDescription(int QuartalIndex)
                {
                    switch (QuartalIndex)
                    {
                        case 0:
                            return Quartal(QuartalIndex).ToString() + 'x' + (Monthes[0].AddCount + Monthes[1].AddCount + Monthes[2].AddCount).ToString();
                        case 1:
                            return Quartal(QuartalIndex).ToString() + 'x' + (Monthes[3].AddCount + Monthes[4].AddCount + Monthes[5].AddCount).ToString();
                        case 2:
                            return Quartal(QuartalIndex).ToString() + 'x' + (Monthes[6].AddCount + Monthes[7].AddCount + Monthes[8].AddCount).ToString();
                        case 3:
                            return Quartal(QuartalIndex).ToString() + 'x' + (Monthes[9].AddCount + Monthes[10].AddCount + Monthes[11].AddCount).ToString();
                        default: throw new IndexOutOfRangeException();
                    }
                }

                struct Month_struct
                {
                    public Month_struct(int MonthIndex)
                    {
                        this.MonthIndex = MonthIndex;
                        this._Summ = 0;
                        this._AddCount = 0;
                    }

                    public readonly int MonthIndex;

                    int _AddCount;
                    double _Summ;

                    public double Summ { get { return _Summ; } }
                    public int AddCount { get { return _AddCount; } }
                    public double Middle
                    { 
                        get 
                        { 
                            return (AddCount > 0 ? Summ / AddCount : 0); 
                        }
                    }

                    public void AddSumm(double Amount)
                    {
                        _Summ += Amount;
                        _AddCount++;
                    }

                    public void SetCount(int Count)
                    {
                        if (this._AddCount > Count)
                        { throw new Exception("Не верное количество концентраций"); }
                        else
                        { this._AddCount = Count; }
                    }
                }

                public void AddAmount(int MonthIndex, data.VarType VarType, double Amount)
                {
                    switch (VarType)
                    {
                        case data.VarType.Bool:
                            if (Amount > 0)
                            { Amount = 1; }
                            else
                            { Amount = 0; }

                            Monthes[MonthIndex].AddSumm(Amount);
                            break;
                        case data.VarType.dbl:
                            if (Amount > 0)
                            { Monthes[MonthIndex].AddSumm(Amount); }
                            break;
                        case data.VarType.i32:
                            if (Amount > 0)
                            { Monthes[MonthIndex].AddSumm(Math.Round(Amount)); }
                            break;
                    }
                }

                public double Year
                { 
                    get 
                    {
                        var Summ = (Month(0) + Month(1) + Month(2) + Month(3) + Month(4) + Month(5) + Month(6) + Month(7) + Month(8) + Month(9) + Month(10) + Month(11)) / 12;
                        
                        switch (VarType)
                        {
                            case data.VarType.Bool:
                                return Summ;
                            case data.VarType.dbl:
                                if (Round > 0)
                                { return Math.Round(Summ, Round); }
                                else
                                { return Summ; }
                            case data.VarType.i32:
                                return Math.Round(Summ);
                            default:
                                throw new Exception("Не известный тип");
                        }
                    }
                }

                public string YearDecription
                {
                    get
                    {
                        return (Year).ToString() + 'x'
                    + (Monthes[0].AddCount 
                     + Monthes[1].AddCount 
                     + Monthes[2].AddCount
                     + Monthes[3].AddCount
                     + Monthes[4].AddCount
                     + Monthes[5].AddCount
                     + Monthes[6].AddCount
                     + Monthes[7].AddCount
                     + Monthes[8].AddCount
                     + Monthes[9].AddCount
                     + Monthes[10].AddCount
                     + Monthes[11].AddCount).ToString();
                }
                }

                public override string ToString()
                {
                    return Name.ToString();
                }
            }

            /// <summary>Получить показатель. Первый индекс - номер объекта, второй индекс - номер показателя</summary>
            Mark_struct[] Marks;
            Object_struct[] O;

            public readonly int Year;
            public readonly uint PodrID;
            public readonly uint ObjectID;

            public double YearSumm(int MIndex)
            { return Marks[MIndex].Year; }

            public double MonthSumm(int MIndex, int MonthIndex)
            { return Marks[MIndex].Month(MonthIndex); }

            public double QuartalSumm(int MIndex, int QuartalIndex)
            { return Marks[MIndex].Quartal(QuartalIndex); }

            public string YearDesc(int MIndex)
            { return Marks[MIndex].YearDecription; }

            public string MonthDesc(int MIndex, int MonthIndex)
            { return Marks[MIndex].MonthDescription(MonthIndex); }

            public string QuartalDesc(int MIndex, int QuartalIndex)
            { return Marks[MIndex].QuartalDescription(QuartalIndex); }

            public int MarkCount { get { return Marks.Length; } }

            public string MarkName(int MIndex)
            { return Marks[MIndex].Name; }

            public string EdName(int MIndex)
            { return Marks[MIndex].EdName; }

            public uint MarkID(int MIndex)
            { return Marks[MIndex].MarkID; }

            public data.VarType VarType(int MarkIndex)
            { return Marks[MarkIndex].VarType; }

            public string MZero(int MarkIndex)
            { return Marks[MarkIndex].MZero; }

            public void SetMarkEnabled(int MIndex, bool Enabled)
            { this.Marks[MIndex].Enabled = Enabled; }

            public bool GetMarkEnabled(int MIndex)
            { return this.Marks[MIndex].Enabled; }
        }
    }
}