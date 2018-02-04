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
using Taskify.Tools;

namespace Taskify
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        private string priority = "Common";
        private Task task;
        private bool pAction = false;

        public static readonly RoutedEvent BackOverviewEvent = EventManager.RegisterRoutedEvent("BackOverviewEvent", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TaskControl));

        public TaskControl(Task task_)
        {
            InitializeComponent();
            Task = task_;
            if (Task.DueDate < DateTime.Now)
                TaskAction.Source = null;
        }

        public string Priority { get => priority; set => priority = value; }
        public bool PAction { get => pAction; set => pAction = value; }
        public Task Task { get => task; set => task = value; }

        private void Task_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Task.DueDate < DateTime.Now || Task.Status == 1)
                return;

            String table = "Tasks";
            Dictionary<String, String> data = new Dictionary<string, string>
            {
                { "Status", "1" }
            };
            String cond = " WHERE ID=" + Task.Id.ToString();
            MainWindow.Db.Update(table, data, cond);
            RaiseEvent(new RoutedEventArgs(AddTaskControl.BackOverviewEvent));
        }

        private void Task_MouseEnter(object sender, MouseEventArgs e)
        {
            PAction = true;

            if (Task.DueDate < DateTime.Now || Task.Status == 1)
                return;

            var bc = new BrushConverter();
            Brush color = (Brush)bc.ConvertFrom("#FF0BE881");
            Background = color;
            Background.Opacity = 0.75;
            CountDown.Background = color; 
            CountDown.Background.Opacity = 0.75;
        }

        private void Task_MouseLeave(object sender, MouseEventArgs e)
        {
            PAction = false;

            if (Task.DueDate < DateTime.Now || Task.Status == 1)
                return;

            var bc = new BrushConverter();
            Brush color = MainWindow.GetBrushFromPriority(Priority);
            Background = color;
            Background.Opacity = 0.4;
            CountDown.Background = color;
            CountDown.Background.Opacity = 0.3;
        }

        private void Background_MouseEnter(object sender, MouseEventArgs e)
        {
            if (PAction == false)
                Background.Opacity = 0.25;
        }
        private void Background_MouseLeave(object sender, MouseEventArgs e)
        {
            if (PAction == false)
                Background.Opacity = 0.4;
        }
    }
}
