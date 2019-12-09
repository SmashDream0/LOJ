using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LaboratoryOnlineJournal;

public partial class SetNewPassWord_Form : Form
{
    uint ID;
    public SetNewPassWord_Form(uint ID)
    {
        InitializeComponent();
        this.ID = ID;
    }

    private void Continue_button_Click(object sender, EventArgs e)
    {
        if (NewPassWord_Box.Text.Length == 0)
        {
            MessageBox.Show(this, "Необходимо ввести пароль!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NewPassWord_Box.Focus();
            return;
        }

        if (NewPassWord_Box.Text != RepeateNewPassWord_Box.Text)
        {
            MessageBox.Show(this, "Пароли не совпадают!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RepeateNewPassWord_Box.Focus();
            return;
        }

        T.User.Rows.Set(ID, C.User.Pass, NewPassWord_Box.Text);
        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
    }

    private void Cancel_button_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Пароль не будет изменен. Вы уверены, что хотите оставить все как есть ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            return;

        this.DialogResult = System.Windows.Forms.DialogResult.No;
    }
}
