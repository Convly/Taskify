using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Taskify
{
    /// <summary>
    /// Interaction logic for ProductivityControl.xaml
    /// </summary>
    public partial class ProductivityControl : UserControl
    {
        private DispatcherTimer t;
        private Task cTask = null;
        public static readonly RoutedEvent AddTaskEvent = EventManager.RegisterRoutedEvent("AddTaskEvent", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(ProductivityControl));

        public DispatcherTimer T { get => t; set => t = value; }
        public Task CTask { get => cTask; set => cTask = value; }

        public ProductivityControl()
        {
            InitializeComponent();

            LoadTask();
            RefreshCounters(null, null);

            T = new DispatcherTimer();
            T.Tick += RefreshCounters;
            T.Interval = TimeSpan.FromSeconds(1);
            T.Start();
        }

        private void RefreshCounters(object sender, EventArgs e)
        {
            if (CTask == null)
            {
                CountDown.Content = DateTime.Now.ToLongTimeString();
                return;
            }
            var diff = CTask.DueDate - DateTime.Now;
            String time = ((int)(diff.TotalHours)).ToString() + ":" + diff.ToString(@"mm\:ss");
            BrushConverter bc = new BrushConverter();
            CountDown.Foreground = (Brush)bc.ConvertFrom((diff.TotalDays < 1) ? "#ff3f34" : "NavajoWhite");
            var percentage = (DateTime.Now - CTask.StartDate).TotalSeconds / (CTask.DueDate - CTask.StartDate).TotalSeconds;
            CWidth.Width = new GridLength(percentage, GridUnitType.Star);
            CWidthHover.Width = new GridLength(1 - percentage, GridUnitType.Star);
            CountDown.Content = time;
            if (CTask.DueDate < DateTime.Now || CTask.Status == 1)
            {
                ArchiveAndGetNewTask();
            }
        }

        private void LoadTask()
        {
            List<Taskify.Task> tasks = MainWindow.GetTasks("Tasks", "*", "WHERE EndDate >= Now() AND Status = 0 ORDER BY EndDate,Priority ASC");

            if (tasks.Count() != 0)
            {
                CTask = tasks[0];
                Title.Content = CTask.Title;
                Description.Text = CTask.Description;
                Tags.Content = (CTask.Tags.Count() > 0 && CTask.Tags[0] != "")? string.Join(", ", CTask.Tags): "No tag associated to this task";
                var diff = CTask.DueDate - DateTime.Now;
                CountDown.Content = ((int)(diff.TotalHours)).ToString() + ":" + diff.ToString(@"mm\:ss");
                Main.Background = MainWindow.GetBrushFromPriority(CTask.Priority);
                Main.Background.Opacity = 0.4;
                CountDown.Background = MainWindow.GetBrushFromPriority(CTask.Priority);
                CountDown.Background.Opacity = 0.3;
            }
            else
            {
                Title.Content = "Oups...";
                Description.Text = "It seems that you don't have any task to do, please come back when you will have some work to achieve :)";
                CountDown.Content = DateTime.Now.ToLongTimeString();
                Tags.Content = "";
            }
        }

        private void Done_MouseEnter(object sender, MouseEventArgs e)
        {
            Random rd = new Random();
            int r = rd.Next(0, 5);
            Console.WriteLine(r);

            Done.Background.Opacity = 0.7;

            string rem = "";
            switch (r)
            {
                case (0):
                    rem = "You're the best!";
                    break;
                case (1):
                    rem = "No kidding, you're awesome";
                    break;
                case (2):
                    rem = "Hold my beer, I've work to do";
                    break;
                case (3):
                    rem = "3... 2... 1... Coffee time";
                    break;
                case (4):
                    rem = "2 + 2 is 4, minus 1 that's 3. Quicks math";
                    break;
            }

            Done.Content = rem;
        }

        private void Done_MouseLeave(object sender, MouseEventArgs e)
        {
            Done.Background.Opacity = 1;
            Done.Content = "DONE";
        }

        private void Done_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ArchiveAndGetNewTask();
        }

        private void ArchiveAndGetNewTask()
        {
            Dictionary<String, String> data = new Dictionary<string, string>
            {
                { "Status", "1" }
            };
            MainWindow.Db.Update("Tasks", data, " WHERE ID=" + CTask.Id.ToString());
            LoadTask();
            RefreshCounters(null, null);
        }
    }
}
