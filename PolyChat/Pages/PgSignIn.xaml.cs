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
using Npgsql;
using PolyChat.Repository;

namespace PolyChat.Pages
{
    /// <summary>
    /// Interaction logic for PgSignIn.xaml
    /// </summary>
    public partial class PgSignIn : Page
    {
        Db_Repository db = new Db_Repository();
        public PgSignIn()
        {
            InitializeComponent();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string Email = TxtEmail.Text;
            string Password = TxtPassword.Password;
            if (db.CheckIfUserExist(Email, Password) == true && db.CheckIfPasswordMatchesUser(Email, Password) == true)
            {
                MessageBox.Show("Success");
                this.NavigationService.Navigate(new WinHomescreen());
            }

            if (db.CheckIfPasswordMatchesUser(Email, Password) == false)
            {
                MessageBox.Show("Entered password is not valid");
            }

            if (db.CheckIfUserExist(Email, Password) == false)
            {
                MessageBox.Show("The entered email is not a registered user");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PgRegister());
        }
            






    }
}
