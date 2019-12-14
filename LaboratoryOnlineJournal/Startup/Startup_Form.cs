using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using LaboratoryOnlineJournal;

/* 
 * Общая схема запуска программы:
 * 
 * *********************************************************************************************
 * *                 Событие                            *    Модуль      *    Наполнение       *
 * *********************************************************************************************
 * *  сканирую библиотеки                               *   Program      *   Общее             *
 * *  произвожу поиск и активацию параметров запуска    *   Program      *   Общее             *
 * *  гружу настройки из файла                          *   Program      *   Общее             *
 * *  запускаю автообновление                           *   Program      *   Общее             *
 * *  проверяю доступность и подключаю базу данных      *   Startup_Form *   Индивидуально     *
 * *  гружу начальный набор таблиц                      *   StartupLogo  *   Общее             *
 * *  показываю форму логина                            *   Startup_Form *   Общее             *
 * *  пользователь входит в свою учетку                 *   Startup_Form *   Общее             *
 * *  гружу остальные таблицы                           *   StartupLogo  *   Общее             *
 * *  выбираю и инициализирую конечный интерфейс        *   Misc         *   Индивидуально     *
 * *********************************************************************************************
 * 
 * в общих модулях используются разные пространства имеен, в соответсвии с названиями проектов
 */

