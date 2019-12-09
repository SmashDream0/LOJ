using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Mail;
using System.Security.Authentication;
using ImapX;

namespace LaboratoryOnlineJournal
{
    public partial class Employe_Form : Form
    {
        static bool NeedToGoOut = false;

        public Employe_Form()
        {
            //УДАЛИТЬ В ПОСЛЕДСТВИИ!!!!//////////////////////////

            if (data.DataSourceType == DataBase.RemoteType.MFT)
            { data.User<bool>(C.User.UAll, false); }

            /////////////////////////////////////////////////////

            if (data.Increm > -1)
            { Increment_Form.Tables.Increm = (uint)data.Increm; }

            InitializeComponent();

            RCache.Marks = new RCache.Marks_class();
            RCache.Marks.Update();

            SPoint_Grid.Columns.Add("Num", "№ п/п");
            SPoint_Grid.Columns.Add("Podr", "Подразделение");
            SPoint_Grid.Columns.Add("Name", "Наименование");
            SPoint_Grid.Columns.Add("Volume", "Объём");
            SPoint_Grid.Columns.Add("Object", "Объект");
            SPoint_Grid.Columns.Add("ID", "id");

            SPoint_Grid.RowTemplate.Height = data.User<int>(C.User.SpntH);

            SPoint_Grid.Columns[SPointC.ID].Width = data.User<int>(C.User.SPC1);
            SPoint_Grid.Columns[SPointC.Number].Width = data.User<int>(C.User.SPC2);
            SPoint_Grid.Columns[SPointC.Name].Width = data.User<int>(C.User.SPC3);
            SPoint_Grid.Columns[SPointC.Volume].Width = data.User<int>(C.User.SPC4);
            SPoint_Grid.Columns[SPointC.Object].Width = data.User<int>(C.User.SPC5);
            SPoint_Grid.Columns[SPointC.Podr].Width = data.User<int>(C.User.SPC6);

            SPoint_Grid.VirtualMode = Sample_Grid.VirtualMode = true;

            SCauseEdit_Strip.Text = T.SCause.AlterName;
            Area_Strip.Text = T.Area.AlterName;
            OType_Strip.Text = T.OType.AlterName;
            OLocation_Strip.Text = T.OLocation.AlterName;
            PType_Strip.Text = T.PType.AlterName;
            PType_Strip.Text = T.PType.AlterName;
            PSGM_Strip.Text = T.PSGM.AlterName;
            BackGrd_Strip.Text = T.BackGrd.AlterName;
            Volume_Strip.Text = T.MVolume.AlterName;

            G.Podr.QUERRY().SHOW.WHERE.AC(C.Podr.PSG).More.BV<uint>(1).DO();
            RCache.PSG = new RCache.PSG_class();
            RCache.Volumes = new RCache.Volumes_class();
            SPoints = new SPoints_class(data.User<int>(C.User.YM));
            //this.Resize += this.Employe_Form_Resize;

            int Year, Month;
            ATMisc.GetYearMonthFromYM(data.User<int>(C.User.YM), out Year, out Month);

            CurrentYear_Box.Text = Year_Box.Text = Year.ToString();
            CurrentMonth_Box.Text = Month_Box.Text = Month.ToString();

            if (data.User<bool>(C.User.AlowToGSU) && data.User<int>(C.User.YMDSND) > 1)
            {
                var UMDNow = ATMisc.GetYMDFromDateTime(DateTime.Now);
                var Needed = data.User<int>(C.User.YMDSND);
                string MSGText = null;

                if (UMDNow == Needed)
                {
                    MSGText = "Сегодня день отправки обновлений. Отправить сейчас ?";
                }
                else if (UMDNow > Needed)
                {
                    MSGText = "Похоже, что день отправки обновлений пропущен: " + ATMisc.GetDateTime(data.User<int>(C.User.YMDSND)).ToShortDateString() + ". Отправить сейчас ?";
                }

                if (MSGText != null && MessageBox.Show(this, MSGText, "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                TryAgain: ;

                    var Message = data.SynchPool.GetEncrypted();

                    if (Message == null)
                    {
                        MessageBox.Show(this, "Отправлять нечего", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (new Progress_Form(new DB_Form.SendUpdate_class(Message)).ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        switch (MessageBox.Show(this, "Отправка не удалась, пропустить отправку в этот раз?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                data.IncremUserSendDate();
                                data.SynchPool.AddNewSynch();
                                break;
                            case System.Windows.Forms.DialogResult.No:
                                goto TryAgain;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Операция завершена успешно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        data.IncremUserSendDate();
                        data.SynchPool.AddNewSynch();
                    }
                }
            }

            TimerUpdate();

            switch ((data.UType)data.User<uint>(C.User.UType))
            {
                case data.UType.Employe:
                    Podr_combo.Items.Add(data.User<string>(C.User.Podr, C.Podr.ShrName));
                    Podrs = new uint[] { data.User<uint>(C.User.Podr) };

                    Podr_combo.Enabled = false;
                    ShowUnion_check.Checked = false;

                    break;
                case data.UType.Admin:
                case data.UType.MainEmploye:
                    G.Podr.Sort(C.Podr.ShrName);
                    Podrs = new uint[G.Podr.Rows.Count];

                    Podr_combo.Items.Add("Все");
                    for (int i = 0; i < G.Podr.Rows.Count; i++)
                    {
                        Podr_combo.Items.Add(G.Podr.Rows.Get<string>(i, C.Podr.ShrName) + " (" + G.Podr.Rows.Get<string>(i, C.Podr.PSG, C.PSG.Name) + ")");
                        Podrs[i] = G.Podr.Rows.GetID(i);
                    }

                    ShowUnion_check.Checked = false;
                    break;
                case data.UType.Union:
                    G.Podr.Sort(C.Podr.ShrName);
                    Podrs = new uint[G.Podr.Rows.Count];

                    Podr_combo.Items.Add("Все");
                    for (int i = 0; i < G.Podr.Rows.Count; i++)
                    {
                        Podr_combo.Items.Add(G.Podr.Rows.Get<string>(i, C.Podr.ShrName) + " (" + G.Podr.Rows.Get<string>(i, C.Podr.PSG, C.PSG.Name) + ")");
                        Podrs[i] = G.Podr.Rows.GetID(i);
                    }

                    ShowUnion_check.Checked = true;

                    ShowUnion_check.CheckedChanged += GetSPonts_Changed;
                    MassOutgo_Strip.Visible = false;
                    break;
                default: throw new Exception("Неизвестный тип учетной записи");
            }
            this.FSGSP = new DataBase.table.SubTable.FastSearchGrid_class(SPoint_Grid);

            data.PrgSettings.Forms[(int)data.Forms.Employe].Get(this);

            Podr_combo.SelectedIndex = 0;

            ShowPeriod_button_Click(null, null);

            Sample_Grid.ColumnWidthChanged += Sample_Grid_ColumnWidthChanged;
        }

        public struct SPointC
        {
            public const byte Number = 0;
            public const byte Podr = Number + 1;
            public const byte Name = Podr + 1;
            public const byte Volume = Name + 1;
            public const byte Object = Volume + 1;
            public const byte ID = Object + 1;
        }

        public struct SampleC
        {
            public const byte ID = 0;
            public const byte Loc = ID + 1;
            public const byte Number = Loc + 1;
            public const byte CYMD = Number + 1;
            public const byte AYMD = CYMD + 1;
            public const byte SCause = AYMD + 1;

            public const byte LastColumn = SCause;
        }

        public class MailesCheck_class : Progress_Form.AObject, IDisposable //поиск обновлений 
        {
            public MailesCheck_class(Form Parent, List<LoadDBUpdates_Form.Update_struct> Files)
                : base(true)
            {
                this.Parent = Parent;
                this.Files = Files;
                this.AlowToConnect = true;
            }

            public void Dispose()
            {
                if (Client != null && Client.IsConnected)
                {
                    Client.Disconnect();
                    Client.Dispose();
                }
            }

            string TempPath = Application.StartupPath + "\\принятые\\" + ATMisc.GetMonthName1(DateTime.Now.Month) + "\\" + DateTime.Now.Day.ToString() + "\\";
            ImapX.ImapClient Client;
            Form Parent;
            List<LoadDBUpdates_Form.Update_struct> Files;
            string FullName = (data.EmaleSubjectPart + ' ' + DB_Form.GetImportName(data.UserID));
            string Name = (data.EmaleSubject + ' ' + DB_Form.GetImportName(data.UserID)).Replace(" ", "").ToLower();

            public bool AlowToConnect { get; internal set; }
            public bool Error { get; internal set; }

            bool Connecting()
            {
                if (Client == null)
                {
                    if (data.PrgSettings.Values[(int)data.Strings.AutoCrypto].Bool)
                    {
                        Client = new ImapX.ImapClient(data.PrgSettings.Values[(int)data.Strings.DirectIMAPAdress].String
                                                    , Convert.ToInt32(data.PrgSettings.Values[(int)data.Strings.DirectIMAPPort].Int)
                                                    , data.PrgSettings.Values[(int)data.Strings.AutoCrypto].Bool);
                    }
                    else
                    {
                        var SSL = (SslProtocols)data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int;

                        Client = new ImapX.ImapClient(data.PrgSettings.Values[(int)data.Strings.DirectIMAPAdress].String
                                                    , Convert.ToInt32(data.PrgSettings.Values[(int)data.Strings.DirectIMAPPort].Int)
                                                    , SSL);
                        string abc = "fdsfds";
                        abc.Reverse();
                    }

                    if (!Client.Connect())
                    {
                        AlowToConnect = false;
                        MessageBox.Show(Parent, "Ошибка подключения к почтовому сервису: проверьте настройки подключения либо сервер не доступен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Error = true;
                        return false;
                    }

                    if (!Client.Login(data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String,
                                      data.PrgSettings.Values[(int)data.Strings.DirectMailPass].String))
                    {
                        AlowToConnect = false;
                        MessageBox.Show(Parent, "Ошибка подключения к почтовому сервису: проверьте логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Error = true;
                        return false;
                    }
                }
                AlowToConnect = true;
                return true;
            }

            /// <summary>Отправить письмо согласования еще раз</summary>
            void Resend(uint RequestID, uint PeopleID)
            {

            }

            public void AlterStart() { Start(); }

            protected override bool Do()
            {
                if (!NeedToGoOut)
                {
                    int MaxCount = 4;
                    try
                    {
                        base.Action("Подключение к серверу обновлений", MaxCount, 0);

                        if (!Connecting()) return false;
                    }
                    catch
                    {
                        Parent.Invoke(new MethodInvoker(() => MessageBox.Show(Parent, "Ошибка подключения к почтовому ящику", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                        Action("Ошибка подключения к серверу обновлений", MaxCount, MaxCount);
                        Error = false;
                        return false;
                    }

                    try
                    {
                        Action("Проверка существования пути сохранения", MaxCount, 1);

                        if (!Directory.Exists(TempPath)) Directory.CreateDirectory(TempPath);

                        Action("Отправка запроса", MaxCount, 2);
                        var ReadedMsg = new List<ImapX.Message>();
                        int Count = 3;

                        {
                            var messages = Client.Folders.Inbox.Search("UNSEEN", ImapX.Enums.MessageFetchMode.Headers).ToList();

                            MaxCount = 4 + messages.Count * 2;
                            Action("Получение ответа", MaxCount, 3);

                            for (int Mn = 0; Mn < messages.Count && !NeedToGoOut; Mn++)        //
                            {
                                Action("Обработка обновления " + Mn.ToString() + " из " + messages.Count.ToString(), MaxCount, Count++);

                                var temp1 = messages[Mn].Subject;
                                if (temp1 == null || temp1.Length < data.SendEmaleSubject.Length)
                                { continue; }

                                temp1 = temp1.Replace(" ", "").ToLower();

                                if (temp1.IndexOf(Name) > -1)
                                {
                                    messages[Mn].Download(ImapX.Enums.MessageFetchMode.Full);    //почему-то качает только кол-во вложений, содержимое остаётся недоступным
                                    if (messages[Mn].Attachments.Length == 1)
                                    {
                                        //messages[Mn].Download(ImapX.Enums.MessageFetchMode.Attachments);   //качаю вложения
                                        temp1 = messages[Mn].Attachments[0].FileName;

                                        if (temp1.Length == 0 || temp1 == "Unnamed")
                                        {
                                            Parent.Invoke(new MethodInvoker(() => MessageBox.Show(Parent, "Обнаружена ошибка пустого имени вложения письма.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                                        }
                                        else
                                        {
                                            var FileName = temp1;

                                            if (File.Exists(FileName)) FileName += " (" + DateTime.Now.Ticks.ToString() + ")";

                                            //messages[Mn].Attachments[0].Download();

                                            var str = messages[Mn].Attachments[0].GetTextData();

                                            var Index = str.IndexOf(")IMAPX");
                                            if (Index > -1) str = str.Substring(0, Index);

                                            //messages[Mn].Attachments[0].Save(TempPath, FileName);
                                            var Bytes = Convert.FromBase64String(str);

                                            using (var fs = new FileStream(TempPath + FileName, FileMode.Create))
                                            { fs.Write(Bytes, 0, Bytes.Length); }

                                            Files.Insert(0, new LoadDBUpdates_Form.Update_struct(TempPath + FileName));

                                            ReadedMsg.Add(messages[Mn]);
                                        }
                                    }
                                }
                            }
                        }

                        //удаление писем только после их полной загрузки
                        for (int Mn = 0; Mn < ReadedMsg.Count && !NeedToGoOut; Mn++)        //
                        {
                            Action("Повторная обработка обновления " + Mn.ToString() + " из " + ReadedMsg.Count.ToString(), MaxCount, Count++);
                            ReadedMsg[Mn].Seen = true;   //удаляю письмо
                        }
                        Action("Проверка завершена", MaxCount, MaxCount);
                    }
                    catch (BadImageFormatException ex)
                    {
                        Action("Ошибка загрузки обновления", MaxCount, MaxCount);
                        Parent.Invoke(new MethodInvoker(() => MessageBox.Show(ex.Message)));
                        return false;
                    }
                }
                return true;
            }
        }

        Thread MailCheck_Thread = null;
        MailesCheck_class MailesCheck;
        List<LoadDBUpdates_Form.Update_struct> Files = new List<LoadDBUpdates_Form.Update_struct>();
        uint[] Podrs;
        DataBase.table.SubTable.FastSearchGrid_class FSGSP;

        /// <summary>Точки отбора</summary>
        public class SPoints_class
        {
            public SPoints_class(int YM)
            {
                this.Sample = T.Sample.CreateSubTable();
                this.SPoint = T.SPoint.CreateSubTable();
                this.SM = T.SM.CreateSubTable();

                this.Sample.CanSortRefresh = this.SPoint.CanSortRefresh = this.SM.CanSortRefresh = false;

                Sample.Rows.AfterAddRow += AfterAddSample;
                SPoint.Rows.AfterAddRow += AfterAddSPoint;

                _YM = YM;
            }

            public struct Mark_struct
            {
                public Mark_struct(uint MID, int ColumnIndex)
                {
                    this.MID = MID;
                    this.ColumnIndex = ColumnIndex;
                }

                uint MID;
                public readonly int ColumnIndex;
                public string Name { get { return T.Mark.Rows.Get<string>(MID, C.Mark.Name); } }

                public override string ToString()
                {
                    return ColumnIndex.ToString() + ")" + Name.ToString();
                }
            }

            RCache.Marks_class Marks = new RCache.Marks_class();

            public int MarkCount { get { return Marks.Count; } }

            public string Name(int MarkIndex) { return Marks[MarkIndex].Name; }

            public data.VarType VarType(int MarkIndex) { return Marks[MarkIndex].VarType; }

            public int Round(int MarkIndex) { return Marks[MarkIndex].Round; }

            int _YM;

            public bool MoveSPoint(int SPointIndex, sbyte Direction)
            {
                if (SPointIndex > -1 && SPointIndex + Direction > -1 && SPointIndex + Direction < SPoints.Length)
                {
                    var SP = SPoints[SPointIndex];

                    {
                        var numtmp = SP.Number;

                        SP.Number = SPoints[SPointIndex + Direction].Number;
                        SPoints[SPointIndex + Direction].Number = numtmp;
                    }

                    SPoints[SPointIndex] = SPoints[SPointIndex + Direction];
                    SPoints[SPointIndex + Direction] = SP;

                    return true;
                }
                else
                { return false; }
            }

            public int YM
            {
                get { return _YM; }
                internal set
                {
                    this._YM = value;
                    UpdateSPoints();
                }
            }

            /// <summary>День старта текущего периода - 1</summary>
            public int StartDay { get; internal set; }
            /// <summary>День окончания текущего периода + 1</summary>
            public int EndDay { get; internal set; }

            uint PodrID;
            bool ShowClosed;
            bool ShowUnion;

            public void UpdateSPoints()
            { UpdateSPoints(YM, PodrID, ShowClosed, ShowUnion); }

            public void UpdateSPoints(uint PodrID, bool ShowClosed, bool ShowUnion)
            { UpdateSPoints(YM, PodrID, ShowClosed, ShowUnion); }

            /// <summary>Показать точки отбора</summary>
            /// <param name="PodrID">Идентификатор подразделение</param>
            /// <param name="ShowClosed">Показать закрытые</param>
            /// <param name="ShowOther">Показать с произвольным типом</param>
            public void UpdateSPoints(int YM, uint PodrID, bool ShowClosed, bool ShowUnion)
            {
                this._YM = YM;
                this.PodrID = PodrID;
                this.ShowClosed = ShowClosed;
                this.ShowUnion = ShowUnion;

                this.Marks.Update();

                DataBase.IMathCondition SMSS, SPS, GSMSShow;

                this.StartDay = ATMisc.GetYMDFromYM(YM) - 1;
                this.EndDay = StartDay + ATMisc.GetDaysInMonth(YM) + 1;

                if (PodrID == 0)
                {
                    GSMSShow = G.SMS.QUERRY().SHOW.WHERE
                        .ARC(C.SMS.SPoint, C.SPoint.Podr).More.BV<uint>(PodrID)
                        .AND.ARC(C.SMS.SPoint, C.SPoint.Union).EQUI.BV(ShowUnion)
                        .AND.AC(C.SMS.SPoint).More.BV<uint>(0)
                        .AND.ARC(C.SMS.SPoint, C.SPoint.Number).More.BV<int>(0);

                    SPS = SPoint.QUERRY().SHOW.WHERE
                        .AC(C.SPoint.Podr).More.BV<uint>(PodrID)
                        .AND.C(C.SPoint.Union, ShowUnion)
                        .AND.AC(C.SPoint.Number).More.BV<int>(0);
                }
                else
                {
                    GSMSShow = G.SMS.QUERRY().SHOW.WHERE
                        .ARC(C.SMS.SPoint, C.SPoint.Podr).EQUI.BV<uint>(PodrID)
                        .AND.ARC(C.SMS.SPoint, C.SPoint.Union).EQUI.BV(ShowUnion)
                        .AND.AC(C.SMS.SPoint).More.BV<uint>(0)
                        .AND.ARC(C.SMS.SPoint, C.SPoint.Number).More.BV<int>(0);

                    SPS = SPoint.QUERRY().SHOW.WHERE
                        .AC(C.SPoint.Podr).EQUI.BV<uint>(PodrID)
                        .AND.C(C.SPoint.Union, ShowUnion)
                        .AND.AC(C.SPoint.Number).More.BV<int>(0);
                }

                if (ShowClosed)
                {
                    SPS.DO();
                }
                else
                {
                    GSMSShow
                        .AND.ARC(C.SMS.SPoint, C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.OB()
                            .ARC(C.SMS.SPoint, C.SPoint.YMDE).More.BV<int>(StartDay)
                            .OR.ARC(C.SMS.SPoint, C.SPoint.YMDE).EQUI.BV<int>(0);

                    SPS.AND.AC(C.SPoint.YMDS).Less.BV(EndDay)
                        .AND.OB()
                            .AC(C.SPoint.YMDE).More.BV<int>(StartDay)
                            .OR.AC(C.SPoint.YMDE).EQUI.BV<int>(0)
                        .CB()
                        .DO();
                }

                GSMSShow.DO();

                SPoints = new SPoint_class[SPoint.Rows.Count];

                for (int i = 0; i < SPoints.Length; i++)
                { SPoints[i] = new SPoint_class(this, SPoint.Rows.GetID(i)); }

                for (int i = 0; i < G.SMS.Rows.Count; i++)
                {
                    if (G.SMS.Rows.GetStatus(i) == DataBase.State.Normal)
                    {
                        var MIndex = this.Marks.GetMarkIndex(G.SMS.Rows.Get_UnShow<uint>(i, C.SMS.Mark));

                        if (MIndex > -1)
                        {
                            var SPIndex = SPoint.Rows.GetIndex(G.SMS.Rows.Get_UnShow<uint>(i, C.SMS.SPoint));

                            if (SPIndex > -1)
                            { SPoints[SPIndex].Marks[MIndex] = new SPoint_class.Mark_struct(G.SMS.Rows.GetID(i)); }
                        }
                    }
                }

                for (int i = 0; i < SPoint.Rows.Count; i++)
                {
                    if (SPoint.Rows.Get<bool>(i, C.SPoint.IMLst))
                    {
                        for (int j = 0; j < this.Marks.Count; j++)
                        { SPoints[i].Marks[j].AllowToUse = true; }
                    }
                }

                Array.Sort(SPoints, (it1, it2) =>
                    {
                        var ret1 = it1.Number.CompareTo(it2.Number);

                        if (ret1 == 0)
                        { return it1.SPointID.CompareTo(it2.SPointID); }
                        else
                        { return ret1; }
                    });

                SPoint.Sort((it1, it2) =>
                    {
                        var ret1 = SPoint.Rows.Get<int>(it1, C.SPoint.Number).CompareTo(SPoint.Rows.Get<int>(it2, C.SPoint.Number));

                        if (ret1 == 0)
                        { return it1.ID.CompareTo(it2.ID); }
                        else
                        { return ret1; }
                    });
            }

            /// <summary>Один замер</summary>
            public class Sample_class
            {
                public Sample_class(SPoint_class Parent, uint SampleID)
                {
                    this.SampleID = SampleID;
                    var SPointID = T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint);
                    this.Parent = Parent;
                    this.Marks = new Mark_struct[Parent.Parent.Marks.Count];
                    ClearMarks();
                    this.CanDelete = true;
                }

                public int Index { get { return Parent.Parent.Sample.Rows.GetIndex(SampleID); } }

                public int Loc
                {
                    get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.Loc); }
                    set { T.Sample.Rows.Set<int>(SampleID, C.Sample.Loc, value); }
                }

                public int Number
                {
                    get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.Number); }
                    set
                    {
                        if (value >= 0)
                        {
                            if (value > 0)
                            {
                                uint ID;

                                if (T.SPoint.Rows.Get<bool>(SPointID, C.SPoint.Union))
                                {
                                    ID = (uint)Parent.Parent.Sample.QUERRY()
                                          .GET.ID().WHERE
                                          .AC(C.Sample.CYMD).Less.BV(Parent.Parent.EndDay)
                                          .AND.AC(C.Sample.CYMD).More.BV(Parent.Parent.StartDay)
                                          .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                                          .AND.C(C.Sample.Number, value).DO()[0].Value;
                                }
                                else
                                {
                                    ID = (uint)Parent.Parent.Sample.QUERRY()
                                        .GET.ID().WHERE
                                        .AC(C.Sample.CYMD).Less.BV(Parent.Parent.EndDay)
                                        .AND.AC(C.Sample.CYMD).More.BV(Parent.Parent.StartDay)
                                        .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(T.SPoint.Rows.Get_UnShow<uint>(Parent.SPointID, C.SPoint.Podr))
                                        .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                                        .AND.C(C.Sample.Number, value).DO()[0].Value;
                                }
                                if (ID > 0)
                                {
                                    MessageBox.Show("Указанный номер используется в отборе этого месяца: " + T.Sample.Rows.Get<string>(ID, C.Sample.SPoint, C.SPoint.Name), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }

                            T.Sample.Rows.Set(this.SampleID, C.Sample.Number, value);
                        }
                    }
                }

                public struct Mark_struct
                {
                    public Mark_struct(Sample_class Parent, uint MarkID)
                    {
                        this.MarkID = MarkID;
                        this._SMID = 0;
                        this.Parent = Parent;
                    }
                    public Mark_struct(Sample_class Parent, uint MarkID, uint SMID)
                    {
                        this.MarkID = T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark);
                        if (MarkID != this.MarkID)
                        { throw new Exception("Не верно задан SMID"); }
                        this._SMID = SMID;
                        this.Parent = Parent;
                    }

                    Sample_class Parent;
                    uint MarkID;

                    uint _SMID;
                    public uint SMID { get { return _SMID; } }
                    public data.VarType VarType { get { return (data.VarType)T.Mark.Rows.Get_UnShow<uint>(MarkID, C.Mark.VarType); } }

                    public double Amount
                    {
                        get
                        {
                            switch (VarType)
                            {
                                case data.VarType.Bool:
                                    return (SMID == 0 ? 0 : (T.SM.Rows.Get<double>(SMID, C.SM.Amount) > 0 ? 1 : 0));
                                case data.VarType.dbl:
                                    return (SMID == 0 ? 0 : T.SM.Rows.Get<double>(SMID, C.SM.Amount));
                                case data.VarType.i32:
                                    return (SMID == 0 ? 0 : T.SM.Rows.Get<int>(SMID, C.SM.Amount));
                            }

                            return 0;
                        }
                        set
                        {
                            if (value >= 0)
                            {
                                if (Parent.Parent.NormID == 0)
                                {
                                    MessageBox.Show("Точке отбора не назначен норматив, ввести значение показателя невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                                else if (Parent.Parent.Marks[Parent.Parent.Parent.Marks[MarkID].Number - 1].AllowToUse)
                                {
                                    var Norm = Parent.Parent.Parent.Marks[MarkID].GetNorm(Parent.Parent.NormID);

                                    if (Norm.InputFrom > value &&
                                        MessageBox.Show("Указанное значение меньше допустимого диапазона: " + Norm.InputRange + "\r\nВы уверены, что хотите его внести?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No ||
                                        Norm.InputTo < value &&
                                        MessageBox.Show("Указанное значение больше допустимого диапазона: " + Norm.InputRange + "\r\nВы уверены, что хотите его внести?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                    { return; }
                                    else
                                    {
                                        switch (VarType)
                                        {
                                            case data.VarType.Bool:
                                                value = (value > 0 ? 1 : 0);
                                                break;
                                            case data.VarType.i32:
                                                value = (value < 0 ? 0 : Math.Round(value));
                                                break;
                                            case data.VarType.dbl:
                                                value = (value < 0 ? 0 : value);
                                                break;
                                            default: throw new Exception(VarType.ToString() + " - неизвестный тип значения");
                                        }

                                        if (SMID == 0)
                                        {
                                            /*var Values = new object[T.SM.Columns.Count];
                                            Values[C.SM.Amount] = value;
                                            Values[C.SM.Mark] = MarkID;
                                            Values[C.SM.Sample] = Parent.SampleID;

                                            if (Parent.Parent.Parent.SM.Rows.Add(Values))*/
                                            if ((bool)Parent.Parent.Parent.SM.QUERRY()
                                                .ADD
                                                .C(C.SM.Amount, value)
                                                .C(C.SM.Mark, MarkID)
                                                .C(C.SM.Sample, Parent.SampleID)
                                                .DO()[0].Value)
                                            { this._SMID = Parent.Parent.Parent.SM.Rows.GetID(Parent.Parent.Parent.SM.Rows.Count - 1); }
                                        }
                                        else
                                        { T.SM.Rows.Set<double>(SMID, C.SM.Amount, value); }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Этот показатель не допускается для ввода в замер с точкой отбора - " + T.SPoint.Rows.Get<string>(Parent.SPointID, C.SPoint.Name), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                            else
                            { MessageBox.Show("Недопустимое значение концентрации", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                        }
                    }
                    /// <summary>Существует-ли запись</summary>
                    public bool ExistSM { get { return SMID > 0; } }

                    public string MarkName { get { return T.Mark.Rows.Get<string>(MarkID, C.Mark.Name); } }

                    public override string ToString()
                    {
                        return MarkName + "=" + Amount.ToString();
                    }
                }

                void ClearMarks()
                {
                    for (int i = 0; i < Marks.Length; i++)
                    { Marks[i] = new Mark_struct(this, Parent.Parent.Marks[i].ID); }
                }

                public readonly SPoint_class Parent;
                uint SPointID { get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SPoint); } }
                public readonly uint SampleID;
                public bool CanDelete { get; internal set; }

                public void SetPrtS(uint PrtSID)
                {
                    if (T.PrtS.Rows.Get_UnShow<uint>(PrtSID, C.PrtS.Sample) == this.SampleID)
                    { CanDelete = false; }
                }

                public readonly Mark_struct[] Marks;

                public int SPiontIndex { get { return G.SPoint.Rows.GetIndex(SPointID); } }
                public int SampleIndex { get { return Parent.Parent.Sample.Rows.GetIndex(SampleID); } }

                public string Location { get { return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Name); } }
                public bool ItAdded { get { return SampleID > 0; } }

                /// <summary>День создания</summary>
                public DateTime CDate
                {
                    get { return ATMisc.GetDateTime(T.Sample.Rows.Get<int>(SampleID, C.Sample.CYMD)); }
                    set { T.Sample.Rows.Set<int>(SampleID, C.Sample.CYMD, ATMisc.GetYMDFromDateTime(value)); }
                }
                /// <summary>День анализа</summary>
                public DateTime ADate
                {
                    get { return ATMisc.GetDateTime(T.Sample.Rows.Get<int>(SampleID, C.Sample.AYMD)); }
                    set { T.Sample.Rows.Set<int>(SampleID, C.Sample.AYMD, ATMisc.GetYMDFromDateTime(value)); }
                }
                public int AYMD { get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.AYMD); } }
                public int CYMD { get { return T.Sample.Rows.Get<int>(SampleID, C.Sample.CYMD); } }
                /// <summary>Причина</summary>
                public string SCause
                {
                    get { return T.Sample.Rows.Get<string>(SampleID, C.Sample.SCause); }
                }
                /// <summary>Причина</summary>
                public uint SCauseID
                {
                    get { return T.Sample.Rows.Get_UnShow<uint>(SampleID, C.Sample.SCause); }
                    set { T.Sample.Rows.Set<uint>(SampleID, C.Sample.SCause, value); }
                }

                /*public data.Period Period { get { return (data.Period)T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Period); } }
                public string SPeriod { get { return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Period); } }
                public int CountPerPeriod { get { return T.SPoint.Rows.Get_UnShow<int>(SPointID, C.SPoint.CountpP); } }*/

                /// <summary>Для загрузки введенных показателей</summary>
                /// <param name="SMID">Идентификатор записи введенного показателя</param>
                public void SetSM(uint SMID)
                {
                    var Index = Parent.Parent.Marks.GetMarkIndex(T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark));
                    if (Index > -1)
                    { this.Marks[Index] = new Mark_struct(this, T.SM.Rows.Get_UnShow<uint>(SMID, C.SM.Mark), SMID); }
                }
            }

            public class SPoint_class
            {
                public SPoint_class(SPoints_class Parent, uint SPointID)
                {
                    this.SPointID = SPointID;
                    this.Parent = Parent;
                    //this._Period = CurrentPeriodNumber;
                    this.Marks = new Mark_struct[Parent.Marks.Count];

                    for (int i = 0; i < this.Marks.Length; i++)
                    { this.Marks[i] = new Mark_struct(Parent.Marks[i].Name); }

                    MinDay = ATMisc.GetYMDFromDateTime(new DateTime(1800, 1, 1));
                    MaxDay = ATMisc.GetYMDFromDateTime(new DateTime(2100, 1, 1));
                }

                int MinDay, MaxDay;
                public readonly uint SPointID;
                public readonly SPoints_class Parent;
                public bool Union { get { return T.SPoint.Rows.Get<bool>(SPointID, C.SPoint.Union); } }

                public struct Mark_struct
                {
                    public Mark_struct(uint SMSID)
                    {
                        this.SMSID = SMSID;
                        this.Name = T.SMS.Rows.Get<string>(SMSID, C.SMS.Mark, C.Mark.Name);
                        this.AllowToUse = true;
                    }
                    public Mark_struct(string Name)
                    {
                        this.SMSID = 0;
                        this.Name = Name;
                        this.AllowToUse = false;
                    }
                    readonly uint SMSID;
                    public readonly string Name;
                    public bool AllowToUse;
                }

                public struct OperType_struct
                {}

                public readonly Mark_struct[] Marks;

                public int Index { get { return Parent.SPoint.Rows.GetIndex(this.SPointID); } }

                public string Name { get { return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Name); } }
                public string OLocation { get { return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Name); } }
                public string PodrShortName { get { return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Podr, C.Podr.ShrName); } }
                public uint PodrID { get { return T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Podr); } }
                /// <summary>Всё-ли нормально с началом периода и началом/окончанием использования точки отбома</summary>
                public bool ItsOK
                {
                    get
                    {
                        var YMs = ATMisc.GetYMFromYMD(T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDS));
                        var YMe = ATMisc.GetYMFromYMD(T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDE));
                        return YMs <= Parent.YM &&
                            (T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDE) == 0 || YMe >= Parent.YM);
                    }
                }

                public uint BackGrdID { get { return T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.BackGrd); } }
                public bool AreBackGrd { get { return T.SPoint.Rows.Get_UnShow<bool>(SPointID, C.SPoint.BckGnd); } }
                public bool UseBackGrd { get { return T.SPoint.Rows.Get_UnShow<bool>(SPointID, C.SPoint.UsBckGnd); } }
                public uint ObjectID { get { return T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object); } }
                public string Object { get { return T.SPoint.Rows.Get_UnShow<string>(SPointID, C.SPoint.Object, C.Object.Name); } }
                public bool Volumed { get { return T.SPoint.Rows.Get_UnShow<bool>(SPointID, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed); } }
                /// <summary>Номер дня с которого действует текущая точка отбора</summary>
                public int EnabledPeriodFrom { get { return T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDS); } }
                /// <summary>Номер дня по который действует текущая точка отбора</summary>
                public int EnabledPeriodTo { get { return T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDE); } }

                public int YMStart { get { return ATMisc.GetYMFromYMD(T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDS)); } }
                public int YMEnd { get { return ATMisc.GetYMFromYMD(T.SPoint.Rows.Get<int>(SPointID, C.SPoint.YMDE)); } }

                public int Number
                {
                    get { return Parent.SPoint.Parent.Rows.Get<int>(this.SPointID, C.SPoint.Number); }
                    set { Parent.SPoint.Parent.Rows.Set(this.SPointID, C.SPoint.Number, value); }
                }

                public uint NormID { get { return T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object, C.Object.Norm); } }

                Sample_class[] Samples = null;

                public Sample_class this[int SampleIndex]
                {
                    get
                    {
                        LoadSamples();
                        return Samples[SampleIndex];
                    }
                }
                public int SampleCount
                {
                    get
                    {
                        LoadSamples();
                        return Samples.Length;
                    }
                }

                public void AddSample(uint SID)
                {
                    if (SID > 0)
                    {
                        if (Samples == null)
                        { Samples = new Sample_class[1]; }
                        else
                        {
                            for (int i = 0; i < Samples.Length; i++)
                            {
                                if (Samples[i].SampleID == SID)
                                { return; }
                            }

                            Array.Resize(ref Samples, Samples.Length + 1);
                        }

                        Samples[Samples.Length - 1] = new Sample_class(this, SID);

                        if (Samples[Samples.Length - 1].Loc != Samples.Length)
                        { Samples[Samples.Length - 1].Loc = Samples.Length; }
                    }
                }

                public int DeleteSample(params int[] SampleIndex)   //тут что-то не то
                {
                    var SI = new List<int>(SampleIndex);

                    for (int i = SI.Count - 1; i > -1; i--)
                    {
                        if (!Samples[SampleIndex[i]].CanDelete)
                        { SI.RemoveAt(i); }
                    }

                    if (SI.Count > 0)
                    {
                        SI.Sort();

                        var SQ = G.Sample.QUERRY().SET.Delete().WHERE.ID(0);
                        var SMQ = G.SM.QUERRY().SET.Delete().WHERE.ID(0);

                        for (int i = 0; i < SI.Count; i++)
                        {
                            SQ.OR.ID(Samples[SI[i]].SampleID);

                            for (int j = 0; j < Samples[SI[i]].Marks.Length; j++)
                            {
                                if (Samples[SI[i]].Marks[j].SMID > 0)
                                { SMQ.OR.ID(Samples[SI[i]].Marks[j].SMID); }
                            }
                        }

                        SQ.DO();
                        SMQ.DO();

                        for (int i = SI.Count - 1; i > -1; i--)
                        { Array.Copy(Samples, SI[i] + 1, Samples, SI[i], Samples.Length - SI[i] - 1); }

                        Array.Resize(ref Samples, Samples.Length - SI.Count);

                        for (int i = SI[0]; i < Samples.Length; i++)
                        { Samples[i].Loc = i + 1; }
                    }

                    return SI.Count;
                }

                public void LoadSamples()
                {
                    if (Samples == null)
                    {
                        Parent.SM.QUERRY()
                              .SHOW
                              .WHERE
                                   .ARC(C.SM.Sample, C.Sample.CYMD).More.BV(Parent.StartDay)
                                   .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(Parent.EndDay)
                                   .AND.ARC(C.SM.Sample, C.Sample.SPoint).EQUI.BV(this.SPointID)
                              .DO();

                        Parent.Sample.QUERRY()
                            .SHOW
                            .WHERE
                                .AC(C.Sample.CYMD).More.BV(Parent.StartDay)
                                .AND.AC(C.Sample.CYMD).Less.BV(Parent.EndDay)
                                .AND.C(C.Sample.SPoint, this.SPointID)
                              .DO();

                        this.Samples = new Sample_class[Parent.Sample.Rows.Count];

                        if (Parent.Sample.Rows.Count > 0)
                        {
                            var PrtS = T.PrtS.CreateSubTable(false);
                            var PrtSQ = PrtS.QUERRY().SHOW.WHERE.C<uint>(C.PrtS.Sample, 0);

                            for (int i = 0; i < Parent.Sample.Rows.Count; i++)
                            {
                                Samples[i] = new Sample_class(this, Parent.Sample.Rows.GetID(i));
                                PrtSQ.OR.C(C.PrtS.Sample, Samples[i].SampleID);
                            }

                            PrtSQ.DO();

                            for (int i = 0; i < Parent.SM.Rows.Count; i++)
                            {
                                var SIndex = Parent.Sample.Rows.GetIndex(Parent.SM.Rows.Get_UnShow<uint>(i, C.SM.Sample));
                                if (SIndex > -1)
                                { Samples[SIndex].SetSM(Parent.SM.Rows.GetID(i)); }
                            }

                            for (int i = 0; i < PrtS.Rows.Count; i++)
                            {

                            }

                            Array.Sort(Samples, (it1, it2) => it1.Loc.CompareTo(it2.Loc));
                        }
                    }
                }

                public void Move(int SampleIndex, sbyte Direction)
                {
                    if (Samples.Length > SampleIndex + Direction && SampleIndex + Direction > -1)
                    {
                        var NewRI = SampleIndex + Direction;

                        Samples[SampleIndex].Loc = NewRI + 1;
                        Samples[NewRI].Loc = SampleIndex + 1;

                        var S = Samples[SampleIndex];
                        Samples[SampleIndex] = Samples[NewRI];
                        Samples[NewRI] = S;
                    }
                }

                public override string ToString()
                {
                    return T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Podr) + "(" + T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Object) + ") - " + this.OLocation;
                }
            }

            public DataBase.ISTable Sample, SPoint, SM;

            SPoint_class[] SPoints;

            public SPoint_class this[int SPointIndex] { get { return SPoints[SPointIndex]; } }
            public int SPointsCount { get { return SPoints.Length; } }

            void AfterAddSPoint(DataBase.ISTable Table, uint ID)
            {
                if (SPoints.Length < SPoint.Rows.Count)
                { Array.Resize(ref SPoints, SPoints.Length + 1); }

                SPoints[SPoint.Rows.Count - 1] = new SPoint_class(this, ID);
            }

            public void DeleteSPoint(int Index)
            {
                if (SPoints[Index].SampleCount > 0)
                {
                    var samples = new List<uint>();
                    var sms = new List<uint>();

                    for (int i = 0; i < SPoints[Index].SampleCount; i++)
                    {
                        samples.Add(SPoints[Index][i].SampleID);

                        for (int j = 0; j < SPoints[Index][i].Marks.Length; j++)
                        {
                            if (SPoints[Index][i].Marks[j].ExistSM)
                            { sms.Add(SPoints[Index][i].Marks[j].SMID); }
                        }
                    }

                    if (samples.Count > 0)
                    {
                        var SetSample = G.Sample.QUERRY().SET.Delete().WHERE.ID(samples[0]);

                        for (int i = 1; i < samples.Count; i++)
                        { SetSample.OR.ID(samples[i]); }

                        SetSample.DO();
                    }

                    if (sms.Count > 0)
                    {
                        var SetMark = G.SM.QUERRY().SET.Delete().WHERE.ID(sms[0]);

                        for (int i = 1; i < sms.Count; i++)
                        { SetMark.OR.ID(sms[i]); }

                        SetMark.DO();
                    }
                }

                SPoint.Rows.Delete(SPoints[Index].SPointID);

                if (Index + 1 < SPoints.Length)
                {
                    Array.Copy(SPoints, Index + 1, SPoints, Index, SPoints.Length - Index - 1);
                    Array.Resize(ref SPoints, SPoints.Length - 1);
                }
            }

            void AfterAddSample(DataBase.ISTable Table, uint ID)
            {
                var SPRI = SPoint.Rows.GetIndex(T.Sample.Rows.Get_UnShow<uint>(ID, C.Sample.SPoint));

                if (SPRI > -1)
                { SPoints[SPRI].AddSample(ID); }
            }
        }

