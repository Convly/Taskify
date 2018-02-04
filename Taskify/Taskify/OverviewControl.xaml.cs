using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Taskify.Tools;

namespace Taskify
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        private DispatcherTimer t;

        public DispatcherTimer T { get => t; set => t = value; }

        public OverviewControl()
        {
            InitializeComponent();

            LoadTodoList();

            RefreshCounters(null, null);

            T = new DispatcherTimer();
            T.Tick += RefreshCounters;
            T.Interval = TimeSpan.FromMilliseconds(100);
            T.Start();
        }

        private void LoadTodoList()
        {
            List<Taskify.Task> tasks = MainWindow.GetTasks("Tasks", "*", "WHERE EndDate >= Now() AND Status = 0 ORDER BY EndDate,Priority ASC");

            foreach (var task in tasks)
            {
                TodoPanel.Children.Add(MainWindow.CreateNewTask(task));
            }
        }

        private void RefreshCounters(object sender, EventArgs e)
        {
            var collection = TodoPanel.Children;
            foreach (var task in collection)
            {
                TaskControl control = task as TaskControl;

                var diff = control.Task.DueDate - DateTime.Now;
                String time = ((int)(diff.TotalHours)).ToString() + ":" + diff.ToString(@"mm\:ss");
                BrushConverter bc = new BrushConverter();
                control.CountDown.Foreground = (Brush)bc.ConvertFrom((diff.TotalDays < 1)? "#ff3f34": "NavajoWhite");
                control.CountDown.Content = time;
                if (control.Task.DueDate < DateTime.Now || control.Task.Status == 1)
                {
                    TodoPanel.Children.Clear();
                    LoadTodoList();
                }
            }
        }
    }
}