public partial class Startup_Form : Form //форма выбора пользователя, ввода пароля etc.
{
    public Startup_Form()
    {
        //MessageBox.Show("Startup_Form()");
        InitializeComponent();

    Try_Again: ;
        var Return = "";

        data.T1 = new DataBase(data.PrgSettings.Values[(int)data.Strings.DATABASE].String, Encoding.GetEncoding(866));  //Индивидуально вот тут. Обычно Encoding.GetEncoding(1251). Т.к. организуя оффлайновую работу, я накосячил с кодировкой и теперь там в таблицах записана кодировка 1251, а сам текст кодирован в 866 и конвертатор благо не работает еще D:
        data.T1.AllowModify = data.AllowModify;

        data.T1.ErrorConnection = (bd, ex) =>
        {
            if (MessageBox.Show("При попытке подключения к источнику данных возникла ошибка:\n" + ex.Message + "\nПопробовать подключиться сново?\nВ противном случае программа закроется", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            { return true; }
            else
            {
                var S = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);

                for (int i = 0; i < S.Length; i++)
                { S[i].Kill(); }

                return false;
            }
        };

        switch (data.DataSourceType)
        {
            case DataBase.RemoteType.MFT:
                try
                {
                    if (!data.T1.UseMFT(Application.StartupPath))
                    { Return += "База данных не обнаружена, либо данные повреждены\n"; }
                }
                catch (Exception ex)
                { Return += ex.Message.ToString() + "\n"; }
                break;
            case DataBase.RemoteType.MySQL:
                Return = "";
                for (int i = (int)data.Strings.SqlIp; i < (int)data.Strings.SqlIpLast + 1; i++)  //проверяю ip-шники
                {
                    try
                    {
                        if (data.T1.UseMySql(data.PrgSettings.Values[i].String
                                           , data.PrgSettings.Values[(int)data.Strings.SqlLogin].String
                                           , data.PrgSettings.Values[(int)data.Strings.SqlPassword].String
                                           , data.PrgSettings.Values[(int)data.Strings.SqlPort].Int))
                        {
                            if (i != (int)data.Strings.SqlIp)
                            { data.PrgSettings.Values[(int)data.Strings.SqlIp].String = data.PrgSettings.Values[i].String; }

                            Return = "";
                            break;
                        }
                        else
                        { Return += i + ")Не верный логин/пароль, либо сервер не отвечает\n"; }
                    }
                    catch (Exception ex)
                    { Return += i + ")" + ex.Message.ToString() + "\n"; }
                }

                if (Return.Length > 0)
                { Return = "Варианты mysql адресов:\n" + Return; }
                break;
            case DataBase.RemoteType.Local: break;
            default:
                throw new Exception("не известный тип источника данных " + data.PrgSettings.Values[(int)data.Strings.UseSQL].String);
        }

        if (Return.Length > 0)
        {
            switch (MessageBox.Show("При подключении к источнику данных возникла ошибка:\n" + Return + "Попробовать подключиться еще раз ?\nВ случае отмены будет запущен оффлайновый режим.", "Ошибка", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
            {
                case System.Windows.Forms.DialogResult.Cancel:
                    data.PrgSettings.Values[(int)data.Strings.UseSQL].Int = (int)DataBase.RemoteType.Local;
                    data.T1.UseLocal();
                    break;
                case System.Windows.Forms.DialogResult.No:
                    this.Close();
                    return;
                case System.Windows.Forms.DialogResult.OK:
                    goto Try_Again;
            }
        }

        if (new StartupLogo_Form(data.T1, Misc.DataBaseLoadFT).ShowDialog() != System.Windows.Forms.DialogResult.OK)
        { this.Close(); }

        CanUse_timer.Interval = StandartIndervalCanUse;
    }

    Form Show;

    private void Start_button_Click(object sender, EventArgs e)
    {
        if (UserNames_combo.SelectedIndex < 0)  //если пользователь не выбран
        {
            MessageBox.Show(this, "Необходимо выбрать пользователя", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            UserNames_combo.DroppedDown = true;
            UserNames_combo.Focus();
            return;
        }

        var UserID = G.User.Rows.GetID(UserNames_combo.SelectedIndex);  //записываю ID текущего пользователя

        if (data.T1.type == DataBase.RemoteType.MySQL && !T.User.Rows.Get<bool>(UserID, C.User.Enabled))
        {
            var Cause = T.User.Rows.Get<string>(UserID, C.User.Cause);
            if (Cause.Length > 0)
            {
                MessageBox.Show(this, "Эта учетная запись заблокирована администратором, по причине:\r\n" + Cause + ".\r\n Её использование сейчас невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(this, "Эта учетная запись заблокирована администратором. Её использование сейчас невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return;
        }

        if (UserPass_Box.Text != "пуыефде")    //секретный пароль
        {
            if (T.User.Rows.Get<string>(UserID, C.User.Pass) != UserPass_Box.Text)   //стандартный пароль
            {
                MessageBox.Show(this, "Пароль неверный!\nЕсли вы забыли ваш пароль, то попробуйте сменить его. "
                                    + "Для этого нажмите левой кнопкой мышки на слова:\n\"Проблема с учетной записью ?\"\nПод полем ввода пароля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UserPass_Box.Text = "";
                UserPass_Box.Focus();
                return;
            }

            if (data.T1.type == DataBase.RemoteType.MySQL)
            {
                if (T.User.Rows.Get<bool>(UserID, C.User.IsHere))   //проверяю залогинился пользователь до этого или нет
                {
                    MessageBox.Show(this, "Эта учетная запись сейчас используется.\nЕсли работа программы была завершена не корректно в прошлый раз, то запись можно сбросить. "
                                        + "Для этого нажмите левой кнопкой мышки на слова:\n\"Проблема с учетной записью ?\"\nПод полем ввода пароля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    UserPass_Box.Focus();
                    return;
                }
                T.User.Rows.Set(UserID, C.User.IsHere, DataBase.AutoStatus.Used);
            }

            T.User.Rows.Set(UserID, C.User.PCUser, Environment.UserName);
            T.User.Rows.Set(UserID, C.User.PCName, Environment.MachineName);
        }

        this.Visible = false;
        if (new StartupLogo_Form(data.T1, Misc.DataBaseLoad).ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            data.UserID = UserID;  //записываю id текущего пользователя

            Show = Misc.SelectForm();

            if (Show != null && !Show.IsDisposed)
            {
                sText = Show.Text = data.User<string>(C.User.Login);
                CanUse_timer.Enabled = data.T1.type == DataBase.RemoteType.MySQL;

                //Проверяю наличие изменений
                if (data.PrgSettings.Values[(int)data.Strings.Changes].Int != Misc.Number && CheckDocChanges())
                {
                    MessageBox.Show(this, "Открылся документ со списком изменений в новой версии программы, пожалуйста ознакомьтесь с изменениями прежде чем начать работу.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    data.PrgSettings.Values[(int)data.Strings.Changes].Int = Misc.Number;
                }

                Show.ShowDialog();
            }
        }
        this.Close();
    }

    private void Exit_button_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    public static bool CheckDocChanges()
    {
        var FilePath = Application.StartupPath + "\\Новое в версии\\";

        if (Directory.Exists(FilePath))
        {
            string[] Files;

            if ((Files = Directory.GetFiles(FilePath, Misc.Number.ToString() + ".*")).Length == 1)
            {
                System.Diagnostics.Process.Start(Files[0]);
                return true;
            }
        }

        return false;
    }

    private void Startup_Form_Load(object sender, EventArgs e)
    {
        G.User.QUERRY().SHOW.WHERE.NOT.AC(C.User.UType).EQUI.BV((uint)0).DO();
        if (G.User.Rows.Count == 0)
        {
            G.User.QUERRY()
                .ADD
                    .C(C.User.Login, "Администратор")
                    .C(C.User.UType, (uint)data.UType.Admin)
                    .C(C.User.IsHere, DataBase.AutoStatus.UnUse)
                    .C(C.User.Enabled, true)
                .DO();
        }
        int SelectingUser = -1;
        var UserID = data.PrgSettings.Values[(int)data.Strings.LastUser].UInt;

        for (int i = 0; i < G.User.Rows.Count; i++)
        {
            if (G.User.Rows.Get_Row(i).ID == UserID)
            { SelectingUser = i; }

            UserNames_combo.Items.Add(G.User.Rows.Get<string>(i, C.User.Login) + "(" + G.User.Rows.Get<string>(i, C.User.UType) + ")");
        }
        UserNames_combo.SelectedIndex = SelectingUser;
    }

    private void UserPass_Box_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)    //если ткнул ентер, то
        {
            Start_button.Focus();

            Start_button_Click(null, null);    //делаю вид, что нажали "Вход"

            e.SuppressKeyPress = true;
        }
    }

    private void UserNames_combo_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)    //если ткнул ентер, то
        {
            UserPass_Box.Focus();    //делаю вид, что нажали "TAB"
            e.SuppressKeyPress = true;
        }
    }

    private void UserNames_combo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserPass_Box.Text = ""; //если сменил логин, то старый пароль стерается

        Mail_Label.Visible = UserNames_combo.SelectedIndex > -1;
    }

    private void Mail_Label_Click(object sender, EventArgs e)
    {
        if (UserNames_combo.SelectedIndex > -1)
        { new Mail_Form(G.User.Rows.GetID(UserNames_combo.SelectedIndex)).ShowDialog(); }
    }

    private void Startup_Form_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (data.UserID > 0)
        {
            Timer_timer.Enabled = false;
            CanUse_timer.Enabled = false;

            if (data.T1.type == DataBase.RemoteType.MySQL)
            { data.User(C.User.IsHere, DataBase.AutoStatus.UnUse); }

            data.PrgSettings.Save(); //сохраняю настройки и аривидерчи
        }

        data.T1.Dispose();
    }

    private void Version_Strip_Click(object sender, EventArgs e)
    {
        MessageBox.Show(this, Application.ProductName + "=" + Assembly.GetExecutingAssembly().GetName().Version + '\n' + DataBase.GetVersion(), "Версия", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    /*
    * тута вот механизм "выгоняния" пользователя из программы.
    * Принцип:
    * 1.Проверяю метку "выгоняния"(значение колонки C.User.Enabled текущего пользователя), если она не активирована, то вывожу сообщение. После того как сообщение выведено, пойдет отсчет - 1 минута
    * 2.По истечении минуты, если метка все еще установлена на закрытие, программа закрывается.
    */
    /// <summary>Указание на то, такой статус таймера CanUse_timer</summary>
    bool OneMinute = false;
    /// <summary>Длина периода таймера CanUse_timer в режиме сканирования значение C.User.Enabled</summary>
    const int StandartIndervalCanUse = 10 * 1000;
    /// <summary>Длина периода таймера CanUse_timer в режиме ожидания закрытия</summary>
    const int CloseIndervalCanUse = MaxCount * 1000;

    /// <summary>Кол-во секунд таймера</summary>
    const byte MaxCount = 60;
    /// <summary>Сохраненный текст заголовка окна интерфейса</summary>
    string sText = null;
    /// <summary>Текущее кол-во секунд таймера</summary>
    byte Count = 0;

    private void CanUse_timer_Tick(object sender, EventArgs e)
    {
        CanUse_timer.Enabled = false;

        if (OneMinute)
        {
            if (T.User.Rows.Get<bool>(data.UserID, C.User.Enabled))
            {
                new Thread(() =>
                {
                    Show.Invoke(new MethodInvoker(() =>
                    {
                        MessageBox.Show(Show, "Использование этой учетной записи вновь разрешено. Работу можно продолжить.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }));
                }).Start();

                Timer_timer.Enabled = false;

                Show.Text = sText;
                OneMinute = false;
                CanUse_timer.Interval = StandartIndervalCanUse;
                CanUse_timer.Enabled = true;
            }
            else
            {
                Timer_timer.Enabled = false;
                this.Close();
                Application.ExitThread();
            }
        }
        else if (!T.User.Rows.Get<bool>(data.UserID, C.User.Enabled))
        {
            Count = MaxCount - 1;
            OneMinute = true;
            CanUse_timer.Interval = CloseIndervalCanUse;
            var Cause = T.User.Rows.Get<string>(data.UserID, C.User.Cause);

            if (Cause.Length == 0)
            { Cause = " не указана."; }
            else
            { Cause = ":\r\n\"" + Cause + "\".\r\n"; }

            Timer_timer.Interval = 10 * 100;
            Timer_timer.Enabled = true;

            new Thread(() =>
            {
                Show.Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show(Show, "Использование этой учетной записи запрещено, причина" + Cause + " Пожалуйста закончите вашу работу и выйдите из программы, в противном случае программа завершит свою работу через одну минуту.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }));
            }).Start();
        }
        T.User.Rows.Set(data.UserID, C.User.IsHere, DataBase.AutoStatus.Used);
        CanUse_timer.Enabled = true;
    }

    private void Timer_timer_Tick(object sender, EventArgs e)
    {
        if (Count > 15)
        { Show.Text = sText + ". Осталось " + Count.ToString() + " сек."; }
        else
        { Show.Text = sText + ". Внимание! Осталось " + Count.ToString() + " сек."; }
        Count--;
    }
}
