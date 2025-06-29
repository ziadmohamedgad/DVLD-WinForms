using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People.Conrols
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> Handler = OnPersonSelected;
            if (Handler != null)
                Handler(PersonID);
        }
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        private int _PersonID = -1;
        public int PersonID
        {
            get
            {
                return ctrlPersonCard1.PersonID;
            }
        }
        private clsPerson SelectedPersonInfo
        {
            get
            {
                return ctrlPersonCard1.SelectedPersonInfo;
            }
        }
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }
        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            _FindNow();
        }
        private void _FindNow()
        {
            switch(cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text.Trim()));
                    break;
                case "National No.":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text.Trim());
                    break;
            }
            if (OnPersonSelected != null && FilterEnabled && ctrlPersonCard1.SelectedPersonInfo != null)
                PersonSelected(ctrlPersonCard1.PersonID);
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid!, Put The Mouse Over The Red Icon(s) To See The" +
                    "Error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FindNow();
        }
        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This Field Is Required!");
            }
            else
                errorProvider1.SetError(txtFilterValue, null);
        }
        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }
        private void DataBackEvent(object Sender, int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            if (OnPersonSelected != null && ctrlPersonCard1.SelectedPersonInfo != null)
                PersonSelected(ctrlPersonCard1.PersonID);
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }
        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // it means ENTER Key
            {
                btnFind.PerformClick();
            }
            if (cbFilterBy.SelectedIndex == 1)
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar);
            }
        }
    }
}
