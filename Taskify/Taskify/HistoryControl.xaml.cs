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

namespace Taskify
{
    /// <summary>
    /// Interaction logic for HistoryControl.xaml
    /// </summary>
    public partial class HistoryControl : UserControl
    {
        public HistoryControl()
        {
            InitializeComponent();
            LoadDoneList();
        }
        private void LoadDoneList()
        {
            List<Taskify.Task> tasks = MainWindow.GetTasks("Tasks", "*", "WHERE EndDate < Now() OR Status = 1 ORDER BY EndDate DESC");

            foreach (var task in tasks)
            {
                TaskControl tc = MainWindow.CreateNewTask(task);
                tc.TaskAction.Source = null;
                DonePanel.Children.Add(tc);
            }
        }

    }
}
