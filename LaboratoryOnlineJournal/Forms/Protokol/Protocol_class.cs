using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaboratoryOnlineJournal
{
    public class Protokols_class
    {
        static Protokols_class()
        {
            Base_SGroups = new SGroup_class[Enum.GetNames(typeof(data.SGroup)).Length];
            Base_Criterions = new SGroup_class.ACriterions[BaseValues.COUNT];

            Base_Criterions[BaseValues.Podr] = new SGroup_class.Criterion_class<uint>(BaseValues.Podr, C.Prt.Podr, C.Sample.SPoint, new int[] { C.SPoint.Podr }, T.Podr, C.Podr.ShrName);
            Base_Criterions[BaseValues.Object] = new SGroup_class.Criterion_class<uint>(BaseValues.Object, C.Prt.Object, C.Sample.SPoint, new int[] { C.SPoint.Object }, T.Object, C.Object.Name);
            Base_Criterions[BaseValues.OLocation] = new SGroup_class.Criterion_class<uint>(BaseValues.OLocation, C.Prt.OLocation, C.Sample.SPoint, new int[] { C.SPoint.Object, C.Object.OLocationFrom }, T.OLocation, C.OLocation.Name);
            Base_Criterions[BaseValues.SGNum] = new SGroup_class.Criterion_class<uint>(BaseValues.SGNum, -1, C.Sample.SPoint, new int[] { C.SPoint.SGNum }, null, -1);
            Base_Criterions[BaseValues.Loc] = new SGroup_class.Criterion_class<uint>(BaseValues.Loc, -1, C.Sample.Loc, new int[0], null, -1);
            Base_Criterions[BaseValues.PaPoS] = new SGroup_class.Criterion_class<uint>(BaseValues.PaPoS, C.Prt.PaPoS, C.Sample.SPoint, new int[] { C.SPoint.PaPoS }, T.PaPoS, C.PaPoS.Name);
            Base_Criterions[BaseValues.PType] = new SGroup_class.Criterion_class<uint>(BaseValues.PType, -1, C.Sample.SPoint, new int[] { C.SPoint.PType }, T.PType, C.PType.Name);
            Base_Criterions[BaseValues.Area] = new SGroup_class.Criterion_class<uint>(BaseValues.Area, C.Prt.Area, C.Sample.SPoint, new int[] { C.SPoint.Area }, T.Area, C.Area.Name);

            Base_SGroups[(int)data.SGroup.Group1] = new SGroup_class((uint)data.SGroup.Group1);
            {
                Base_SGroups[(int)data.SGroup.Group1].AddCriteria(BaseValues.Podr);
                Base_SGroups[(int)data.SGroup.Group1].AddCriteria(BaseValues.Loc);
                Base_SGroups[(int)data.SGroup.Group1].AddCriteria(BaseValues.SGNum);
                Base_SGroups[(int)data.SGroup.Group1].AddCriteria(BaseValues.PaPoS);
            }
            Base_SGroups[(int)data.SGroup.Group2] = new SGroup_class((uint)data.SGroup.Group2);
            { Base_SGroups[(int)data.SGroup.Group2].SetCriteriaLike((int)data.SGroup.Group1); }

            Base_SGroups[(int)data.SGroup.Group3] = new SGroup_class((uint)data.SGroup.Group3);
            { Base_SGroups[(int)data.SGroup.Group3].SetCriteriaLike((int)data.SGroup.Group1); }

            Base_SGroups[(int)data.SGroup.Middle1] = new SGroup_class((uint)data.SGroup.Middle1);
            {
                Base_SGroups[(int)data.SGroup.Middle1].AddCriteria(BaseValues.PType);
                Base_SGroups[(int)data.SGroup.Middle1].AddCriteria(BaseValues.Podr);
                Base_SGroups[(int)data.SGroup.Middle1].AddCriteria(BaseValues.SGNum);
                Base_SGroups[(int)data.SGroup.Middle1].AddCriteria(BaseValues.Area);
                Base_SGroups[(int)data.SGroup.Middle1].AddCriteria(BaseValues.PaPoS);
            }

            Base_SGroups[(int)data.SGroup.Middle4] = new SGroup_class((uint)data.SGroup.Middle4);
            {
                Base_SGroups[(int)data.SGroup.Middle4].AddCriteria(BaseValues.PType);
                Base_SGroups[(int)data.SGroup.Middle4].AddCriteria(BaseValues.Podr);
                Base_SGroups[(int)data.SGroup.Middle4].AddCriteria(BaseValues.SGNum);
                Base_SGroups[(int)data.SGroup.Middle4].AddCriteria(BaseValues.PaPoS);
            }

            Base_SGroups[(int)data.SGroup.NotGroup1] = new SGroup_class((uint)data.SGroup.NotGroup1);
            { }

            Base_SGroups[(int)data.SGroup.AquaAurat] = new SGroup_class((uint)data.SGroup.AquaAurat);
            { }

            Base_SGroups[(int)data.SGroup.KOCA] = new SGroup_class((uint)data.SGroup.KOCA);
            { }

            Base_SGroups[(int)data.SGroup.Toxicity1] = new SGroup_class((uint)data.SGroup.Toxicity1);
            { }

            Base_SGroups[(int)data.SGroup.Toxicity2] = new SGroup_class((uint)data.SGroup.Toxicity2);
            { }
        }

        public Protokols_class(int YM, bool AllowGeneration, uint PodrID, bool Union)
        {
            this.YM = YM;
            uint[] SGroups;

            this.Union = Union;

            if (Union)
            {
                SGroups = new uint[]
                    {
                        (uint)data.SGroup.Group3, (uint)data.SGroup.Group2
                    };
            }
            else
            {
                SGroups = new uint[]
                    {
                        (uint)data.SGroup.Group1, 
                        (uint)data.SGroup.Middle1, 
                        (uint)data.SGroup.NotGroup1,
                        (uint)data.SGroup.AquaAurat,
                        (uint)data.SGroup.KOCA,
                        (uint)data.SGroup.Toxicity1,
                        (uint)data.SGroup.Toxicity2,
                    };
            }

            this.Criterions = new SGroup_class[SGroups.Length];

            for (int i = 0; i < SGroups.Length; i++)
            { this.Criterions[i] = GetSG(SGroups[i]); }

            DataBase.ISTable Sample = T.Sample.CreateSubTable(),
                             SPoint = T.SPoint.CreateSubTable(),
                             SM = T.SM.CreateSubTable(),
                             SMS = T.SMS.CreateSubTable();

            this.StartDay = ATMisc.GetYMDFromYM(YM) - 1;
            this.EndDay = this.StartDay + ATMisc.GetDaysInMonth(YM) + 1;

            {
                this.Mark.QUERRY().SHOW.WHERE.AC(C.Mark.Number).More.BV(0).DO();
                this.Mark.Get_Default();
                this.Mark.Sort(C.Mark.Number);

                var SPQ = SPoint.QUERRY()
                    .SHOW
                        .WHERE
                            .OB()
                            .AC(C.SPoint.YMDE).More.BV(StartDay)
                            .OR.C(C.SPoint.YMDE, 0)
                            .CB()
                        .AND.AC(C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.AC(C.SPoint.SGroup).More.BV<uint>(0);

                var SMSQ = SMS.QUERRY()
                    .SHOW
                        .WHERE
                        .OB()
                            .ARC(C.SMS.SPoint, C.SPoint.YMDE).More.BV(StartDay)
                            .OR.ARC(C.SMS.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                            .CB()
                        .AND.ARC(C.SMS.SPoint, C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.ARC(C.SMS.SPoint, C.SPoint.SGroup).More.BV<uint>(0);

                var SQ = Sample.QUERRY()
                    .SHOW
                        .WHERE
                        .AC(C.Sample.Number).More.BV(0)
                        .AND.ARC(C.Sample.SPoint, C.SPoint.SGroup).More.BV<uint>(0)
                        .AND.ARC(C.Sample.SPoint, C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.OB()
                            .ARC(C.Sample.SPoint, C.SPoint.YMDE).More.BV(StartDay)
                            .OR.ARC(C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                            .CB()
                        .AND.AC(C.Sample.CYMD).More.BV(StartDay)
                        .AND.AC(C.Sample.CYMD).Less.BV(EndDay);

                var SMQ = SM.QUERRY()
                    .SHOW
                        .WHERE
                        .ARC(C.SM.Sample, C.Sample.Number).More.BV(0)
                        .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.SGroup).More.BV<uint>(0)
                        .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.OB()
                            .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).More.BV(StartDay)
                            .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                            .CB()
                        .AND.ARC(C.SM.Sample, C.Sample.CYMD).More.BV(StartDay)
                        .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(EndDay);

                var PrtShow = G.Prt.QUERRY().SHOWL(C.Prt.Number).WHERE.C(C.Prt.YM, YM);
                var PrtSShow = G.PrtS.QUERRY().SHOW.WHERE.ARC(C.PrtS.Prt, C.Prt.YM).EQUI.BV(YM);

                PrtShow.AND.C(C.Prt.Union, Union);   //проблема тут
                PrtSShow.AND.ARC(C.PrtS.Prt, C.Prt.Union).EQUI.BV(Union);

                SPQ.AND.C(C.SPoint.Union, Union);
                SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Union).EQUI.BV(Union);
                SQ.AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(Union);
                SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Union).EQUI.BV(Union);

                if (PodrID > 0)
                {
                    PrtShow.AND.C(C.Prt.Podr, PodrID).DO();
                    PrtSShow.AND.ARC(C.PrtS.Prt, C.Prt.Podr).EQUI.BV<uint>(PodrID).DO();
                    SPQ.AND.C(C.SPoint.Podr, PodrID).DO();
                    SMSQ.AND.ARC(C.SMS.SPoint, C.SPoint.Podr).EQUI.BV<uint>(PodrID).DO();
                    SQ.AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV<uint>(PodrID).DO();
                    SMQ.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV<uint>(PodrID).DO();
                }
                else
                {
                    PrtShow.DO();
                    PrtSShow.DO();
                    SPQ.DO();
                    SMSQ.DO();
                    SQ.DO();
                    SMQ.DO();
                }
            }

            //
            var samples = new List<SGroup_class.Protokol_class.Sample_class>(Sample.Rows.Count);
            var orgsamples = new SGroup_class.Protokol_class.Sample_class[Sample.Rows.Count];

            for (int i = 0; i < Sample.Rows.Count; i++)
            {
                var news = new SGroup_class.Protokol_class.Sample_class(this, Sample.Rows.GetID(i));

                if (news.Status == DataBase.State.Normal)
                { samples.Add(news); }

                orgsamples[i] = news;
            }

            for (int i = 0; i < SM.Rows.Count; i++)
            {
                int SIndex = Sample.Rows.GetIndex(SM.Rows.Get_UnShow<uint>(i, C.SM.Sample));

                if (SIndex > -1)
                { orgsamples[SIndex].AddMark(SM.Rows.GetID(i)); }
            }

            int Number = G.Prt.Rows.Count;
            int MaxProto = 0;

            {//гружу сохраненные протоколы
                var Protoks = new List<SGroup_class.Protokol_class.Sample_class>[G.Prt.Rows.Count];

                for (int i = 0; i < Protoks.Length; i++)
                { Protoks[i] = new List<SGroup_class.Protokol_class.Sample_class>(); }

                for (int j = 0; j < G.PrtS.Rows.Count; j++)
                {
                    int PrtIndex = G.Prt.Rows.GetIndex(G.PrtS.Rows.Get_UnShow<uint>(j, C.PrtS.Prt));

                    if (PrtIndex > -1)
                    {
                        var SIndex = Sample.Rows.GetIndex(G.PrtS.Rows.Get_UnShow<uint>(j, C.PrtS.Sample));
                        if (SIndex > -1)
                        {
                            Protoks[PrtIndex].Add(orgsamples[SIndex]);

                            for (int i = 0; i < samples.Count; i++)
                            {
                                if (samples[i].SampleID == orgsamples[SIndex].SampleID)
                                {
                                    samples.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    { }
                }

                for (int i = 0; i < G.Prt.Rows.Count; i++)
                {
                    this.Protokols.Add(new SGroup_class.Protokol_class(this.Protokols.Count, this, G.Prt.Rows.GetID(i), Protoks[i]));
                    if (this.Protokols[this.Protokols.Count - 1].Number > MaxProto)
                    { MaxProto = this.Protokols[this.Protokols.Count - 1].Number; }
                }
            }
            //распределение замеров

            samples.Sort((it1, it2) =>
            {
                var ret = T.Sample.Rows.Get_UnShow<int>(it1.SampleID, C.Sample.SPoint, C.SPoint.Number).CompareTo(T.Sample.Rows.Get<int>(it2.SampleID, C.Sample.SPoint, C.SPoint.Number));

                if (ret == 0)
                { return T.Sample.Rows.Get_UnShow<int>(it1.SampleID, C.Sample.Loc).CompareTo(T.Sample.Rows.Get<int>(it2.SampleID, C.Sample.Loc)); }
                else
                { return ret; }
            });

            if (AllowGeneration)
            {   //генерирую новые протоколы
                var protokols = new List<SGroup_class.Protokol_class>();

                for (int i = 0; i < samples.Count; i++)
                {
                    for (int j = 0; j < protokols.Count; j++)
                    {
                        if (protokols[j].AddSample(samples[i]))
                        { goto NEXT_SAMPLE; }
                    }

                    protokols.Add(new SGroup_class.Protokol_class(this.Protokols.Count + protokols.Count, this, GetSG(samples[i].SGroupID), samples[i], protokols.Count + MaxProto + 1));

                NEXT_SAMPLE: ;

                    samples.RemoveAt(i);
                    i--;
                }

                this.Protokols.AddRange(protokols);
            }

            {//пилю разрешения для показа показателей
                var SPs = new List<SGroup_class.Protokol_class.Sample_class>[SPoint.Rows.Count];

                for (int i = 0; i < SPs.Length; i++)
                { SPs[i] = new List<SGroup_class.Protokol_class.Sample_class>(); }

                for (int i = 0; i < orgsamples.Length; i++)
                {
                    if (orgsamples[i].Parent != null)
                    {
                        int SPIndex = SPoint.Rows.GetIndex(orgsamples[i].SPointID);

                        if (SPIndex > -1)
                        {
                            SPs[SPIndex].Add(orgsamples[i]);
                            if (SPoint.Rows.Get_UnShow<bool>(SPIndex, C.SPoint.IMLst))
                            {
                                for (int j = 0; j < SPs[SPIndex].Count; j++)
                                {
                                    for (int k = 0; k < this.Mark.Rows.Count; k++)
                                    { SPs[SPIndex][j].Parent.SetTotalAlowMark(k, true); }
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < SMS.Rows.Count; i++)
                {
                    int SPIndex = SPoint.Rows.GetIndex(SMS.Rows.Get_UnShow<uint>(i, C.SMS.SPoint));

                    if (SPIndex > -1)
                    {
                        int MIndex = this.Mark.Rows.GetIndex(SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark));

                        if (MIndex > -1)
                        {
                            for (int j = 0; j < SPs[SPIndex].Count; j++)
                            {
                                SPs[SPIndex][j].Parent.SetTotalAlowMark(MIndex, true);
                                var Mark = SPs[SPIndex][j][MIndex];
                                Mark.LocalAlow = true;
                                SPs[SPIndex][j].SetLocalAlow(MIndex);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < orgsamples.Length; i++)
            { orgsamples[i].UpdateMCount(); }

            for (int i = 0; i < Protokols.Count; i++)
            { Protokols[i].GetTables(); }
        }

        /// <summary>Тип протокола</summary>
        public class SGroup_class
        {
            public SGroup_class(uint SGroupID)
            {
                this.SGroupID = SGroupID;
                this.Criterions = new ACriterions[0];
            }

            public abstract class ACriterions
            {
                public ACriterions(int Prt_Index, int PrtColumn, int SampleColumn, int[] SampleColumns, DataBase.ITable TargetTable, int TargetRowNameColumn)
                {
                    this.Prt_Index = Prt_Index;
                    this.PrtColumn = PrtColumn;
                    this.SampleColumn = SampleColumn;
                    this.SampleColumns = SampleColumns;
                    this.TargetRowNameColumn = TargetRowNameColumn;
                    this.TargetTable = TargetTable;
                }
                /// <summary>Колонка протокола, по которому проверяю соответсвует</summary>
                public readonly int Prt_Index;
                public readonly int PrtColumn;
                /// <summary>Колонка замера, где находится параметр для сравнения</summary>
                protected readonly int SampleColumn;
                /// <summary>Реляционный путь замера, где находится параметр для сравнения</summary>
                protected readonly int[] SampleColumns;
                /// <summary>Колонка таблицы, ведущая к текстовой идентификации записи параметра</summary>
                protected readonly int TargetRowNameColumn;
                protected readonly DataBase.ITable TargetTable;

                public abstract Type GetValue<Type>(uint SID);

                public abstract AValue SampleValueInit(uint SID);

                public abstract AValue ProtokolValueInit(Protokol_class Protokol, Protokol_class.Sample_class Sample);

                public abstract AValue ProtokolValueInit(Protokol_class Protokol, uint PrtID, Protokol_class.Sample_class Sample);

                public string GetRowName(AValue value)
                {
                    if (TargetTable == null)
                    { return value.GetStringValue; }
                    else
                    { return TargetTable.Rows.Get_UnShow<string>(value.GetValue<uint>(), TargetRowNameColumn); }
                }

                public override string ToString()
                {
                    if (SampleColumns.Length == 0)
                    { return T.Sample.GetColumn(SampleColumn).AlterName; }
                    else
                    {
                        var C = T.Sample.GetColumn(SampleColumn);

                        for (int i = 0; i < SampleColumns.Length; i++)
                        { C = C.RelatedTable.GetColumn(SampleColumns[i]); }

                        return C.AlterName;
                    }
                }
            }
            /// <summary>Критерий сравления</summary>
            public class Criterion_class<Type> : ACriterions
            {
                public Criterion_class(int Prt_Index, int PrtColumn, int SampleColumn, int[] Smpl_Columns, DataBase.ITable TargetTable, int TargetRowNameColumn)
                    : base(Prt_Index, PrtColumn, SampleColumn, Smpl_Columns, TargetTable, TargetRowNameColumn)
                { }

                public override GType GetValue<GType>(uint SID)
                {
                    if (SampleColumns.Length > 0)
                    { return T.Sample.Rows.Get_UnShow<GType>(SID, SampleColumn, SampleColumns); }
                    else
                    { return T.Sample.Rows.Get_UnShow<GType>(SID, SampleColumn); }
                }

                public override AValue SampleValueInit(uint SID)
                { return new Val_class<Type>(Prt_Index, this.GetValue<Type>(SID)); }

                public override AValue ProtokolValueInit(Protokol_class Protokol, Protokol_class.Sample_class Sample)
                { return new Val_class<Type>(Prt_Index, Sample.GetValue(Prt_Index).GetValue<Type>()); }

                public override AValue ProtokolValueInit(Protokol_class Protokol, uint PrtID, Protokol_class.Sample_class Sample)
                {
                    if (PrtColumn > -1)
                    { return new Val_class<Type>(Prt_Index, Sample.GetValue(Prt_Index).GetValue<Type>()); }
                    else
                    { return new Val_class<Type>(Prt_Index, G.Prt.Rows.Get<Type>(PrtID, PrtColumn)); }
                }

                /// <summary>Контейнер для хранения и получения Типизированных значений</summary>
                /// <typeparam name="Type">Тип значения</typeparam>
                public class Val_class<VType> : AValue
                {
                    public Val_class(int ValueIndex, VType Value)
                    {
                        base.ValueIndex = ValueIndex;
                        this.Value = Value;
                    }

                    VType Value;

                    public override T2 GetValue<T2>()
                    { return DataBase.C1<T2>.C<VType>.To(Value); }

                    public override string GetStringValue
                    { get { return Value.ToString(); } }

                    public override bool Check(AValue Value)
                    { return DataBase.C1<VType>.C<VType>.Equal(this.Value, Value.GetValue<VType>()); }

                    public override string ToString()
                    { return GetStringValue; }
                }


            }

            /// <summary>Протокол</summary>
            public class Protokol_class
            {
                Protokol_class(int Index, Protokols_class Parent, SGroup_class Criterion, int Number, int Act)
                {
                    this.Index = Index;
                    this.Parent = Parent;
                    this.Criterions = Criterion;
                    this.Values = new AValue[BaseValues.COUNT];
                    this.AM = new bool[Parent.Mark.Rows.Count];
                    this.Number = Number;
                    this._Act = Act;
                }
                public Protokol_class(int Index, Protokols_class Parent, SGroup_class Criterion, Sample_class Sample, int Number)
                    : this(Index, Parent, Criterion, Number, 0)
                {
                    for (int i = 0; i < Criterions.Count; i++)
                    { Values[Criterions[i].Prt_Index] = Criterions[i].ProtokolValueInit(this, Sample); }

                    this.AreaID = T.Sample.Rows.Get_UnShow<uint>(Sample.SampleID, C.Sample.SPoint, C.SPoint.Area);
                    this.ObjectID = T.Sample.Rows.Get_UnShow<uint>(Sample.SampleID, C.Sample.SPoint, C.SPoint.Object);
                    this.PodrID = T.Sample.Rows.Get_UnShow<uint>(Sample.SampleID, C.Sample.SPoint, C.SPoint.Podr);
                    this.OLocationID = T.Sample.Rows.Get_UnShow<uint>(Sample.SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom);
                    this.PaPoSID = T.Sample.Rows.Get_UnShow<uint>(Sample.SampleID, C.Sample.SPoint, C.SPoint.PaPoS);
                    //this.YM = ATMisc.GetYMFromYMD(Sample.AYMD);

                    Sample.Parent = this;
                    this.Samples.Add(Sample);
                }

                public Protokol_class(int Index, Protokols_class Parent, uint PrtID, List<Sample_class> Samples)
                    : this(Index, Parent, GetSG(T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.SGroup)), T.Prt.Rows.Get_UnShow<int>(PrtID, C.Prt.Number), T.Prt.Rows.Get<int>(PrtID, C.Prt.Taos))
                {
                    this.PrtID = PrtID;

                    if (Samples.Count > 0)
                    {
                        for (int i = 0; i < Samples.Count; i++)
                        {
                            Samples[i].Parent = this;
                            this.Samples.Add(Samples[i]);
                        }

                        for (int i = 0; i < Criterions.Count; i++)
                        { Values[Criterions[i].Prt_Index] = Criterions[i].ProtokolValueInit(this, Samples[0]); }
                    }

                    this._Time = T.Prt.Rows.Get_UnShow<int>(PrtID, C.Prt.Time);
                    this.AreaID = T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.Area);
                    this.ObjectID = T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.Object);
                    this.PodrID = T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.Podr);
                    this.OLocationID = T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.OLocation);
                    this.PaPoSID = T.Prt.Rows.Get_UnShow<uint>(PrtID, C.Prt.PaPoS);
                    //this.YM = T.Prt.Rows.Get<int>(PrtID, C.Prt.YM);
                }

                /// <summary>Замер</summary>
                public class Sample_class
                {
                    public Sample_class(Protokols_class Parent, uint SampleID)
                    {
                        this.Values = new AValue[BaseValues.COUNT];
                        this.SampleID = SampleID;

                        for (int i = 0; i < Base_Criterions.Length; i++)
                        { Values[i] = Base_Criterions[i].SampleValueInit(SampleID); }

                        this.Marks = new Mark_struct[Parent.Mark.Rows.Count];

                        for (int i = 0; i < this.Marks.Length; i++)
                        { this.Marks[i] = new Mark_struct(this, Parent.Mark.Rows.GetID(i)); }

                        this.Status = T.Sample.Rows.Status(SampleID);
                        if (this.Status == DataBase.State.Normal)
                        { this.Status = T.SPoint.Rows.Status(T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint)); }

                        this.pParent = Parent;
                    }
                    public Sample_class(Protokol_class Parent, uint PrtSID)
                        : this(Parent.Parent, T.PrtS.Rows.Get_UnShow<uint>(PrtSID, C.PrtS.Sample))
                    {
                        this.Parent = Parent;
                        this.PrtSID = PrtSID;
                        this.Status = T.PrtS.Rows.Status(PrtSID);
                    }

                    /// <summary>Показатель по замеру протокола</summary>
                    public struct Mark_struct
                    {
                        public Mark_struct(Sample_class Parent, uint SMID, uint MarkID)
                        {
                            this.Parent = Parent;
                            this.SMID = SMID;
                            this.MarkID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark);
                            this.LocalAlow = false;
                        }
                        public Mark_struct(Sample_class Parent, uint MarkID)
                        {
                            this.Parent = Parent;
                            this.SMID = 0;
                            this.MarkID = MarkID;
                            this.LocalAlow = false;
                        }

                        Sample_class Parent;
                        uint SMID;
                        public readonly uint MarkID;

                        public double Amount
                        {
                            get
                            {
                                if (SMID == 0)
                                { return 0; }
                                else
                                {
                                    switch (VarType)
                                    {
                                        case data.VarType.Bool:
                                            return (T.SM.Rows.Get_UnShow<double>(SMID, C.SM.Amount) > 0 ? 1 : 0);
                                        case data.VarType.dbl:
                                            return T.SM.Rows.Get_UnShow<double>(SMID, C.SM.Amount);
                                        case data.VarType.i32:
                                            return T.SM.Rows.Get_UnShow<int>(SMID, C.SM.Amount);
                                        default: throw new Exception("Неизветсный тип");
                                    }
                                }
                            }
                        }
                        public string Mark { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name); } }
                        public string EdType { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.Name); } }
                        public string MeanZero { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.MZero); } }
                        public string Method { get { return Norm.MethodName; } }
                        public data.VarType VarType { get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.VarType); } }
                        public bool Exp { get { return T.Mark.Rows.Get<bool>(MarkID, C.Mark.Exp); } }
                        //Зависимость ниже надо убрать, пока не знаю как!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        public RCache.Marks_class.Mark_class.INorm Norm { get { return RCache.Marks[MarkID].GetNorm(T.SPoint.Rows.Get_UnShow<uint>(this.Parent.SPointID, C.SPoint.Object, C.Object.Norm)); } }

                        public bool Alow { get { return Parent.Parent.AM[Parent.Parent.Parent.Mark.Rows.GetIndex(MarkID)]; } }

                        public bool LocalAlow;

                        public override string ToString()
                        { return Mark + ", " + Method + ", Amount=" + Amount.ToString(); }
                    }

                    public struct TCS_struct
                    {
                        public TCS_struct(uint ID, uint TCID)
                        {
                            this.ID = ID;
                            this.TCID = T.TCS.Rows.Get_UnShow<uint>(ID, C.TCS.TestCond);
                        }
                        public TCS_struct(uint TCID)
                        {
                            this.ID = 0;
                            this.TCID = TCID;
                        }

                        public readonly uint ID;
                        public readonly uint TCID;

                        public string Name { get { return T.TestCond.Rows.Get<string>(TCID, C.TestCond.Name); } }
                        public string EdType { get { return T.TestCond.Rows.Get<string>(TCID, C.TestCond.EdType, C.EdType.Name); } }
                        public string Value { get { return T.TCS.Rows.Get<string>(ID, C.TCS.Value); } }

                        public override string ToString()
                        { return Name + ' ' + EdType + '=' + Value; }
                    }

                    public AValue GetValue(int Index) { return Values[Index]; }

                    public uint PrtSID { get; internal set; }
                    public int CYMD { get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.CYMD); } }
                    public int AYMD { get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.AYMD); } }
                    public uint SampleID { get; internal set; }
                    public uint SPointID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint); } }
                    public uint SGroupID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.SGroup); } }
                    public uint ObjectID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object); } }
                    public uint ObjectLoationID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo); } }
                    public uint OTypeID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OType); } }
                    public uint AreaID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Area); } }
                    public uint OLocationFromID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom); } }
                    public uint PTypeID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.PType); } }
                    public uint SCauseID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SCause); } }
                    public bool Volumed { get { return T.Sample.Rows.Get<bool>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed); } }
                    public int SPSGNum { get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.SPoint, C.SPoint.SGNum); } }
                    public int Number
                    {
                        get { return T.Sample.Rows.Get_UnShow<int>(SampleID, C.Sample.Number); }
                    }
                    public string SPointName { get { return T.Sample.Rows.Get_UnShow<string>(SampleID, C.Sample.SPoint, C.SPoint.Name); } }
                    public uint NormID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.Norm); } }
                    public uint PeopleID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.Resp, C.Resp.PodrPpl, C.PodrPpl.People); } }
                    public uint PodrID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Podr); } }
                    public uint OLocationToID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationTo); } }

                    public Protokol_class Parent { get; internal set; }
                    public Protokols_class pParent { get; internal set; }

                    public DataBase.State Status { get; internal set; }

                    public int ValueCount { get { return Values.Length; } }

                    public readonly AValue[] Values;

                    TCS_struct[] TCS = null;

                    Mark_struct[] Marks;
                    public int MarkCount { get; internal set; }

                    public int TCSCount
                    {
                        get
                        {
                            if (TCS == null)
                            { GetTestCriterons(); }

                            return TCS.Length;
                        }
                    }

                    public string TCSName(int Index)
                    {
                        if (TCS == null)
                        { GetTestCriterons(); }

                        return TCS[Index].Name;
                    }

                    public string TCSEdType(int Index)
                    {
                        if (TCS == null)
                        { GetTestCriterons(); }

                        return TCS[Index].EdType;
                    }

                    public string TCSValue(int Index)
                    {
                        if (TCS == null)
                        { GetTestCriterons(); }

                        return TCS[Index].Value;
                    }

                    public Mark_struct this[int Index] { get { return Marks[Index]; } }
                    public void SetLocalAlow(int MarkIndex)
                    { Marks[MarkIndex].LocalAlow = true; }
                    public int Count { get { return Marks.Length; } }

                    public void AddMark(uint SMID)
                    {
                        int MIndex = pParent.Mark.Rows.GetIndex(T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark));

                        if (MIndex > -1)
                        {
                            Marks[MIndex] = new Mark_struct(this, SMID, T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark));

                            if (this.Marks[MIndex].Amount > 0)
                            { MarkCount++; }
                        }
                    }
                    public void UpdateMCount()
                    {
                        if (Parent != null)
                        {
                            for (int i = 0; i < Marks.Length; i++)
                            {
                                if (Parent.AM[i])
                                {
                                    switch (Marks[i].VarType)
                                    {
                                        case data.VarType.Bool:
                                            MarkCount++;
                                            Parent.MarkCount++;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    public void SaveChanges(uint PrtID)
                    {
                        if (PrtSID == 0)
                        {
                            G.PrtS.QUERRY().ADD
                                .C(C.PrtS.Prt, PrtID)
                                .C(C.PrtS.Sample, SampleID)
                                .DO();
                            this.PrtSID = G.PrtS.Rows.GetID(G.PrtS.Rows.Count - 1);
                        }
                    }

                    void GetTestCriterons()
                    {
                        var TestCond = T.TestCond.CreateSubTable(false);
                        TestCond.QUERRY().SHOW.WHERE.C(C.TestCond.SPoint, SPointID).DO();

                        this.TCS = new TCS_struct[TestCond.Rows.Count];

                        for (int i = 0; i < this.TCS.Length; i++)
                        { this.TCS[i] = new TCS_struct(TestCond.Rows.GetID(i)); }

                        var TCS = T.TCS.CreateSubTable(false);
                        TCS.QUERRY().SHOW.WHERE.C(C.TCS.Sample, this.SampleID).DO();

                        for (int i = 0; i < TCS.Rows.Count; i++)
                        {
                            var TCIndex = TestCond.Rows.GetIndex(TCS.Rows.Get_UnShow<uint>(i, C.TCS.TestCond));

                            if (TCIndex > -1)
                            { this.TCS[TCIndex] = new TCS_struct(TCS.Rows.GetID(i), 0); }
                        }
                    }

                    public override string ToString()
                    {
                        return "ID=" + this.SampleID.ToString() + ", Number=" + this.Number.ToString() + ", SC=" + MarkCount.ToString();
                    }
                }
                /// <summary>Тип Протокола</summary>
                public readonly SGroup_class Criterions;

                public readonly Protokols_class Parent;
                public uint PrtID { get; internal set; }

                int _Number;
                /// <summary>Номер протокола</summary>
                public int Number
                {
                    get { return _Number; }
                    set
                    {
                        if (value > 0 && _Number != value)
                        {
                            for (int i = 0; i < Parent.Protokols.Count; i++)
                            {
                                if (Parent.Protokols[i].Number == value)
                                {
                                    return;
                                }
                            }

                            this._Number = value;

                            if (PrtID > 0)
                            { T.Prt.Rows.Set(PrtID, C.Prt.Number, this._Number); }

                            Parent.Protokols.Sort((it1, it2) => it1._Number.CompareTo(it2.Number));
                        }
                    }
                }
                public int Index { get; internal set; }
                public data.SGroup SGroup { get { return (data.SGroup)Criterions.SGroupID; } }

                struct Mark_struct
                {
                    public Mark_struct(uint MarkID, uint SPointID)
                    {
                        this.MarkID = MarkID;
                        this.SPointID = SPointID;
                        this._AmCount = 0;
                        this._Summ = 0;
                        this._Method = "";
                        this.MethodOk = true;
                    }

                    public readonly uint MarkID;
                    public readonly uint SPointID;
                    public uint NormID { get { return T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object, C.Object.Norm); } }
                    int _AmCount;
                    double _Summ;

                    public int AmCount { get { return _AmCount; } }

                    public data.VarType VarType { get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.VarType); } }

                    public double Middle { get { return (_AmCount > 0 ? _Summ / _AmCount : 0); } }

                    public void AddAmount(double Amount)
                    {
                        _Summ += Amount;
                        _AmCount++;
                    }

                    public void SetCount(int Count)
                    {
                        if (_AmCount > Count)
                        { throw new Exception(""); }
                        else
                        { _AmCount = Count; }
                    }

                    string _Method;
                    public string Method { get { return _Method; } }
                    public string MeanZero { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.MZero); } }
                    public string EdType { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.Name); } }
                    public string Name { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name); } }
                    public int Round { get { return T.Mark.Rows.Get<int>(MarkID, C.Mark.Round); } }
                    public bool Exp { get { return T.Mark.Rows.Get<bool>(MarkID, C.Mark.Exp); } }
                    public bool MethodOk;

                    public void SetMethod(string Method)
                    {
                        if (Method.Length > 0)
                        {
                            if (_Method.Length > 0 && _Method != Method)
                            {
                                _Method = "[разные методы]";
                                MethodOk = false;
                            }
                            else
                            { _Method = Method; }
                        }
                    }

                    public override string ToString()
                    {
                        return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name) + ", " + Method + ", " + Middle.ToString();
                    }
                }

                Mark_struct[,] Marks;
                string[] TablesNames;
                string[] MarkMethods;

                public int TableCount { get { return TablesNames.Length; } }
                /// <summary>Получить концентрацию, с учетом округления и типа вывода</summary>
                /// <param name="MarkIndex">Номер показателя</param>
                /// <param name="TableIndex">Номер таблицы</param>
                /// <returns></returns>
                public double GetMarkAmount(int MarkIndex, int TableIndex = 0)
                {
                    var Amount = Marks[MarkIndex, TableIndex].Middle;

                    switch (Marks[MarkIndex, TableIndex].VarType)
                    {
                        case data.VarType.Bool:
                            if (Amount == 0 || Amount == 1)
                            { return Amount; }
                            else if (Marks[MarkIndex, TableIndex].Round > 0)
                            { return Math.Round(Amount, Marks[MarkIndex, TableIndex].Round); }
                            else
                            { return Amount; }
                        case data.VarType.dbl:
                            if (Marks[MarkIndex, TableIndex].Round > 0)
                            { return Math.Round(Amount, Marks[MarkIndex, TableIndex].Round); }
                            break;
                        case data.VarType.i32:
                            if (Marks[MarkIndex, TableIndex].Round > 0)
                            { return Math.Round(Amount, Marks[MarkIndex, TableIndex].Round); }
                            else
                            { return Amount; }
                    }
                    return Marks[MarkIndex, TableIndex].Middle;
                }

                public data.VarType GetMakrVarType(int MarkIndex)
                { return Marks[MarkIndex, 0].VarType; }
                public string GetMarkEdType(int MarkIndex)
                { return Marks[MarkIndex, 0].EdType; }
                public string GetMethod(int MarkIndex)
                { return MarkMethods[MarkIndex]; }
                public bool GetMakrExp(int MarkIndex)
                { return Marks[MarkIndex, 0].Exp; }
                public string GetTableName(int TableIndex = 0)
                { return TablesNames[TableIndex]; }
                public string GetMarkName(int MarkIndex)
                { return Marks[MarkIndex, 0].Name; }
                /// <summary>Нужен спец вывод?</summary>
                public bool IsSpetialOut(int MarkIndex, int TableIndex = 0, bool OverLimit = true)
                {
                    var Amount = Marks[MarkIndex, TableIndex].Middle;

                    switch (Marks[MarkIndex, TableIndex].VarType)
                    {
                        case data.VarType.Bool:
                            if (Amount == 0 || Amount == 1)
                            { return true; }
                            break;
                        case data.VarType.dbl:
                        case data.VarType.i32:
                            {
                                if (Amount == 0)
                                { return true; }
                                else
                                {
                                    var Norm = RCache.Marks[Marks[MarkIndex, TableIndex].MarkID].GetNorm(Marks[MarkIndex, TableIndex].NormID);

                                    if (Norm.Ranges.Count > 0)
                                    {
                                        double From = Norm.Ranges[0].From, To = Norm.Ranges[Norm.Ranges.Count - 1].To;

                                        if (OverLimit && From + To > 0 && (Amount < From || Amount > To))
                                        { return true; }
                                    }

                                    if (Marks[MarkIndex, TableIndex].Exp && (Amount > 1000 || Amount < 0.0001))
                                    { return true; }
                                }
                                break;
                            }
                    }

                    return false;
                }
                public uint GetMarkID(int MarkIndex)
                { return Marks[MarkIndex, 0].MarkID; }
                public uint GetNormID(int MarkIndex, int TableIndex = 0)
                { return Marks[MarkIndex, TableIndex].NormID; }
                public string GetMarkMeanZero(int MarkIndex)
                { return Marks[MarkIndex, 0].MeanZero; }

                public string GetSpetialOut(int MarkIndex, int TableIndex = 0)
                {
                    var Amount = Marks[MarkIndex, TableIndex].Middle;

                    switch (Marks[MarkIndex, TableIndex].VarType)
                    {
                        case data.VarType.Bool:
                            if (Amount == 0)
                            { return "Нет"; }
                            else if (Amount == 1)
                            { return "Да"; }
                            break;
                        case data.VarType.dbl:
                            if (Amount == 0)
                            { return Marks[MarkIndex, TableIndex].MeanZero; }
                            else
                            {
                                var Norm = RCache.Marks[Marks[MarkIndex, TableIndex].MarkID].GetNorm(Marks[MarkIndex, TableIndex].NormID);

                                if (Norm.Ranges.Count > 0)
                                {
                                    double From = Norm.Ranges[0].From, To = Norm.Ranges[Norm.Ranges.Count - 1].To;

                                    /*switch (Norm.NType)
                                    {
                                        case data.NType.Mark:
                                            From = Norm.Range.From;
                                            To = Norm.Range.To;
                                            break;
                                        case data.NType.PodrAll:
                                        case data.NType.PodrK:
                                        case data.NType.PodrV:
                                            var PIndex = RCache.Marks.Norms.GetPodrIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Marks[MarkIndex, TableIndex].SPointID, C.SPoint.Podr));
                                            if (PIndex > -1)
                                            {
                                                From = Norm.Station(PIndex).From;
                                                To = Norm.Station(PIndex).To;
                                            }
                                            break;
                                        case data.NType.Volume:
                                            var VIndex = RCache.Marks.Norms.GetVolumeIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Marks[MarkIndex, TableIndex].SPointID, C.SPoint.Object, C.Object.OLocationFrom));
                                            if (VIndex > -1)
                                            {
                                                From = Norm.Volume(VIndex).From;
                                                To = Norm.Volume(VIndex).To;
                                            }
                                            break;
                                        default:
                                            throw new Exception("Неизвестный тип нормы");
                                    }*/

                                    if (From + To > 0)
                                    {
                                        if (Amount > To)
                                        { return '>' + To.ToString(); }
                                        else if (Amount < From)
                                        { return '<' + From.ToString(); }
                                    }
                                }

                                if (Marks[MarkIndex, TableIndex].Exp)
                                {
                                    if (Amount > 0)
                                    {
                                        if (Amount > 1000)
                                        {
                                            var tmp = Math.Round(Amount).ToString();
                                            int count = -1;
                                            for (count = 0; count < tmp.Length; count++)
                                            {
                                                if (tmp[count] == '0')
                                                { break; }
                                            }

                                            return tmp.Substring(0, count) + "E+" + (tmp.Length - count).ToString();
                                        }
                                        else if (Amount < 1)
                                        {
                                            var tmp = Amount.ToString("0.###################").Substring(2);
                                            int count = -1;
                                            for (int i = tmp.Length - 1; i > -1; i--)
                                            {
                                                if (tmp[i] != '0')
                                                { count = tmp.Length - i; }
                                            }

                                            return tmp.Substring(tmp.Length - count, count) + "E-" + (tmp.Length).ToString();
                                        }
                                    }
                                }

                                return Amount.ToString();
                            }
                        case data.VarType.i32:
                            if (Amount == 0)
                            { return Marks[MarkIndex, TableIndex].MeanZero; }
                            else
                            {
                                var Norm = RCache.Marks[Marks[MarkIndex, TableIndex].MarkID].GetNorm(Marks[MarkIndex, TableIndex].NormID);

                                if (Norm.Ranges.Count > 0)
                                {
                                    double From = Norm.Ranges[0].From, To = Norm.Ranges[Norm.Ranges.Count - 1].To;

                                    /*switch (Norm.NType)
                                    {
                                        case data.NType.Mark:
                                            From = Norm.Range.From;
                                            To = Norm.Range.To;
                                            break;
                                        case data.NType.PodrAll:
                                        case data.NType.PodrK:
                                        case data.NType.PodrV:
                                            var PIndex = RCache.Marks.Norms.GetPodrIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Marks[MarkIndex, TableIndex].SPointID, C.SPoint.Podr));
                                            if (PIndex > -1)
                                            {
                                                From = Norm.Station(PIndex).From;
                                                To = Norm.Station(PIndex).To;
                                            }
                                            break;
                                        case data.NType.Volume:
                                            var VIndex = RCache.Marks.Norms.GetVolumeIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Marks[MarkIndex, TableIndex].SPointID, C.SPoint.Object, C.Object.OLocationFrom));
                                            if (VIndex > -1)
                                            {
                                                From = Norm.Volume(VIndex).From;
                                                To = Norm.Volume(VIndex).To;
                                            }
                                            break;
                                        default:
                                            throw new Exception("Неизвестный тип нормы");
                                    }*/

                                    if (From + To > 0)
                                    {
                                        if (Amount > To)
                                        { return '>' + To.ToString(); }
                                        else if (Amount < From)
                                        { return '<' + From.ToString(); }
                                    }
                                }

                                if (Marks[MarkIndex, TableIndex].Exp)
                                {
                                    if (Amount > 1000)
                                    { return Amount.ToString("E+1000"); }
                                }

                                break;
                            }
                    }

                    return Amount.ToString();
                }

                public uint OLocationID { get; internal set; }
                public uint PodrID { get; internal set; }
                public uint PaPoSID { get; internal set; }
                public uint AreaID { get; internal set; }
                public uint ObjectID { get; internal set; }
                public int Loc { get; internal set; }
                public int YM
                {
                    get
                    {
                        if (PrtID == 0)
                        {
                            var YMDMax = 0;

                            for (int i = 0; i < Samples.Count; i++)
                            {
                                if (Samples[i].AYMD > YMDMax)
                                { YMDMax = Samples[i].AYMD; }
                            }

                            return ATMisc.GetYMFromYMD(YMDMax);
                        }
                        else
                        { return T.Prt.Rows.Get<int>(PrtID, C.Prt.YM); }
                    }
                    set
                    {
                        if (PrtID == 0)
                        {

                        }
                        else
                        { T.Prt.Rows.Set<int>(PrtID, C.Prt.YM, value); }
                    }
                }

                List<Sample_class> Samples = new List<Sample_class>();

                int _Act;
                public int Act
                {
                    get { return _Act; }
                    set
                    {
                        _Act = value;

                        if (PrtID > 0)
                        { T.Prt.Rows.Set(PrtID, C.Prt.Taos, value); }
                    }
                }

                int _Time = 8 * 60;

                public int Time
                {
                    get { return _Time; }
                    set
                    {
                        _Time = value;

                        if (PrtID > 0)
                        { T.Prt.Rows.Set(PrtID, C.Prt.Time, value); }
                    }
                }
                public string StrTime
                {
                    get { return new DataBase.Time(_Time).ToString(); }
                    set { this.Time = new DataBase.Time(value).AllMinutes; }
                }

                public Sample_class this[int Index] { get { return Samples[Index]; } }
                public int SampleCount { get { return Samples.Count; } }

                AValue[] Values;

                public int MarkCount { get; internal set; }
                public int TotalMarkCount { get { return AM.Length; } }

                bool[] AM;

                public bool GetTotalAlowedMark(int Index)
                { return AM[Index]; }
                public void SetTotalAlowMark(int Index, bool Alow)
                { AM[Index] = Alow; }

                public uint GetTotalMarkID(int Index)
                { return Parent.Mark.Rows.GetID(Index); }
                public string GetTotalMarkName(int Index)
                { return Parent.Mark.Rows.Get<string>(Index, C.Mark.Name); }
                public string GetTotalMarkEdType(int Index)
                { return Parent.Mark.Rows.Get<string>(Index, C.Mark.EdType, C.EdType.Name); }

                public void SaveChanges()
                {
                    if (this.Index > 0)
                    { Parent.Protokols[this.Index - 1].SaveChanges(); }

                    if (PrtID == 0)
                    {
                        var Q = new DataBase.FastAdd_class(1, G.Prt);

                        Q.C(C.Prt.YM, Parent.YM);
                        Q.C(C.Prt.Number, Number);
                        Q.C(C.Prt.OLocation, OLocationID);
                        Q.C(C.Prt.Union, Parent.Union);
                        Q.C(C.Prt.Podr, PodrID);
                        Q.C(C.Prt.Area, AreaID);
                        Q.C(C.Prt.Object, ObjectID);
                        Q.C(C.Prt.SGroup, Criterions.SGroupID);
                        Q.C(C.Prt.PaPoS, PaPoSID);
                        Q.C(C.Prt.Day, (byte)Day);
                        Q.C(C.Prt.Time, _Time);

                        if (Q.NEXTROW() || Q.DO())
                        {
                            PrtID = G.Prt.Rows.GetID(G.Prt.Rows.Count - 1);

                            for (int i = 0; i < Samples.Count; i++)
                            { Samples[i].SaveChanges(PrtID); }
                        }
                    }
                    else
                    {
                        var Q = new DataBase.FastSet_class(G.Prt, PrtID);

                        Q.C(C.Prt.YM, Parent.YM);
                        Q.C(C.Prt.Number, Number);
                        Q.C(C.Prt.Union, Parent.Union);
                        Q.C(C.Prt.OLocation, OLocationID);
                        Q.C(C.Prt.Podr, PodrID);
                        Q.C(C.Prt.Area, AreaID);
                        Q.C(C.Prt.Object, ObjectID);
                        Q.C(C.Prt.SGroup, Criterions.SGroupID);
                        Q.C(C.Prt.PaPoS, PaPoSID);
                        Q.C(C.Prt.Day, (byte)Day);
                        Q.C(C.Prt.Time, _Time);

                        Q.DO();
                    }
                }

                public bool Delete(bool ExchangeNumbers)
                {
                    if (PrtID > 0)
                    {
                        G.Prt.QUERRY().DELETE.WHERE.ID(PrtID).DO(); //использую запрос, т.к. SetValue не срабатывает в .Rows.Status()

                        if (Parent.Protokols.Count - 1 > Index && Parent.Protokols[Index].PrtID != 0 && Index < Parent.Protokols.Count)
                        {
                            Parent.Protokols.RemoveAt(Index);

                            if (ExchangeNumbers)
                            {
                                int LastNumber = 1;

                                for (int i = Index; i < Parent.Protokols.Count - 1; i++)
                                {
                                    if (Parent.Protokols[i].PrtID > 0)
                                    { LastNumber = Parent.Protokols[i]._Number = Parent.Protokols[i + 1]._Number; }

                                    Parent.Protokols[i].Index = i;
                                }

                                Parent.Protokols[Parent.Protokols.Count - 1]._Number = LastNumber;
                                Parent.Protokols[Parent.Protokols.Count - 1].Index = Parent.Protokols.Count - 1;
                            }
                            else
                            {
                                for (int i = Index; i < Parent.Protokols.Count; i++)
                                { Parent.Protokols[i].Index = i; }
                            }
                        }

                        this.PrtID = 0;

                        return true;
                    }
                    else
                    { return false; }
                }

                public bool Move(sbyte Direction)
                {
                    if (this.PrtID == 0 ||
                        Index + Direction < 0 ||
                        Index + Direction >= Parent.Protokols.Count ||
                        Parent.Protokols[this.Index + Direction].PrtID == 0)
                    { return false; }
                    else
                    {
                        var Proto1 = this;
                        var Proto2 = Parent.Protokols[this.Index + Direction];

                        {
                            var SaveNumber1 = Proto1.Number;
                            var SaveNumber2 = Proto2.Number;

                            //Proto1.Number = int.MaxValue;   //т.к. ведется проверка на уникальность номера

                            Proto2._Number = SaveNumber1;
                            Proto1._Number = SaveNumber2;
                        }

                        Parent.Protokols.RemoveAt(Index + Direction);
                        Parent.Protokols.Insert(Index, Proto2);

                        Proto1.Index = this.Index + Direction;
                        Proto2.Index = this.Index - Direction;

                        if (Proto1.Index > Proto2.Index)
                        { Proto1.SaveChanges(); }
                        else
                        { Proto2.SaveChanges(); }

                        return true;
                    }
                }

                public bool AddSample(Sample_class Sample)
                {
                    if (Criterions.SGroupID != Sample.SGroupID || Criterions.Count == 0)
                    { return false; }
                    else
                    {
                        for (int i = 0; i < Criterions.Count; i++)
                        {
                            if (!Values[Criterions[i].Prt_Index].Check(Sample.Values[Criterions[i].Prt_Index]))
                            { return false; }
                        }

                        this.Samples.Add(Sample);
                        Sample.Parent = this;

                        return true;
                    }
                }

                struct SPMarks_struct
                {
                    public SPMarks_struct(uint SPID, int MarkCount)
                    {
                        this.SPID = SPID;
                        this.Marks = new Mark_struct[MarkCount];
                    }

                    public readonly uint SPID;
                    public Mark_struct[] Marks;

                    public override string ToString()
                    {
                        return T.SPoint.Rows.Get<string>(SPID, C.SPoint.Name);
                    }
                }

                /// <summary>Дата создания замеров</summary>
                public string DateOstr
                {
                    get
                    {
                        if (Samples.Count > 0)
                        {
                            if (Samples.Count == 1)
                            { return ATMisc.GetDateTime(Samples[0].CYMD).ToShortDateString(); }
                            else
                            {
                                var DTMin = Samples[0].CYMD;
                                var DTMax = Samples[Samples.Count - 1].CYMD;

                                for (int i = Samples.Count - 1; i > 0; i--)
                                {
                                    if (DTMax < Samples[i].CYMD)
                                    { DTMax = Samples[i].CYMD; }
                                    if (DTMin > Samples[i].CYMD)
                                    { DTMin = Samples[i].CYMD; }
                                }

                                if (DTMax == DTMin)
                                { return ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                                else
                                { return ATMisc.GetDateTime(DTMin).ToShortDateString() + "-" + ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                            }
                        }
                        else
                        { return "SC=0"; }
                    }
                }

                /// <summary>Дата создания протокола</summary>
                public DateTime Date
                { get { return ATMisc.GetDateTimeFromYM(this.YM).AddDays(Day - 1); } }

                public int Day
                {
                    get
                    {
                        if (this.PrtID == 0)
                        {
                            var YMNow = ATMisc.GetYMFromDateTime(DateTime.Now);

                            if (this.YM < YMNow)
                            { return ATMisc.GetDaysInMonth(this.YM); }
                            else if (this.YM > YMNow)
                            { return 1; }
                            else
                            {
                                switch (DateTime.Now.DayOfWeek)
                                {
                                    case DayOfWeek.Sunday:
                                        return DateTime.Now.Day - 2;
                                    case DayOfWeek.Saturday:
                                        return DateTime.Now.Day - 1;
                                }
                            }

                            return DateTime.Now.Day;
                        }
                        else if (T.Prt.Rows.Get<byte>(PrtID, C.Prt.Day) == 0)
                        {
                            var DT = ATMisc.GetDateTimeFromYM(this.YM + 1);
                            DT = DT.AddDays(-1);

                            switch (DT.DayOfWeek)
                            {
                                case DayOfWeek.Sunday:
                                    return DT.Day - 2;
                                case DayOfWeek.Saturday:
                                    return DT.Day - 1;
                            }

                            return DT.Day;
                        }
                        else
                        { return T.Prt.Rows.Get<byte>(PrtID, C.Prt.Day); }
                    }
                    set
                    {
                        if (PrtID > 0 && value > 0 && value < ATMisc.GetDaysInMonth(this.YM) + 1)
                        { T.Prt.Rows.Set<byte>(PrtID, C.Prt.Day, (byte)value); }
                    }
                }
                /// <summary>Дата анализа</summary>
                public string DateP
                {
                    get
                    {
                        if (Samples.Count == 1)
                        { return ATMisc.GetDateTime(Samples[0].AYMD).ToShortDateString(); }
                        else
                        {
                            var DTMin = Samples[0].AYMD;
                            var DTMax = Samples[Samples.Count - 1].AYMD;

                            for (int i = Samples.Count - 1; i > 0; i--)
                            {
                                if (DTMax < Samples[i].AYMD)
                                { DTMax = Samples[i].AYMD; }
                                if (DTMin > Samples[i].AYMD)
                                { DTMin = Samples[i].AYMD; }
                            }

                            if (DTMax == DTMin)
                            { return ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                            else
                            { return ATMisc.GetDateTime(DTMin).ToShortDateString() + "-" + ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                        }
                    }
                }
                public string Objects
                {
                    get
                    {
                        var OIDs = new List<uint>();
                        var Objects = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (OIDs.IndexOf(Samples[i].ObjectID) < 0)
                            {
                                OIDs.Add(Samples[i].ObjectID);

                                Objects += ", " + T.Object.Rows.Get<string>(Samples[i].ObjectID, C.Object.Name).ToLower();
                            }
                        }

                        return Objects.Substring(2);
                    }
                }
                public string SCause
                {
                    get
                    {
                        var ScIDs = new List<uint>();
                        var SCauses = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (ScIDs.IndexOf(Samples[i].SCauseID) < 0)
                            {
                                ScIDs.Add(Samples[i].SCauseID);

                                SCauses += ", " + T.SCause.Rows.Get<string>(Samples[i].SCauseID, C.SCause.Name).ToLower();
                            }
                        }

                        return SCauses.Substring(2);
                    }
                }
                public string ObjectsLocations
                {
                    get
                    {
                        if (this.SGroup == data.SGroup.Middle1)
                        {
                            var OLIDs = new List<uint>();
                            var ObjectLocations = "";

                            for (int i = 0; i < Samples.Count; i++)
                            {
                                if (OLIDs.IndexOf(Samples[i].AreaID) < 0)
                                {
                                    OLIDs.Add(Samples[i].AreaID);

                                    ObjectLocations += ", " + T.Area.Rows.Get<string>(Samples[i].AreaID, C.Area.Name).ToLower();
                                }
                            }

                            return ObjectLocations.Substring(2);
                        }
                        else
                        {
                            var OLIDs = new List<uint>();
                            var ObjectLocations = "";

                            for (int i = 0; i < Samples.Count; i++)
                            {
                                if (OLIDs.IndexOf(Samples[i].ObjectLoationID) < 0)
                                {
                                    OLIDs.Add(Samples[i].ObjectLoationID);

                                    ObjectLocations += ", " + T.OLocation.Rows.Get<string>(Samples[i].ObjectLoationID, C.OLocation.Name).ToLower();
                                }
                            }

                            return ObjectLocations.Substring(2);
                        }
                    }
                }
                public string OType
                {
                    get
                    {
                        var OTIDs = new List<uint>();
                        var OTypes = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (OTIDs.IndexOf(Samples[i].OTypeID) < 0)
                            {
                                OTIDs.Add(Samples[i].OTypeID);

                                OTypes += ", " + T.OType.Rows.Get<string>(Samples[i].OTypeID, C.OType.Name).ToLower();
                            }
                        }

                        return OTypes.Substring(2);
                    }
                }
                public string Methods
                {
                    get
                    {
                        var MethodsList = new List<string>();
                        var Methods = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            for (int j = 0; j < Samples[i].MarkCount; j++)
                            {
                                if (MethodsList.IndexOf(Samples[i][j].Method) < 0)
                                {
                                    MethodsList.Add(Samples[i][j].Method);

                                    Methods += ", " + Samples[i][j].Method;
                                }
                            }
                        }

                        return Methods.Substring(2);
                    }
                }
                public string Peoples
                {
                    get
                    {
                        var PeoplesList = new List<uint>();
                        var Peoples = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (PeoplesList.IndexOf(Samples[i].PeopleID) < 0)
                            {
                                PeoplesList.Add(Samples[i].PeopleID);

                                Peoples += ", " + Misc.GetShortFIO(Samples[i].PeopleID);
                            }
                        }

                        return Peoples.Substring(2);
                    }
                }
                public string Causes
                {
                    get
                    {
                        var CausesList = new List<uint>();
                        var Causes = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (CausesList.IndexOf(Samples[i].SCauseID) < 0)
                            {
                                CausesList.Add(Samples[i].SCauseID);

                                Causes += ", " + T.SCause.Rows.Get<string>(Samples[i].SCauseID, C.SCause.Name);
                            }
                        }

                        return Causes.Substring(2);
                    }
                }
                public string PTypes
                {
                    get
                    {
                        var PTypesList = new List<uint>();
                        var PTypes = "";

                        for (int i = 0; i < Samples.Count; i++)
                        {
                            if (PTypesList.IndexOf(Samples[i].PTypeID) < 0)
                            {
                                PTypesList.Add(Samples[i].PTypeID);

                                PTypes += ", " + T.PType.Rows.Get<string>(Samples[i].PTypeID, C.PType.Name);
                            }
                        }

                        return PTypes.Substring(2);
                    }
                }

                public string Numbers
                {
                    get
                    {
                        if (Samples.Count > 0)
                        {
                            var numbers = Samples[0].Number.ToString();

                            for (int i = 1; i < Samples.Count; i++)
                            { numbers += ", " + Samples[i].Number.ToString(); }

                            return numbers;
                        }
                        else
                        { return "SC=0"; }
                    }
                }

                public string ProtoType
                {
                    get
                    {
                        if (Samples.Count == 0)
                        { return "Ошибка CS=0"; }
                        else
                        {
                            switch (SGroup)
                            {
                                case data.SGroup.Middle4:
                                case data.SGroup.Middle1:
                                    return T.SGroup.Rows.Get<string>((uint)SGroup, C.SGroup.Name);
                                case data.SGroup.Group1:
                                case data.SGroup.Group2:
                                case data.SGroup.Group3:
                                case data.SGroup.NotGroup1:
                                case data.SGroup.AquaAurat:
                                case data.SGroup.KOCA:
                                case data.SGroup.Toxicity1:
                                case data.SGroup.Toxicity2:
                                    return T.SGroup.Rows.Get<string>((uint)SGroup, C.SGroup.Name) + ':' + OType;
                                default: throw new Exception("Неизвестный тип протокола");
                            }
                        }
                    }
                }

                public void GetTables()
                {
                    Samples.Sort((it1, it2) =>
                    {
                        var ret = it1.SPSGNum.CompareTo(it2.SPSGNum);

                        if (ret == 0)
                        { ret = it1.Number.CompareTo(it2.Number); }

                        return ret;
                    });

                    switch (SGroup)
                    {
                        case data.SGroup.Group1:
                            {
                                int count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    {
                                        if (Parent.Mark.Rows.Get<bool>(i, C.Mark.ShowZr))
                                        { count++; }
                                        else if ((data.VarType)Parent.Mark.Rows.Get_UnShow<uint>(i, C.Mark.VarType) != data.VarType.Bool)
                                        {
                                            var Used = false;
                                            for (int j = 0; j < Samples.Count; j++)
                                            {
                                                if (Samples[j][i].Amount > 0)
                                                {
                                                    Used = true;
                                                    break;
                                                }
                                            }

                                            AM[i] = Used;

                                            if (Used)
                                            { count++; }
                                        }
                                        else
                                        { count++; }
                                    }
                                }

                                MarkCount = count;

                                this.Marks = new Mark_struct[MarkCount, Samples.Count];
                                this.TablesNames = new string[Samples.Count];
                                this.MarkMethods = new string[MarkCount];

                                count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    {
                                        var MID = Parent.Mark.Rows.GetID(i);

                                        for (int j = 0; j < Samples.Count; j++)
                                        {
                                            this.Marks[count, j] = new Mark_struct(MID, Samples[j].SPointID);

                                            switch (this.Marks[count, j].VarType)
                                            {
                                                case data.VarType.Bool:
                                                    this.Marks[count, j].AddAmount(Samples[j][i].Amount);
                                                    break;
                                                case data.VarType.dbl:
                                                case data.VarType.i32:
                                                    if (Samples[j][i].Amount > 0)
                                                    { this.Marks[count, j].AddAmount(Samples[j][i].Amount); }
                                                    break;
                                            }

                                            if (MarkMethods[count] == null || MarkMethods[count].Length == 0)
                                            {
                                                if (Samples[j][i].LocalAlow)
                                                { MarkMethods[count] = Samples[j][i].Method; }
                                                else
                                                { MarkMethods[count] = ""; }
                                            }

                                            this.Marks[count, j].SetMethod(Samples[j][i].Method);
                                        }

                                        count++;
                                    }
                                }

                                for (int i = 0; i < Samples.Count; i++)
                                { this.TablesNames[i] = T.SPoint.Rows.Get<string>(Samples[i].SPointID, C.SPoint.Object, C.Object.OType, C.OType.Name); }

                                MarkCount = count;
                            }
                            break;
                        case data.SGroup.Group2:
                        case data.SGroup.Group3:
                            {
                                int count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    {
                                        if (Parent.Mark.Rows.Get<bool>(i, C.Mark.ShowZr))
                                        { count++; }
                                        else if ((data.VarType)Parent.Mark.Rows.Get_UnShow<uint>(i, C.Mark.VarType) != data.VarType.Bool)
                                        {
                                            var Used = false;
                                            for (int j = 0; j < Samples.Count; j++)
                                            {
                                                if (Samples[j][i].Amount > 0)
                                                {
                                                    Used = true;
                                                    break;
                                                }
                                            }

                                            AM[i] = Used;

                                            if (Used)
                                            { count++; }
                                        }
                                        else
                                        { count++; }
                                    }
                                }

                                MarkCount = count;

                                this.Marks = new Mark_struct[MarkCount, Samples.Count];
                                this.TablesNames = new string[Samples.Count];
                                this.MarkMethods = new string[MarkCount];

                                count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    {
                                        var MID = Parent.Mark.Rows.GetID(i);

                                        for (int j = 0; j < Samples.Count; j++)
                                        {
                                            this.Marks[count, j] = new Mark_struct(MID, Samples[j].SPointID);

                                            switch (this.Marks[count, j].VarType)
                                            {
                                                case data.VarType.Bool:
                                                    this.Marks[count, j].AddAmount(Samples[j][i].Amount);
                                                    break;
                                                case data.VarType.dbl:
                                                case data.VarType.i32:
                                                    if (Samples[j][i].Amount > 0)
                                                    { this.Marks[count, j].AddAmount(Samples[j][i].Amount); }
                                                    break;
                                            }

                                            if (MarkMethods[count] == null || MarkMethods[count].Length == 0)
                                            {
                                                if (Samples[j][i].LocalAlow)
                                                { MarkMethods[count] = Samples[j][i].Method; }
                                                else
                                                { MarkMethods[count] = ""; }
                                            }

                                            this.Marks[count, j].SetMethod(Samples[j][i].Method);
                                        }

                                        count++;
                                    }
                                }

                                for (int i = 0; i < Samples.Count; i++)
                                { this.TablesNames[i] = T.SPoint.Rows.Get<string>(Samples[i].SPointID, C.SPoint.Name); }

                                MarkCount = count;
                            }
                            break;
                        case data.SGroup.Middle1:
                            {
                                int count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    { count++; }
                                }

                                MarkCount = count;

                                var SPIDs = new List<SPMarks_struct>();

                                var Marks = new Mark_struct[AM.Length, SPIDs.Count];

                                MarkMethods = new string[MarkCount];

                                for (int i = 0; i < Samples.Count; i++)
                                {
                                    var index = SPIDs.FindIndex((it) => it.SPID == Samples[i].SPointID);

                                    if (index < 0)
                                    {
                                        var sp = new SPMarks_struct(Samples[i].SPointID, AM.Length);

                                        SPIDs.Add(sp);


                                        for (int j = 0; j < AM.Length; j++)
                                        {
                                            if (AM[j])
                                            {
                                                sp.Marks[j] = new Mark_struct(Parent.Mark.Rows.GetID(j), Samples[i].SPointID);

                                                switch (sp.Marks[j].VarType)
                                                {
                                                    case data.VarType.Bool:
                                                        sp.Marks[j].AddAmount(Samples[i][j].Amount);
                                                        break;
                                                    case data.VarType.dbl:
                                                    case data.VarType.i32:
                                                        if (Samples[i][j].Amount > 0)
                                                        { sp.Marks[j].AddAmount(Samples[i][j].Amount); }
                                                        break;
                                                }

                                                sp.Marks[j].SetMethod(Samples[i][j].Method);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var sp = SPIDs[index];

                                        for (int j = 0; j < AM.Length; j++)
                                        {
                                            if (AM[j])
                                            {
                                                switch (sp.Marks[j].VarType)
                                                {
                                                    case data.VarType.Bool:
                                                        sp.Marks[j].AddAmount(Samples[i][j].Amount);
                                                        break;
                                                    case data.VarType.dbl:
                                                    case data.VarType.i32:
                                                        if (Samples[i][j].Amount > 0)
                                                        { sp.Marks[j].AddAmount(Samples[i][j].Amount); }
                                                        break;
                                                }

                                                sp.Marks[j].SetMethod(Samples[i][j].Method);
                                            }
                                        }
                                    }
                                }

                                this.Marks = new Mark_struct[count, SPIDs.Count];
                                this.TablesNames = new string[SPIDs.Count];

                                for (int i = 0; i < SPIDs.Count; i++)
                                {
                                    this.TablesNames[i] = T.SPoint.Rows.Get<string>(SPIDs[i].SPID, C.SPoint.Name);

                                    count = 0;
                                    for (int j = 0; j < AM.Length; j++)
                                    {
                                        if (AM[j])
                                        {
                                            this.Marks[count, i] = SPIDs[i].Marks[j];

                                            if (MarkMethods[count] == null || MarkMethods[count].Length == 0)
                                            {
                                                if (Samples[i][j].LocalAlow)
                                                { MarkMethods[count] = Samples[i][j].Method; }
                                                else
                                                { MarkMethods[count] = ""; }
                                            }

                                            count++;
                                        }
                                    }
                                }
                            }
                            break;
                        case data.SGroup.Middle4:
                            {
                                int count = 0;

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    { count++; }
                                }

                                MarkCount = count;

                                var TableMark = new List<Mark_struct[]>();
                                this.MarkMethods = new string[count];
                                var TablesNames = new List<string>();

                                for (int i = 0; i < this.Samples.Count; i++)
                                {
                                    for (int j = 0; j < TableMark.Count; j++)
                                    {
                                        if (T.SPoint.Rows.Get_UnShow<uint>(TableMark[j][0].SPointID, C.SPoint.Object, C.Object.OLocationTo) == Samples[i].OLocationToID)
                                        {
                                            for (int m = 0; m < AM.Length; m++)
                                            {
                                                if (AM[m])
                                                {
                                                    count = 0;

                                                    switch (Samples[i][m].VarType)
                                                    {
                                                        case data.VarType.Bool:
                                                            TableMark[j][count].AddAmount(Samples[i][m].Amount);
                                                            break;
                                                        case data.VarType.dbl:
                                                        case data.VarType.i32:
                                                            if (Samples[i][count].Amount > 0)
                                                            { TableMark[j][count].AddAmount(Samples[i][m].Amount); }
                                                            break;
                                                    }

                                                    if (MarkMethods[count] == null || MarkMethods[count].Length == 0)
                                                    {
                                                        if (Samples[i][m].LocalAlow)
                                                        { MarkMethods[count] = Samples[i][m].Method; }
                                                        else
                                                        { MarkMethods[count] = ""; }
                                                    }

                                                    count++;
                                                }
                                            }
                                            goto FindedIt;
                                        }
                                    }

                                    var smpl = new Mark_struct[MarkCount];

                                    TablesNames.Add(T.OLocation.Rows.Get<string>(Samples[i].OLocationToID, C.OLocation.Name));
                                    count = 0;

                                    for (int m = 0; m < AM.Length; m++)
                                    {
                                        if (AM[m])
                                        {
                                            smpl[count] = new Mark_struct(Samples[i][m].MarkID, Samples[i].SPointID);

                                            switch (Samples[i][m].VarType)
                                            {
                                                case data.VarType.Bool:
                                                    smpl[count].AddAmount(Samples[i][m].Amount);
                                                    break;
                                                case data.VarType.dbl:
                                                case data.VarType.i32:
                                                    if (Samples[i][m].Amount > 0)
                                                    { smpl[count].AddAmount(Samples[i][m].Amount); }
                                                    break;
                                            }

                                            if (MarkMethods[count] == null || MarkMethods[count].Length == 0)
                                            {
                                                if (Samples[i][m].LocalAlow)
                                                { MarkMethods[count] = Samples[i][m].Method; }
                                                else
                                                { MarkMethods[count] = ""; }
                                            }

                                            count++;
                                        }
                                    }

                                    TableMark.Add(smpl);

                                FindedIt: ;
                                }

                                this.Marks = new Mark_struct[this.MarkCount, TableMark.Count];
                                this.TablesNames = TablesNames.ToArray();

                                for (int i = 0; i < TableMark.Count; i++)
                                {
                                    for (int j = 0; j < TableMark[i].Length; j++)
                                    { this.Marks[j, i] = TableMark[i][j]; }
                                }
                            }
                            break;
                        case data.SGroup.NotGroup1:
                        case data.SGroup.AquaAurat:
                        case data.SGroup.KOCA:
                        case data.SGroup.Toxicity1:
                        case data.SGroup.Toxicity2:
                            if (Samples.Count > 0)
                            {
                                if (Samples.Count != 1)
                                { throw new Exception("Не верное количество замеров протокола"); }

                                var Marks = new List<Mark_struct>(AM.Length);
                                var MarkMethods = new List<string>();

                                for (int i = 0; i < AM.Length; i++)
                                {
                                    if (AM[i])
                                    {
                                        var m = new Mark_struct(Parent.Mark.Rows.GetID(i), Samples[0].SPointID);

                                        switch (m.VarType)
                                        {
                                            case data.VarType.Bool:
                                                m.AddAmount(Samples[0][i].Amount);
                                                break;
                                            case data.VarType.dbl:
                                            case data.VarType.i32:
                                                if (Samples[0][i].Amount > 0)
                                                { m.AddAmount(Samples[0][i].Amount); }
                                                else if (!Parent.Mark.Rows.Get<bool>(i, C.Mark.ShowZr))
                                                {
                                                    AM[i] = false;
                                                    goto NextMark;
                                                }

                                                break;
                                        }

                                        MarkMethods.Add(Samples[0][i].Method);
                                        m.SetMethod(Samples[0][i].Method);

                                        Marks.Add(m);
                                    NextMark: ;
                                    }
                                }

                                this.Marks = new Mark_struct[Marks.Count, 1];
                                this.TablesNames = new string[1];
                                this.TablesNames[0] = T.SPoint.Rows.Get<string>(Samples[0].SPointID, C.SPoint.Object, C.Object.OType, C.OType.Name);
                                this.MarkMethods = MarkMethods.ToArray();

                                for (int i = 0; i < Marks.Count; i++)
                                { this.Marks[i, 0] = Marks[i]; }

                                this.MarkCount = Marks.Count;
                            }
                            break;
                        default: throw new Exception("неизвестный тип протокола");
                    }
                }

                public string TemplateFileName()
                {
                    switch (this.SGroup)
                    {
                        case data.SGroup.Toxicity1:
                            return "Протокол испытаний токсичность1.xls";
                        case data.SGroup.Toxicity2:
                            return "Протокол испытаний токсичность2.xls";
                        case data.SGroup.AquaAurat:
                            return "Протокол испытаний аквааурат.xls";
                        case data.SGroup.KOCA:
                            return "Протокол испытаний сульфат алюминия.xls";
                        case data.SGroup.Group1:

                            break;
                        case data.SGroup.Group2:
                            break;
                        case data.SGroup.Group3:
                            break;
                    }

                    throw new Exception("неизвестный тип протокола");
                }

                /// <summary>Описание параметров</summary>
                public string Discribe
                {
                    get
                    {
                        var value = "";

                        if (PrtID > 0)
                        { value = "СОХРАНЕН ID=" + PrtID.ToString(); }
                        else
                        { value = "НЕ СОХРАНЕН"; }

                        if (Criterions.Criterions.Length > 0)
                        {
                            for (int i = 0; i < Criterions.Criterions.Length; i++)
                            {
                                var obj = this.Values[Criterions.Criterions[i].Prt_Index];

                                value += '\n' + Criterions.Criterions[i].ToString() + "=" + (obj == null ? "не определено" : Criterions.Criterions[i].GetRowName(obj));
                            }

                            return value;
                        }
                        else
                        { return value + "\nНет параметров"; }
                    }
                }

                public override string ToString()
                { return Criterions.ToString() + ", SC=" + Samples.Count.ToString(); }
            }

            /// <summary>Тип протокола</summary>
            public readonly uint SGroupID;
            ACriterions[] Criterions;

            public ACriterions this[int Index] { get { return Criterions[Index]; } }
            public int Count { get { return Criterions.Length; } }

            public void AddCriteria(int CriteriaIndex)
            {
                Array.Resize(ref Criterions, Criterions.Length + 1);
                Criterions[Criterions.Length - 1] = Base_Criterions[CriteriaIndex];
            }
            public void SetCriteriaLike(int CriterionsIndex)
            {
                if (Base_SGroups[CriterionsIndex].Criterions == null)
                { throw new Exception("Не возможно задать критерии у \"" + this.ToString() + "\" как в CriterionsIndex=" + CriterionsIndex.ToString()); }
                else
                { this.Criterions = Base_SGroups[CriterionsIndex].Criterions; }
            }

            public override string ToString()
            { return T.SGroup.Rows.Get<string>(SGroupID, C.SGroup.Name); }
        }

        public abstract class AValue
        {
            public int ValueIndex;
            public abstract T2 GetValue<T2>();
            public abstract string GetStringValue { get; }
            public abstract bool Check(AValue Value);
        }

        public struct BaseValues
        {
            /// <summary>Место, для усредненного протокола</summary>
            public const byte Area = 0;
            /// <summary>Объект точки отбора</summary>
            public const byte Object = Area + 1;
            /// <summary>Подразделение точки отбора</summary>
            public const byte Podr = Object + 1;
            /// <summary>Объект</summary>
            public const byte OLocation = Podr + 1;
            /// <summary>Объект</summary>
            public const byte PaPoS = OLocation + 1;
            /// <summary>Номер поколения замера</summary>
            public const byte Loc = PaPoS + 1;
            /// <summary>Номер группы точки отбора</summary>
            public const byte SGNum = Loc + 1;
            /// <summary>Тип пробы точки отбора</summary>
            public const byte PType = SGNum + 1;

            public const byte COUNT = PType + 1;
        }

        readonly bool Union;

        readonly DataBase.ISTable Mark = T.Mark.CreateSubTable(false);

        public int MarkCount { get { return Mark.Rows.Count; } }
        public string MarkName(int MarkIndex) { return Mark.Rows.Get<string>(MarkIndex, C.Mark.Name); }

        public int StartDay { get; internal set; }
        public int EndDay { get; internal set; }
        public readonly int YM;

        readonly List<SGroup_class.Protokol_class> Protokols = new List<SGroup_class.Protokol_class>();

        public SGroup_class.Protokol_class this[int Index] { get { return Protokols[Index]; } }
        public int Count { get { return Protokols.Count; } }
        /// <summary>Критерии, которые будут использоваться в этой сессии</summary>
        SGroup_class[] Criterions;
        /// <summary>Типы протоколов</summary>
        static readonly SGroup_class[] Base_SGroups;
        static readonly SGroup_class.ACriterions[] Base_Criterions;
        static SGroup_class GetSG(uint SGID)
        { return Base_SGroups[SGID]; }

        public override string ToString()
        { return "PCount=" + Protokols.Count.ToString(); }
    }
}
