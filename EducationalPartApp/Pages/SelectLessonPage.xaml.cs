using EducationalPartApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для SelectLessonPage.xaml
    /// </summary>
    public partial class SelectLessonPage : Page
    {
        public ObservableCollection<TableRow> TableRows { get; set; }
        public SelectLessonPage(List<List<Schedule>> schedules)
        {
            InitializeComponent();

            TableRows = new ObservableCollection<TableRow>();

            for (int i = 1; i <= App.DB.ClassTime.Count(); i++)
            {


                TableRows.Add(new TableRow 
                { 
                    Pair = i.ToString(), 
                    Monday = schedules[0][i - 1].Discipline != null ? true : false,
                    Tuesday = schedules[1][i - 1].Discipline != null ? true : false, 
                    Wednesday = schedules[2][i - 1].Discipline != null ? true : false, 
                    Thursday = schedules[3][i - 1].Discipline != null ? true : false, 
                    Friday = schedules[4][i - 1].Discipline != null ? true : false, 
                    Saturday = schedules[5][i - 1].Discipline != null ? true : false
                });
            }
            DataGrid.ItemsSource = TableRows;
        }

        public class TableRow
        {
            public string Pair { get; set; }
            public bool Monday { get; set; }
            public bool Tuesday { get; set; }
            public bool Wednesday { get; set; }
            public bool Thursday { get; set; }
            public bool Friday { get; set; }
            public bool Saturday { get; set; }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
