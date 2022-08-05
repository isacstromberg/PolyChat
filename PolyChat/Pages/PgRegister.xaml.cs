using PolyChat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolyChat.Pages
{
    /// <summary>
    /// Interaction logic for PgRegister.xaml
    /// </summary>
    public partial class PgRegister : Page
    {
        Db_Repository db = new Db_Repository();
        public PgRegister()
        {
            InitializeComponent();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string Email = TxtEmail.Text;
            string Password = TxtPassword.Text;


            if (db.CheckIfEmailIsRegistered(db.GetPolyUsers(), Email) == false)
            {
                MessageBox.Show("This email already has an registered user");
                return;
            } 

            db.AddUser(Email, Password);
            MessageBox.Show("Your new account has been successfully registered.");
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
