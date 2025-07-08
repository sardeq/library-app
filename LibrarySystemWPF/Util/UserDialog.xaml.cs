using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System.Windows;

namespace LibrarySystemWPF
{
    public partial class UserDialog : Window
    {
        private readonly UserService _userService = new UserService();
        public User User { get; set; }

        public UserDialog(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = User;
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            cmbGender.ItemsSource = _userService.GetGenders().DefaultView;
            cmbStatus.ItemsSource = _userService.GetStatusList().DefaultView;
            cmbType.ItemsSource = _userService.GetUserTypes().DefaultView;
            cmbLanguage.ItemsSource = _userService.GetLanguages().DefaultView;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Set password from password box
            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                User.Password = txtPassword.Password;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}