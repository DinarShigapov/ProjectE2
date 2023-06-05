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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CallSchedulePage.xaml
    /// </summary>
    public partial class CallSchedulePage : Page
    {
        public CallSchedulePage()
        {
            InitializeComponent();
            LVCallSchedule.ItemsSource = App.DB.ClassTime.ToList();
            var timeNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            foreach (var item in App.DB.ClassTime.ToList())
            {
                if (item.StartClass <= timeNow &&
                    item.EndClass >= timeNow)
                {
                    LVCallSchedule.SelectedIndex = item.Id - 1;
                    switch (item.Id)
                    {
                        case 1:
                        case 2:
                        case 3:
                            ImageShow.Source = (ImageSource)this.TryFindResource("ComputerDrawingImage");
                            break;
                        case 4:
                        case 5:
                            ImageShow.Source = (ImageSource)this.TryFindResource("SkillsDrawingImage");
                            break;
                        case 6:
                        case 7:
                            ImageShow.Source = (ImageSource)this.TryFindResource("HardWorkDrawingImage");
                            break;
                    }
                    break;

                }
                else
                {
                    ImageShow.Source = (ImageSource)this.TryFindResource("LogoV1DrawingImage");
                }
            }

        }
    }
}
