using ProteinTrackerClient.ProteinTrackerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProteinTrackerClient
{
    public partial class ProteinTrackerForm : Form
    {
        private ProteinTrackingServiceSoapClient service = new ProteinTrackingServiceSoapClient();
        private User[] users;

        public ProteinTrackerForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            users = service.ListUsers();
            ddlUser.DataSource = users;
            ddlUser.DisplayMember = "Name";
            ddlUser.ValueMember = "UserID";
        }

        private void OnAddNewUser(object sender, EventArgs e)
        {
            service.AddUser(tbName.Text, int.Parse(tbGoal.Text));
            users = service.ListUsers();
            ddlUser.DataSource = users;
        }

        private void OnUserChanged(object sender, EventArgs e)
        {
            var index = ddlUser.SelectedIndex;
            lblTotal.Text = users[index].Total.ToString();
            lblGoal.Text = users[index].Goal.ToString();
        }

        async private void OnAddProtein(object sender, EventArgs e)
        {
            var userId = users[ddlUser.SelectedIndex].UserID;
            try
            {
                var auth = new AuthenticationHeader { UserName = "A", Password = "Pass" };
                var newTotal = await service.AddProteinAsync(auth, int.Parse(tbAmount.Text), userId);
                users[ddlUser.SelectedIndex].Total = newTotal.AddProteinResult;
                lblTotal.Text = newTotal.AddProteinResult.ToString();
            }
            catch (FaultException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