        public static SPoints_class SPoints;

        public static bool StandartEnabled
        { get { return Employe_Form.SPoints.YM == Employe_Form.WorkYM && (((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye || (data.UType)data.User<uint>(C.User.UType) == data.UType.Union)); } }

        public static int WorkYM
        {
            get { return data.User<int>(C.User.YM); }
            set
            {
                if (data.User<int>(C.User.YM) != value)
                { data.User<int>(C.User.YM, value); }
            }
        }

        void CreateSampleColumns()
        {
            Sample_Grid.Columns.Clear();

            Sample_Grid.Columns.Add("ID", "id");
            Sample_Grid.Columns.Add("Loc", "Порядок");
            Sample_Grid.Columns.Add("Number", "Номер");
            Sample_Grid.Columns.Add("CYMD", "День замера");
            Sample_Grid.Columns.Add("AYMD", "День анализа");
            Sample_Grid.Columns.Add("SCause", "Причина");

            Sample_Grid.RowTemplate.Height = data.User<int>(C.User.SplH);

            Sample_Grid.Columns[SampleC.ID].Width = data.User<int>(C.User.SC1);
            Sample_Grid.Columns[SampleC.AYMD].Width = data.User<int>(C.User.SC2);
            Sample_Grid.Columns[SampleC.CYMD].Width = data.User<int>(C.User.SC3);
            Sample_Grid.Columns[SampleC.Number].Width = data.User<int>(C.User.SC4);
            Sample_Grid.Columns[SampleC.SCause].Width = data.User<int>(C.User.SC5);
            Sample_Grid.Columns[(int)SampleC.Loc].Width = data.User<int>(C.User.SC6);

            var Format = "0." + new string('#', 40);
            var Width = data.User<int>(C.User.SCM);

            for (int i = 0; i < SPoints.MarkCount; i++)
            {

                switch (SPoints.VarType(i))
                {
                    case data.VarType.Bool:
                        var column = new DataGridViewColumn();
                        column.HeaderText = SPoints.Name(i);
                        column.Name = "mark_" + (i + 1).ToString();
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                        column.ValueType = typeof(bool);
                        column.CellTemplate = new DataGridViewCheckBoxCell();
                        Sample_Grid.Columns.Add(column);
                        break;
                    default:
                        Sample_Grid.Columns.Add("mark_" + (i + 1).ToString(), SPoints.Name(i));
                        Sample_Grid.Columns[Sample_Grid.Columns.Count - 1].DefaultCellStyle.Format = Format;
                        break;
                }

                Sample_Grid.Columns[Sample_Grid.Columns.Count - 1].Width = Width;
            }
        }

        void TimerUpdate()
        {
            if (data.User<bool>(C.User.AlowToGSU) && DB_Form.UpdatePeriod > 0)
            {
                UpdateDB_timer.Enabled = true;
                UpdateDB_timer.Interval = DB_Form.UpdatePeriod;

                MailCheck_Thread = new Thread(() => { });
                MailesCheck = new MailesCheck_class(this, this.Files);
            }
            else
            {
                UpdateDB_timer.Enabled = false;
            }
        }

        private void About_Strip_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void Close_Strip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserEdit_Strip_Click(object sender, EventArgs e)
        {
            G.User.Rows.GetEditRow_Form(data.UserID, 500).ShowDialog();
        }

        private void Marks_Strip_Click(object sender, EventArgs e)
        {
            new MarksCompanyEdit_Form().ShowDialog();
        }

        private void PodrPpl_Strip_Click(object sender, EventArgs e)
        {
            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Union)
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.C(C.PodrPpl.Podr, data.User<uint>(C.User.Podr)).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV(data.User<uint>(C.User.Podr))
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                        .DO();
            }
            else
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.AC(C.PodrPpl.Podr).EQUI.BV<uint>(Podrs[Podr_combo.SelectedIndex]).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV<uint>(Podrs[Podr_combo.SelectedIndex])
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                        .DO();
            }

