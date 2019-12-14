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

using LaboratoryOnlineJournal;

/// <summary>форма-лого, чтоб показать пользователю, что прога не ничего не делает</summary>
public partial class StartupLogo_Form : Form
{
    Loading_class Loading;
    byte PCount = 0;

    public class Loading_class
    {
        public Loading_class(DataBase db, Loading_delegate action)
        {
            thread = new Thread(() => { action(db, this); });
            thread.Start();
        }
        public delegate void Loading_delegate(DataBase db, Loading_class LC);

        public string LoadingComment;

        Thread thread;

        public bool Ready { get { return !thread.IsAlive; } }

        public void Abort() { thread.Abort(); }

        public override string ToString()
        {
            if (!Ready)
            { return "Ready=" + Ready.ToString() + ": " + LoadingComment; }
            else
            { return "Ready=" + Ready.ToString(); }
        }
    }

    public StartupLogo_Form(DataBase db, Loading_class.Loading_delegate action)
    {
        InitializeComponent();

        Loading = new Loading_class(db, action);
    }

    const byte MaxCount = 4;
    private void timer1_Tick(object sender, EventArgs e)
    {
        if (PCount == MaxCount)
        { PCount = 0; }

        var temp = ".";
        for (int i = 0; i < PCount; i++)
        { temp += "."; }
        for (int i = PCount; i < MaxCount; i++)
        { temp += ' '; }

        PCount++;

        label1.Text = "Загрузка:" + Loading.LoadingComment + temp;    //отображаю плацебо-загрузку

        if (Loading.Ready)
        {
            this.FormClosing -= StartupLogo_Form_FormClosing;
            timer1.Enabled = false;

            if (data.T1.DataSourceEnabled || data.T1.type == DataBase.RemoteType.Local)
            { DialogResult = System.Windows.Forms.DialogResult.OK; }
            else
            { DialogResult = System.Windows.Forms.DialogResult.No; }
        }
    }

    private void StartupLogo_Form_FormClosing(object sender, FormClosingEventArgs e)
    {
        timer1.Enabled = false;
        Loading.Abort();
        DialogResult = System.Windows.Forms.DialogResult.No;
    }
}