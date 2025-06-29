using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID = -1;
        private clsUser _User;
        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        private void _FillUserInfo()
        {
            _UserID = _User.UserID;
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUsername.Text = _User.UserName;
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
        }
        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindByUserID(UserID);
            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User With UserID = " + UserID.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillUserInfo();
        }
        private void _ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();
            _UserID = -1;
            lblUserID.Text = "[???]";
            lblUserID.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}