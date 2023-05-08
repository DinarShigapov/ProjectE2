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

namespace EducationalPartApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupSelectPage.xaml
    /// </summary>
    public partial class GroupSelectPage : Page
    {
        public GroupSelectPage()
        {
            InitializeComponent();
        }

        private void BGroupSelection_Click(object sender, RoutedEventArgs e)
        {
            var group326 = App.DB.Group.FirstOrDefault(x => x.Id == 1);
            new MainWindow().ChangeFrameWindow(new AddSchedulePage(group326));
        }
    }
}