            G.Resp.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void Employe_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Sample_Grid.IsCurrentCellInEditMode) Sample_Grid.EndEdit();

            UpdateDB_timer.Enabled = false;

            NeedToGoOut = true;

            if (MailCheck_Thread != null && MailCheck_Thread.IsAlive)
                MailCheck_Thread.Join();//прерываем поток

            data.PrgSettings.Forms[(int)data.Forms.Employe].Set(this);

            var UserE = new DataBase.FastSet_class(G.User, data.UserID);

            UserE.C(C.User.SC1, Sample_Grid.Columns[SampleC.ID].Width);
            UserE.C(C.User.SC2, Sample_Grid.Columns[SampleC.AYMD].Width);
            UserE.C(C.User.SC3, Sample_Grid.Columns[SampleC.CYMD].Width);
            UserE.C(C.User.SC4, Sample_Grid.Columns[SampleC.Number].Width);
            UserE.C(C.User.SC5, Sample_Grid.Columns[SampleC.SCause].Width);
            UserE.C(C.User.SC6, Sample_Grid.Columns[SampleC.Loc].Width);

            if (Sample_Grid.Columns.Count > SampleC.LastColumn)
            { UserE.C(C.User.SCM, Sample_Grid.Columns[SampleC.LastColumn + 1].Width); }

            UserE.C(C.User.SPC1, SPoint_Grid.Columns[SPointC.ID].Width);
            UserE.C(C.User.SPC2, SPoint_Grid.Columns[SPointC.Number].Width);
            UserE.C(C.User.SPC3, SPoint_Grid.Columns[SPointC.Name].Width);
            UserE.C(C.User.SPC4, SPoint_Grid.Columns[SPointC.Volume].Width);
            UserE.C(C.User.SPC5, SPoint_Grid.Columns[SPointC.Object].Width);
            UserE.C(C.User.SPC6, SPoint_Grid.Columns[SPointC.Podr].Width);

            UserE.C(C.User.SpntH, SPoint_Grid.RowTemplate.Height);
            UserE.C(C.User.SplH, Sample_Grid.RowTemplate.Height);

            UserE.C(C.User.SplDist, SPoint_split.SplitterDistance);

            UserE.DO();
        }

        void SetNext(TextBox text, int Step)
        {
            text.Text = (Convert.ToInt32(text.Text) + Step).ToString();
        }

        private void AddSPoint_Strip_Click(object sender, EventArgs e)
        {
            if (!AddSPoint_Strip.Enabled)
            { return; }

            var SPED = new Edit_Form(SPoints.SPoint, 750, false);

            uint PodrID;
            if (SPoint_Grid.CurrentCell != null)
            { PodrID = SPoints.SPoint.Rows.Get_UnShow<uint>(SPoint_Grid.CurrentCell.RowIndex, C.SPoint.Podr); }
            else
            { PodrID = data.User<uint>(C.User.Podr); }

            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Number, false, (int)G.SPoint.QUERRY().COUNT.DO()[0].Value + 1));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Union, false, data.User<uint>(C.User.UType) == (uint)data.UType.Union, data.User<uint>(C.User.UType) == (uint)data.UType.MainEmploye));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Name));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Area, G.Area));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Object, G.Object));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BckGnd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BackGrd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.UsBckGnd));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.SGroup, G.SGroup));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.SGNum));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PaPoS));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDS, false, true, SPoints.StartDay + 1));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDE, true, true, ATMisc.GetYMDFromDateTime(ATMisc.GetDateTime(SPoints.StartDay + 1).AddMonths(1).AddDays(-1))));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Podr, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, PodrID));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.IMLst));

            SPED.SetChecks(C.SPoint.Checks);

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
            {
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.SMS
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.TestCond
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }
            else
            {
                SPED.AddSubEdit(G.SMS
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(G.TestCond
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }

            if (SPED.ShowDialog() == DialogResult.OK)
            {
                this.SPoint_Grid.RowCount++;
                //Обновляю разрешения на ввод показателей
                this.SPoint_Grid.Invalidate();
            }
        }

        private void EditSPoint_Strip_Click(object sender, EventArgs e)
        {
            if (this.SPoint_Grid.CurrentCell == null || !EditSPoint_Strip.Enabled)
            { return; }

            var SPED = new Edit_Form(SPoints.SPoint, SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, 750, false);

            bool CanEdit = data.User<uint>(C.User.UType) != (uint)data.UType.Employe;
            var PType = new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType, true, CanEdit);

            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Number, false));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Name));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Union, data.User<uint>(C.User.UType) == (uint)data.UType.MainEmploye));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Area, G.Area));

            //SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Object, G.Object));
            SPED.AddControl(new SelectObj_Class(SPED, PType));

            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BckGnd));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.BackGrd, G.BackGrd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.UsBckGnd));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.SGroup));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.SGNum));

            SPED.AddControl(PType);
            //SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType, PTypeEdit));

            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PaPoS, CanEdit));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDS, false, CanEdit));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDE, true));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Podr, CanEdit));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.IMLst));

            SPED.SetChecks(C.SPoint.Checks);

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
            {
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.SMS
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.TestCond
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }
            else
            {
                SPED.AddSubEdit(G.SMS
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(G.TestCond
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }

            if (SPED.ShowDialog() == DialogResult.OK)
            { this.SPoint_Grid.InvalidateRow(this.SPoint_Grid.CurrentCell.RowIndex); }            
        }
        private static bool LoadObj(uint IDObj)
        {
            return T.Object.Rows.Get<string>(IDObj, C.Object.Name) == "Осадок сточных вод";
        }
        public class SelectObj_Class : Edit_Form.RelationCombo_class
        {
            public SelectObj_Class(Edit_Form NCTEF, Edit_Form.RelationCombo_class PType) : base(NCTEF, C.SPoint.Object)
            {
                combo.SelectedIndexChanged += (sender, e) =>
                {
                    PType.Enabled = LoadObj(SubTable.GetID(combo.SelectedIndex));
                };
            }
        }

        private void DeleteSPoint_Strip_Click(object sender, EventArgs e)   //Указать дату окончания действия точки отбора
        {
            if (SPoint_Grid.CurrentRow != null && MessageBox.Show(this, "Вы уверены, что хотите удалить точку отбора и все её замеры?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                SPoints.DeleteSPoint(SPoint_Grid.CurrentRow.Index);
                SPoint_Grid.RowCount--;
                SPoint_Grid.Invalidate();
            }
        }

        private void DeleteSample_Strip_Click(object sender, EventArgs e)   //Удалить
        {
            if (SPoint_Grid.CurrentCell != null && Sample_Grid.SelectedRows.Count > 0 && MessageBox.Show(this, "Вы уверены, что хотите удалить замер?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                var Indexes = new int[Sample_Grid.SelectedRows.Count];

                for (int i = 0; i < Sample_Grid.SelectedRows.Count; i++)
                { Indexes[i] = Sample_Grid.SelectedRows[i].Index; }

                Sample_Grid.RowCount -= Indexes.Length;
                SPoints[SPoint_Grid.CurrentCell.RowIndex].DeleteSample(Indexes);
                Sample_Grid.Invalidate();
            }
        }

        private void AddSample_Strip_Click(object sender, EventArgs e)
        {
            if (this.SPoint_Grid.CurrentCell == null || !AddSample_Strip.Enabled || Sample_Grid.IsCurrentCellInEditMode || SPoints.YM != WorkYM)
            { return; }

            var SED = new Edit_Form(SPoints.Sample, 750, false);

            var DT = ATMisc.GetYMDFromDateTime(DateTime.Now);
            if (DT > SPoints.EndDay)
            { DT = SPoints.EndDay - 1; }
            else if (DT < SPoints.StartDay)
            { DT = SPoints.StartDay + 1; }

            var cpSpoint = SPoints[this.SPoint_Grid.CurrentCell.RowIndex];

            int Number;

            if (data.User<uint>(C.User.UType) == (uint)data.UType.Union)
            {
                Number = (int)G.Sample.QUERRY()
                      .COUNT.WHERE
                      .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                      .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                      .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                      .AND.AC(C.Sample.Number).More.BV(0).DO()[0].Value + 1;

                while ((bool)SPoints.Sample.QUERRY().EXIST.WHERE
                      .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                      .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                      .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                    .AND.C(C.Sample.Number, Number).DO()[0].Value)
                { Number++; }
            }
            else
            {
                Number = (int)G.Sample.QUERRY()
                    .COUNT.WHERE
                    .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                    .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(T.SPoint.Rows.Get_UnShow<uint>(SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, C.SPoint.Podr))
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                    .AND.AC(C.Sample.Number).More.BV(0).DO()[0].Value + 1;

                while ((bool)SPoints.Sample.QUERRY().EXIST.WHERE
                    .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                    .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(T.SPoint.Rows.Get_UnShow<uint>(SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, C.SPoint.Podr))
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                    .AND.C(C.Sample.Number, Number).DO()[0].Value)
                { Number++; }
            }

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Union)
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.C(C.PodrPpl.Podr, data.User<uint>(C.User.Podr)).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV(data.User<uint>(C.User.Podr))
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                        .DO();
            }
            else
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.AC(C.PodrPpl.Podr).EQUI.BV<uint>(cpSpoint.PodrID).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV<uint>(cpSpoint.PodrID)
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                    .DO();
            }

            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.SPoint, false, SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Loc, false, SPoints.Sample.Rows.Count + 1));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Number, true, Number));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.CYMD, false, true, DT, SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.AYMD, false, true, DT, SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.Resp, G.Resp, false));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.SCause));

            SED.SetChecks(C.Sample.Checks);

            {
                var TCSSubEdit = new C.Sample.TCSSubEdit_class(SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, true, ((data.UType)data.User<uint>(C.User.UType) == data.UType.Admin) ? AutoForm.ShowType.Admin : AutoForm.ShowType.User);
                if (TCSSubEdit.Count > 0)
                { SED.AddSubEdit(TCSSubEdit); }
            }

            if (SED.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Sample_Grid.RowCount++;

                for (int i = SampleC.LastColumn + 1; i < Sample_Grid.Columns.Count; i++)
                {
                    if (Sample_Grid.Columns[i].Visible)
                    {
                        Sample_Grid.CurrentCell = Sample_Grid[i, Sample_Grid.RowCount - 1];
                        break;
                    }
                }
            }

            Sample_Grid.Focus();
        }

        private void SFT_Strip_Click(object sender, EventArgs e)
        {
            uint OID = 0;

            if (SPoint_Grid.CurrentCell != null)
            { OID = SPoints[SPoint_Grid.CurrentCell.RowIndex].ObjectID; }

            G.Object.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog(OID);
        }

        private void EditSample_Strip_Click(object sender, EventArgs e)
        {
            if (Sample_Grid.CurrentCell == null || Sample_Grid.IsCurrentCellInEditMode || WorkYM != SPoints.YM || !EditSample_Strip.Enabled)
            { return; }

            var cpSample = SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex];
            var cpSPoint = SPoints[SPoint_Grid.CurrentCell.RowIndex];

            var SED = new Edit_Form(SPoints.Sample, cpSample.SampleID, 750, false);

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Union)
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.C(C.PodrPpl.Podr, data.User<uint>(C.User.Podr)).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV(data.User<uint>(C.User.Podr))
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                        .DO();
            }
            else
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.AC(C.PodrPpl.Podr).EQUI.BV<uint>(cpSPoint.PodrID).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV<uint>(cpSPoint.PodrID)
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                    .DO();
            }

            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.SPoint, false, SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Loc, false));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Number, true));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.CYMD, false, true, cpSample.CYMD, SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.AYMD, false, true, cpSample.AYMD, SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.Resp, G.Resp, false));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.SCause));

            SED.SetChecks(C.Sample.Checks);

            {
                var TCSSubEdit = new C.Sample.TCSSubEdit_class(SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, StandartEnabled, ((data.UType)data.User<uint>(C.User.UType) == data.UType.Admin) ? AutoForm.ShowType.Admin : AutoForm.ShowType.User);
                if (TCSSubEdit.Count > 0)
                { SED.AddSubEdit(TCSSubEdit); }
            }

            if (SED.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { Sample_Grid.InvalidateRow(Sample_Grid.CurrentCell.RowIndex); }

            Sample_Grid.Focus();
        }

        private void SCauseEdit_Strip_Click(object sender, EventArgs e)
        {
            G.SCause.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void SPoint_Grid_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        bool CanDo = true;
        private void SPoint_Grid_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;

                for (int i = 0; i < SPoint_Grid.RowCount; i++)
                    SPoint_Grid.Rows[i].Height = e.Row.Height;

                SPoint_Grid.RowTemplate.Height = e.Row.Height;

                CanDo = true;
            }
        }

        private void SPoint_Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Sample_Grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (CanDo && e.Column.Index > SampleC.LastColumn)
            {
                CanDo = false;

                for (int i = SampleC.LastColumn + 1; i < Sample_Grid.Columns.Count; i++)
                { Sample_Grid.Columns[i].Width = e.Column.Width; }

                CanDo = true;
            }
        }

        private void SPoint_Grid_SelectionChanged(object sender, EventArgs e)
        {
            Sample_Grid.RowCount = 0;

            if (SPoint_Grid.SelectedRows.Count == 1)
            {
                DownSample_Strip.Enabled = UpSample_Strip.Enabled = AddSample_Strip.Enabled = CopySample_Strip.Enabled
                    = (data.User<uint>(C.User.UType) == (uint)data.UType.Employe ||
                    (SPoints[SPoint_Grid.SelectedRows[0].Index].Union && data.User<uint>(C.User.UType) == (uint)data.UType.Union))
                    && SPoints.YM == WorkYM;

                int StartColumn = SampleC.LastColumn;

                for (int i = 0; i < SPoints.MarkCount; i++)
                { Sample_Grid.Columns[StartColumn + 1 + i].Visible = SPoints[SPoint_Grid.SelectedRows[0].Index].Marks[i].AllowToUse; }

                Sample_Grid.RowCount = SPoints[SPoint_Grid.SelectedRows[0].Index].SampleCount;
                Sample_Grid.Invalidate();
            }
            else
            {
                DownSample_Strip.Enabled = UpSample_Strip.Enabled = CopySample_Strip.Enabled = AddSample_Strip.Enabled = false;
            }
        }

        private void ChangePeriod_Strip_Click(object sender, EventArgs e)
        {

        }

        private void SPoint_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditSPoint_Strip_Click(null, null);
        }

        private void Sample_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditSample_Strip_Click(null, null);
        }

        private void Sample_Grid_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;

                for (int i = 0; i < Sample_Grid.RowCount; i++)
                    Sample_Grid.Rows[i].Height = e.Row.Height;

                Sample_Grid.RowTemplate.Height = e.Row.Height;

                CanDo = true;
            }
        }

        private void Area_Strip_Click(object sender, EventArgs e)
        {
            G.Area.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        uint SPID = 0;
        void SaveSPID()
        {
            if (SPoint_Grid.CurrentCell != null)
            {
                SPID = SPoints[SPoint_Grid.CurrentCell.RowIndex].SPointID;
                SPoint_Grid.CurrentCell = null;
            }
            else
            { SPID = 0; }
        }

        void SetSPID()
        {
            if (SPoints.SPointsCount > 0)
            {
                if (SPID > 0)
                {
                    for (int i = 0; i < SPoints.SPointsCount; i++)
                    {
                        if (SPoints[i].SPointID == SPID)
                        {
                            SPoint_Grid.CurrentCell = SPoint_Grid[0, i];
                            return;
                        }
                    }
                }

                SPoint_Grid.CurrentCell = SPoint_Grid[0, 0];
            }
        }

        void GetSPonts_Changed(object sender, EventArgs e)
        {
            if (!CanDo)
            { return; }

            SaveSPID();

            uint PodrID;
            if (Podr_combo.SelectedIndex > 0)
            { PodrID = Podrs[Podr_combo.SelectedIndex - 1]; }
            else if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Employe)
            { PodrID = data.User<uint>(C.User.Podr); }
            else
            { PodrID = 0; }

            int Year = Convert.ToInt32(Year_Box.Text),
                Month = Convert.ToInt32(Month_Box.Text);

            int YM = ATMisc.GetYMFromYearMonth(Year, Month); 

            RCache.PSG.Reload(YM);
            RCache.Volumes.Reload(YM);

            SPoints.UpdateSPoints(YM, PodrID, ShowClosed_check.Checked, ShowUnion_check.Checked);
            CreateSampleColumns();
            Sample_Grid.ReadOnly = WorkYM != SPoints.YM;

            CopySPoint_Strip.Enabled = AddSPoint_Strip.Enabled = (data.UType)data.User<uint>(C.User.UType) != data.UType.MainEmploye;

            AddSample_Strip.Enabled = WorkYM == SPoints.YM && (data.UType)data.User<uint>(C.User.UType) != data.UType.MainEmploye;
            UpSample_Strip.Enabled = DownSample_Strip.Enabled = CopySample_Strip.Enabled = EditSample_Strip.Enabled = DeleteSample_Strip.Enabled = WorkYM == SPoints.YM;
            

            SPoint_Grid.RowCount = SPoints.SPointsCount;
            SPoint_Grid.Invalidate();

            SetSPID();
        }

        private void Volume_Strip_Click(object sender, EventArgs e)
        {
            new VolumeEdit_Form().ShowDialog();
            RCache.Volumes.Reload(ATMisc.GetYMFromYearMonth(Convert.ToInt32(Year_Box.Text), Convert.ToInt32(Month_Box.Text)));
        }

        private void ShowPeriod_button_Click(object sender, EventArgs e)
        {
            int Year = Convert.ToInt32(Year_Box.Text),
                Month = Convert.ToInt32(Month_Box.Text);

            CanDo = false;
            RCache.PSG.Reload(ATMisc.GetYMFromYearMonth(Year, Month));
            RCache.Volumes.Reload(ATMisc.GetYMFromYearMonth(Year, Month));
            SPoints.YM = ATMisc.GetYMFromYearMonth(Year, Month);
            CanDo = true;
        }

        private void Year_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic(sender as TextBox);
        }

        private void SetPeriod_button_Click(object sender, EventArgs e)
        {
            Year_Box.Text = CurrentYear_Box.Text;
            Month_Box.Text = CurrentMonth_Box.Text;

            GetSPonts_Changed(null, null);
        }

        void NextMonth(string EndMonth, string StartMonth, short Increment)
        {
            if (Month_Box.Text == Year_Box.Text && Month_Box.Text == "1" && Increment < 0)
            { return; }

            if (Month_Box.Text == EndMonth)
            {
                SetNext(Year_Box, Increment);
                Month_Box.Text = StartMonth;
            }
            else
            { SetNext(Month_Box, Increment); }

            int NewMYM = ATMisc.GetYMFromYearMonth(Convert.ToInt32(Year_Box.Text), Convert.ToInt32(Month_Box.Text));

            if (NewMYM != SPoints.YM)
            { GetSPonts_Changed(null, null); }
        }

        private void NextMonth_button_Click(object sender, EventArgs e)
        {
            NextMonth("12", "1", 1);
        }

        private void PreviousMonth_button_Click(object sender, EventArgs e)
        {
            NextMonth("1", "12", -1);
        }

        private void Month_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic(sender as TextBox);
            int Slovo = Convert.ToInt32((sender as TextBox).Text);

            if (Slovo > 12)
            { (sender as TextBox).Text = "12"; }
            else if (Slovo < 1)
            { (sender as TextBox).Text = "1"; }
        }

        private void OType_Strip_Click(object sender, EventArgs e)
        {
            G.OType.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void OLocation_Strip_Click(object sender, EventArgs e)
        {
            G.OLocation.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
            RCache.Volumes.Reload(SPoints.YM);
        }

        private void MMarks_Strip_Click(object sender, EventArgs e)
        {
            new MiddleMarks_Form().ShowDialog();
        }
        #region Если верить, то это обновление БДшки LaboratoryOnlineJournal нужно найти сам путь к БДшке и как он записывает даннные
        private void UpdateBD_Strip_Click(object sender, EventArgs e)
        {
            if (data.SynchPool == null)
            { MessageBox.Show(this, "Обновление базы данных не возможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            {
                var WorkYM = Employe_Form.WorkYM;

                new DB_Form().ShowDialog();

                if (WorkYM != Employe_Form.WorkYM)
                { Employe_Form.WorkYM = WorkYM; }

                TimerUpdate();

                GetSPonts_Changed(null, null);
            }
        }
        #endregion
        #region Как я понял, эта тема отвечает за протоколы, над РАЗОБРАТЬСЯ
        private void Protoks_Strip_Click(object sender, EventArgs e)
        {
            new Protok_Form(Employe_Form.SPoints.YM,
                (data.UType)data.User<uint>(C.User.UType) == data.UType.Employe ? data.User<uint>(C.User.Podr) : 
                0, 
                (data.UType)data.User<uint>(C.User.UType) == data.UType.Union, 
                Employe_Form.WorkYM == Employe_Form.SPoints.YM,
                (data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye || (data.UType)data.User<uint>(C.User.UType) == data.UType.Admin || (data.UType)data.User<uint>(C.User.UType) == data.UType.Union).ShowDialog();
        }
        #endregion 


        private void PType_Strip_Click(object sender, EventArgs e)
        {
            G.PType.GetAutoForm((data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void PSGM_Strip_Click(object sender, EventArgs e)
        {
            new PSGMethods_Form().ShowDialog();
        }

        private void MassOutgo_Strip_Click(object sender, EventArgs e)
        {
            if (data.DataSourceType != DataBase.RemoteType.MySQL)
            {
                for (int i = 0; i < SPoints.SPointsCount; i++)
                {
                    if (SPoints[i].BackGrdID > 0 && SPoints[i].UseBackGrd)
                    {
                        MessageBox.Show(this, "Система не подключена к удаленному истонику данных", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            uint PodrID, OTypeID;
            {
                if (Podr_combo.SelectedIndex > 1)
                {
                    OTypeID = PodrID = 0;

                    for (int i = 0; i < SPoints.SPointsCount; i++)
                    {
                        if (SPoints[i].Volumed)
                        {
                            var SPointID = SPoints[i].SPointID;
                            PodrID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Podr);
                            OTypeID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object, C.Object.OType);
                            break;
                        }
                    }
                }
                else
                {
                    var SPointID = (SPoint_Grid.CurrentCell == null ? 0 : SPoints[SPoint_Grid.CurrentCell.RowIndex].SPointID);
                    PodrID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Podr);
                    OTypeID = T.SPoint.Rows.Get_UnShow<uint>(SPointID, C.SPoint.Object, C.Object.OType);
                }
            }

            new MassOutgo_Form(PodrID, OTypeID).ShowDialog();
        }

        private void ChangePeriod_Strip_Click_1(object sender, EventArgs e)
        {
            new PeriodChange_Form().ShowDialog();

            int Year, Month;

            ATMisc.GetYearMonthFromYM(data.User<int>(C.User.YM), out Year, out Month);

            CurrentMonth_Box.Text = Month.ToString();
            CurrentYear_Box.Text = Year.ToString();

            SetPeriod_button_Click(null, null);
        }

        private void BackGrd_Strip_Click(object sender, EventArgs e)
        {
            G.BackGrd.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void UpdateDB_timer_Tick(object sender, EventArgs e)
        {
            if (!MailesCheck.AlowToConnect)
            { UpdateDB_timer.Enabled = false; }
            else if (!MailCheck_Thread.IsAlive)
            {
                UpdateDB_timer.Enabled = false;

                if (Files.Count > 0)
                {
                    if (MessageBox.Show(this, "Обнаружены обновления базы данных, загрузить их сейчас ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    { new LoadDBUpdates_Form(Files.ToArray()).ShowDialog(); }
                    else
                    {
                        var Path = Application.StartupPath + "\\принятые\\" + ATMisc.GetMonthName1(DateTime.Now.Month) + "\\" + DateTime.Now.Day.ToString() + "\\";

                        if (MessageBox.Show(this, "Файлы обновлений сохранены в папке: \"" + Path + "\", открыть папку ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        { System.Diagnostics.Process.Start(Path); }
                    }

                    Files.Clear();
                }

                UpdateDB_timer.Enabled = true;

                MailCheck_Thread = new Thread(MailesCheck.AlterStart);
                MailCheck_Thread.Start();
            }
        }

        private void PaPoSEdit_Strip_Click(object sender, EventArgs e)
        {
            G.PaPoS.GetAutoForm(this, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, AutoForm.ShowType.User).ShowDialog();
        }

        private void CopySample_Strip_Click(object sender, EventArgs e)
        {
            if (SPoint_Grid.CurrentCell == null || Sample_Grid.CurrentCell == null || WorkYM != SPoints.YM || !Sample_Grid.Enabled)
            { return; }

            var SRI = Sample_Grid.CurrentCell.RowIndex;
            var SED = new Edit_Form(SPoints.Sample, 750, false);
            var SID = SPoints[SPoint_Grid.CurrentCell.RowIndex][SRI].SampleID;

            var cpSPoint = SPoints[SPoint_Grid.CurrentCell.RowIndex];

            int Number;

            if (data.User<uint>(C.User.UType) == (uint)data.UType.Union)
            {
                Number = (int)G.Sample.QUERRY()
                      .COUNT.WHERE
                      .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                      .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                      .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                      .AND.AC(C.Sample.Number).More.BV(0).DO()[0].Value + 1;

                while ((bool)SPoints.Sample.QUERRY().EXIST.WHERE
                      .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                      .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                      .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(true)
                    .AND.C(C.Sample.Number, Number).DO()[0].Value)
                { Number++; }
            }
            else
            {
                Number = (int)G.Sample.QUERRY()
                    .COUNT.WHERE
                    .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                    .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(cpSPoint.PodrID)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                    .AND.AC(C.Sample.Number).More.BV(0).DO()[0].Value + 1;

                while ((bool)SPoints.Sample.QUERRY().EXIST.WHERE
                    .AC(C.Sample.CYMD).Less.BV(SPoints.EndDay)
                    .AND.AC(C.Sample.CYMD).More.BV(SPoints.StartDay)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(cpSPoint.PodrID)
                    .AND.ARC(C.Sample.SPoint, C.SPoint.Union).EQUI.BV(false)
                    .AND.C(C.Sample.Number, Number).DO()[0].Value)
                { Number++; }
            } 
            
            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Union)
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.C(C.PodrPpl.Podr, data.User<uint>(C.User.Podr)).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV(data.User<uint>(C.User.Podr))
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                        .DO();
            }
            else
            {
                G.PodrPpl.QUERRY().SHOW.WHERE.AC(C.PodrPpl.Podr).EQUI.BV<uint>(cpSPoint.PodrID).DO();
                G.Resp.QUERRY()
                    .SHOW.WHERE
                        .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV<uint>(cpSPoint.PodrID)
                        .AND.C(C.Resp.TResp, (uint)data.TResp.Sampler)
                    .DO();
            }

            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.SPoint, false, SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Loc, false, SPoints.Sample.Rows.Count + 1));
            SED.AddControl(new Edit_Form.Inputs_class(SED, C.Sample.Number, true, Number));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.CYMD, false, true, T.Sample.Rows.Get<int>(SID, C.Sample.CYMD), SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.DaysInt32_class(SED, C.Sample.AYMD, false, true, T.Sample.Rows.Get<int>(SID, C.Sample.AYMD), SPoints.StartDay + 1, SPoints.EndDay - 1));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.Resp, true, T.Sample.Rows.Get_UnShow<uint>(SID, C.Sample.Resp), G.Resp, false));
            SED.AddControl(new Edit_Form.RelationCombo_class(SED, C.Sample.SCause, true, T.Sample.Rows.Get_UnShow<uint>(SID, C.Sample.SCause), G.SCause));

            SED.SetChecks(C.Sample.Checks);

            {
                var TCSSubEdit = new C.Sample.TCSSubEdit_class(SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID, true, ((data.UType)data.User<uint>(C.User.UType) == data.UType.Admin) ? AutoForm.ShowType.Admin : AutoForm.ShowType.User);
                if (TCSSubEdit.Count > 0)
                {
                    TCSSubEdit.SetSID(SID);
                    SED.AddSubEdit(TCSSubEdit);
                }
            }

            if (SED.ShowDialog() == DialogResult.OK)
            {
                var sample = SPoints[SPoint_Grid.CurrentCell.RowIndex][SRI];

                Sample_Grid.RowCount++;
                Sample_Grid.CurrentCell = Sample_Grid[Sample_Grid.CurrentCell.ColumnIndex, Sample_Grid.RowCount - 1];

                for (int i = 0; i < sample.Marks.Length; i++)
                {
                    if (sample.Marks[i].Amount != 0)
                    { SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[i].Amount = sample.Marks[i].Amount; }
                }

                Sample_Grid.Invalidate();
            }

            Sample_Grid.Focus();
        }

        private void CopySPoint_Strip_Click(object sender, EventArgs e)
        {
            if (SPoint_Grid.CurrentCell == null || !CopySPoint_Strip.Enabled)
            { return; }

            var SPED = new Edit_Form(SPoints.SPoint, 750, false);

            var SPID = SPoints[this.SPoint_Grid.CurrentCell.RowIndex].SPointID;
            var SPRI = this.SPoint_Grid.CurrentCell.RowIndex;

            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Number, true, T.SPoint.Rows.Get<int>(SPID, C.SPoint.Number)));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Name, true, T.SPoint.Rows.Get<string>(SPID, C.SPoint.Name)));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Union, false, data.User<uint>(C.User.UType) == (uint)data.UType.Union));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Area, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.Area), G.Area));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.Object, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.Object), G.Object));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.BckGnd, true, T.SPoint.Rows.Get_UnShow<bool>(SPID, C.SPoint.BckGnd)));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.BackGrd, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.BackGrd), G.BackGrd));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.UsBckGnd, true, T.SPoint.Rows.Get_UnShow<bool>(SPID, C.SPoint.UsBckGnd)));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.SGroup, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.SGroup), G.SGroup));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.SGNum, true, T.SPoint.Rows.Get_UnShow<byte>(SPID, C.SPoint.SGNum)));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PType, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.PType), G.PType));
            SPED.AddControl(new Edit_Form.RelationCombo_class(SPED, C.SPoint.PaPoS, true, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.PaPoS), G.PaPoS));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDS, false, true, T.SPoint.Rows.Get_UnShow<int>(SPID, C.SPoint.YMDS)));
            SPED.AddControl(new Edit_Form.DaysInt32_class(SPED, C.SPoint.YMDE, true, true, T.SPoint.Rows.Get_UnShow<int>(SPID, C.SPoint.YMDE)));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.Podr, (data.UType)data.User<uint>(C.User.UType) != data.UType.Employe, T.SPoint.Rows.Get_UnShow<uint>(SPID, C.SPoint.Podr)));
            SPED.AddControl(new Edit_Form.Inputs_class(SPED, C.SPoint.IMLst, true, T.SPoint.Rows.Get_UnShow<bool>(SPID, C.SPoint.IMLst)));

            SPED.SetChecks(C.SPoint.Checks);

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
            {
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.SMS
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(false, AutoForm.ShowType.User, G.TestCond
                       , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }
            else
            {
                SPED.AddSubEdit(G.SMS
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Number)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.EdType)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.SMS.Mark, C.Mark.OPType)
                    });
                SPED.AddSubEdit(G.TestCond
                      , new Edit_Form.SubEdit_class.Columns_struct[] 
                    {
                        new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.Name)
                      , new Edit_Form.SubEdit_class.Columns_struct(C.TestCond.EdType)
                    });
            }

            for (int i = 0; i < SPoints[SPRI].Marks.Length; i++)
            {
                if (SPoints[SPRI].Marks[i].AllowToUse)
                {
                    var Values = new object[T.SMS.Columns.Count];

                    Values[C.SMS.SPoint] = (uint)0;
                    Values[C.SMS.Mark] = RCache.Marks[i].ID;

                    SPED.components_Panel[0].AddRow(Values);
                }
            }

            {
                var TestCond = T.TestCond.CreateSubTable(false);
                TestCond.QUERRY().SHOW.WHERE.C(C.TestCond.SPoint, SPoints[SPRI].SPointID).DO();

                for (int i = 0; i < TestCond.Rows.Count; i++)
                {
                    var Values = new object[T.TestCond.Columns.Count];

                    Values[C.TestCond.SPoint] = (uint)0;
                    Values[C.TestCond.EdType] = TestCond.Rows.Get_UnShow<uint>(i, C.TestCond.EdType);
                    Values[C.TestCond.Name] = TestCond.Rows.Get_UnShow<string>(i, C.TestCond.Name);

                    SPED.components_Panel[1].AddRow(Values);
                }
            }

            if (SPED.ShowDialog() == DialogResult.OK)
            {
                SPoint_Grid.RowCount++;
                SPoint_Grid.CurrentCell = SPoint_Grid[SPoint_Grid.CurrentCell.ColumnIndex, SPoint_Grid.RowCount - 1];
            }
        }

        private void Up_Strip_Click(object sender, EventArgs e)
        {
            if (!UpSample_Strip.Enabled || SPoint_Grid.CurrentCell == null || Sample_Grid.CurrentCell == null)
            { return; }

            if (Sample_Grid.CurrentCell.RowIndex - 1 > -1)
            {
                SPoints[SPoint_Grid.CurrentCell.RowIndex].Move(Sample_Grid.CurrentCell.RowIndex, -1);
                Sample_Grid.CurrentCell = Sample_Grid[Sample_Grid.CurrentCell.ColumnIndex, Sample_Grid.CurrentCell.RowIndex - 1];
                Sample_Grid.Invalidate();
            }
        }

        private void Down_Strip_Click(object sender, EventArgs e)
        {
            if (!DownSample_Strip.Enabled || SPoint_Grid.CurrentCell == null || Sample_Grid.CurrentCell == null)
            { return; }

            if (Sample_Grid.CurrentCell.RowIndex + 1 < SPoints[SPoint_Grid.CurrentCell.RowIndex].SampleCount)
            {
                SPoints[SPoint_Grid.CurrentCell.RowIndex].Move(Sample_Grid.CurrentCell.RowIndex, 1);
                Sample_Grid.CurrentCell = Sample_Grid[Sample_Grid.CurrentCell.ColumnIndex, Sample_Grid.CurrentCell.RowIndex + 1];
                Sample_Grid.Invalidate();
            }
        }

        private void Sample_Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (SPoint_Grid.CurrentCell == null || Sample_Grid.CurrentCell == null || SPoints.YM != WorkYM)
            { return; }

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye && !SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].ExistSM)
            { return; }

            switch (e.ColumnIndex)
            {
                /*case SampleC.Number:
                    if (e.Value != null && e.Value is string)
                    {
                        int Volume;

                        if (int.TryParse((string)e.Value, out Volume))
                        { SPoints[SPoint_Grid.CurrentCell.RowIndex].Number = Volume; }
                    }
                    break;*/
                default:
                    if (e.ColumnIndex > SampleC.LastColumn)
                    {
                        if (e.Value != null)
                        {
                            if (e.Value is string)
                            {
                                var Value = ((string)e.Value).Replace('е', 'e').Replace('.',',');

                                double Volume;

                                if (double.TryParse(Value, out Volume))
                                { SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount = Volume; }
                            }
                            else if (e.Value is bool)
                            { SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount = ((bool)e.Value ? 1 : 0); }
                        }
                    }
                    break;
            }
        }

        private void Sample_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (SPoint_Grid.CurrentCell == null || e.RowIndex < 0 || e.RowIndex >= SPoints[SPoint_Grid.CurrentCell.RowIndex].SampleCount)
            { return; }

            switch (e.ColumnIndex)
            {
                case SampleC.SCause:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].SCause;
                    break;
                case SampleC.ID:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].SampleID;
                    break;
                case SampleC.Loc:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Loc;
                    break;
                case SampleC.Number:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Number;
                    break;
                case SampleC.AYMD:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].ADate.ToShortDateString();
                    break;
                case SampleC.CYMD:
                    e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].CDate.ToShortDateString();
                    break;
                default:
                    if (e.ColumnIndex > -1)
                    {
                        switch (SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].VarType)
                        {
                            case data.VarType.Bool:
                                e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount > 0;
                                break;
                            case data.VarType.dbl:
                                e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount;
                                break;
                            case data.VarType.i32:
                                e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount;
                                break;
                        }
                        e.Value = SPoints[SPoint_Grid.CurrentCell.RowIndex][e.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].Amount;
                    }
                    break;
            }
        }

        private void SPoint_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                switch (e.ColumnIndex)
                {
                    case SPointC.ID:
                        e.Value = SPoints[e.RowIndex].SPointID;
                        break;
                    case SPointC.Name:
                        e.Value = SPoints[e.RowIndex].Name;
                        break;
                    case SPointC.Number:
                        e.Value = e.RowIndex + 1;
                        break;
                    case SPointC.Object:
                        e.Value = SPoints[e.RowIndex].Object;
                        break;
                    case SPointC.Podr:
                        e.Value = SPoints[e.RowIndex].PodrShortName;
                        break;
                    case SPointC.Volume:
                        if (SPoints[e.RowIndex].Volumed)
                        { e.Value = SPoints[e.RowIndex].OLocation; }
                        break;
                }
            }
        }

        private void Sample_Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = e.ColumnIndex < SampleC.LastColumn + 1
                    || SPoint_Grid.CurrentCell == null || Sample_Grid.CurrentCell == null ||
                       (data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye
                    && !SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[e.ColumnIndex - SampleC.LastColumn - 1].ExistSM;
        }

        private void SPoint_Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (SPoints[e.RowIndex].ItsOK)
                { SPoint_Grid[SPointC.Name, e.RowIndex].Style.BackColor = Color.White; }
                else
                { SPoint_Grid[SPointC.Name, e.RowIndex].Style.BackColor = Color.PaleVioletRed; }
            }
        }

        private void Sample_Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (WorkYM == SPoints.YM &&
                SPoint_Grid.CurrentCell != null &&
                Sample_Grid.CurrentCell != null &&
                e.KeyData == Keys.Enter)
            {
                switch (Sample_Grid.CurrentCell.ColumnIndex)
                {
                    default:
                        if (Sample_Grid.CurrentCell.ColumnIndex > SampleC.LastColumn &&
                            SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[Sample_Grid.CurrentCell.ColumnIndex - SampleC.LastColumn - 1].VarType == data.VarType.Bool &&
                            ((data.UType)data.User<uint>(C.User.UType) != data.UType.MainEmploye
                            || SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[Sample_Grid.CurrentCell.ColumnIndex - SampleC.LastColumn - 1].SMID > 0))
                        {
                            SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[Sample_Grid.CurrentCell.ColumnIndex - SampleC.LastColumn - 1].Amount =
                                (SPoints[SPoint_Grid.CurrentCell.RowIndex][Sample_Grid.CurrentCell.RowIndex].Marks[Sample_Grid.CurrentCell.ColumnIndex - SampleC.LastColumn - 1].Amount > 0 ? 0 : 1);
                            
                            Sample_Grid.InvalidateCell(Sample_Grid[Sample_Grid.CurrentCell.ColumnIndex, Sample_Grid.CurrentCell.RowIndex]);
                        }
                        break;
                }
            }
        }

        private void DownSPoint_Strip_Click(object sender, EventArgs e)
        {
            if (SPoint_Grid.CurrentCell != null)
            {
                if (SPoint_Grid.CurrentCell.RowIndex + 1 < SPoints.SPointsCount)
                {
                    SPoints.MoveSPoint(SPoint_Grid.CurrentCell.RowIndex, 1);
                    SPoint_Grid.CurrentCell = SPoint_Grid[SPoint_Grid.CurrentCell.ColumnIndex, SPoint_Grid.CurrentCell.RowIndex + 1];
                    SPoint_Grid.Invalidate();
                }
            }
        }

        private void UpSPoint_Strip_Click(object sender, EventArgs e)
        {
            if (SPoint_Grid.CurrentCell != null)
            {
                if (SPoint_Grid.CurrentCell.RowIndex - 1 > -1)
                {
                    SPoints.MoveSPoint(SPoint_Grid.CurrentCell.RowIndex, -1);
                    SPoint_Grid.CurrentCell = SPoint_Grid[SPoint_Grid.CurrentCell.ColumnIndex, SPoint_Grid.CurrentCell.RowIndex - 1];
                    SPoint_Grid.Invalidate();
                }
            }
        }
    }
}
