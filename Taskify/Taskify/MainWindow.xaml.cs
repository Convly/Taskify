using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Database Db;
        private Label selected = null;

        public Label Selected { get => selected; set => selected = value; }

        public MainWindow()
        {
            String DBPath = AppDomain.CurrentDomain.BaseDirectory + "\\Tasks.mdb";
            Db = new Database(DBPath);
            AddHandler(AddTaskControl.BackOverviewEvent, new RoutedEventHandler(BackHome_Handler));
            AddHandler(ProductivityControl.AddTaskEvent, new RoutedEventHandler(BackAddTask_Handler));
            InitializeComponent();
            SetSelection(OverviewButton);
            this.ContentController.Content = new OverviewControl();
        }

        private void SetSelection(Label lab)
        {
            Selected = lab;
            ResetSelected();
            if (lab != null)
                Selected.Foreground = (Brush)((new BrushConverter()).ConvertFrom("#ff3f34"));
        }

        private void ResetSelected()
        {
            Brush colorWhite = (Brush)((new BrushConverter()).ConvertFrom("White"));
            OverviewButton.Foreground = colorWhite;
            HistoryButton.Foreground = colorWhite;
            ProductivityButton.Foreground = colorWhite;
            //StatButton.Foreground = colorWhite;
        }

        private void Click_Button_Overview(object sender, RoutedEventArgs e)
        {
            SetSelection(OverviewButton);
            this.ContentController.Content = new OverviewControl();
        }
        private void Click_Button_History(object sender, RoutedEventArgs e)
        {
            SetSelection(HistoryButton);
            ContentController.Content = new HistoryControl();
        }

        private void Click_Button_Productivity(object sender, RoutedEventArgs e)
        {
            SetSelection(ProductivityButton);
            this.ContentController.Content = new ProductivityControl();
        }

        private void Click_Button_Stat(object sender, RoutedEventArgs e)
        {
            //SetSelection(StatButton);
            this.ContentController.Content = new ProductivityControl();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelection(null);
            this.ContentController.Content = new AddTaskControl();
        }

        public void BackHome_Handler(object sender, RoutedEventArgs e)
        {
            SetSelection(OverviewButton);
            ContentController.Content = new OverviewControl();
        }

        public void BackAddTask_Handler(object sender, RoutedEventArgs e)
        {
            SetSelection(null);
            ContentController.Content = new AddTaskControl();
        }

        public void BackProductivity_Handler(object sender, RoutedEventArgs e)
        {
            SetSelection(ProductivityButton);
            ContentController.Content = new ProductivityControl();
        }
        public static List<Taskify.Task> GetTasks(string table, string query, string cond)
        {
            List<Taskify.Task> data = new List<Task>();

            OleDbDataReader reader = MainWindow.Db.Select(table, query, cond);
            while (reader.Read())
            {
                data.Add(new Taskify.Task(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    MainWindow.GetPriorityFromInt(Int32.Parse(reader.GetString(4))),
                    reader.GetString(5),
                    reader.GetDateTime(6),
                    reader.GetDateTime(7)
                ));
            }

            return data;
        }

        public static TaskControl CreateNewTask(Taskify.Task task)
        {
            TaskControl control = new TaskControl(task);

            control.Title.Content = task.Title;
            control.Description.Text = task.Description;
            control.Background = GetBrushFromPriority(task.Priority);
            control.Background.Opacity = 0.4;
            control.CountDown.Background = GetBrushFromPriority(task.Priority);
            control.CountDown.Background.Opacity = 0.3;
            control.Priority = task.Priority;
            if (control.Task.DueDate >= DateTime.Now)
            {
                var diff = control.Task.DueDate - DateTime.Now;
                control.CountDown.Content = ((int)(diff.TotalHours)).ToString() + ":" + diff.ToString(@"mm\:ss");
            }
            else
                control.CountDown.Content = control.Task.DueDate.ToString(@"MM/dd/yyyy");

            return control;
        }

        public static Brush GetBrushFromPriority(String priority)
        {
            Dictionary<string, string> priorityBrush = new Dictionary<string, string>
            {
                { "Prioritary", "#FFF39C12" },
                { "High", "#FF8E44AD" },
                { "Medium", "#FF22A7F0" },
                { "Low", "#FF00B16A" },
                { "Common", "#FFDADFE1" }
            };

            var bc = new BrushConverter();
            return (Brush)bc.ConvertFrom(priorityBrush[priority]);
        }

        public static int GetPriorityFromString(String entry)
        {
            Dictionary<String, int> pfs = new Dictionary<String, int>
            {
                { "Prioritary", 0 },
                { "High", 1 },
                { "Medium", 2 },
                { "Low", 3 },
                { "Common", 4 },
            };
            return pfs[entry];
        }

        public static String GetPriorityFromInt(int entry)
        {
            Dictionary<int, String> pfs = new Dictionary<int, String>
            {
                { 0, "Prioritary" },
                { 1, "High" },
                { 2, "Medium" },
                { 3, "Low" },
                { 4, "Common" },
            };
            return pfs[entry];
        }

        private void SideButtonEnter_Event(object sender, RoutedEventArgs e)
        {
            Label lb = (sender as Label);
            if (Selected != null && lb.Name == Selected.Name)
                return;
            lb.Foreground = (Brush)((new BrushConverter()).ConvertFrom("#ff3f34"));
        }

        private void SideButtonLeave_Event(object sender, RoutedEventArgs e)
        {
            Label lb = (sender as Label);
            if (Selected != null && lb.Name == Selected.Name)
                return;
            lb.Foreground = (Brush)((new BrushConverter()).ConvertFrom("White"));
        }
    }
}
