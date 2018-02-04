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
    /// Interaction logic for AddTaskControl.xaml
    /// </summary>
    public partial class AddTaskControl : UserControl
    {

        public static readonly RoutedEvent BackOverviewEvent = EventManager.RegisterRoutedEvent("BackOverviewEvent", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(AddTaskControl));

        private bool valid = false;

        public bool Valid { get => valid; set => valid = value; }

        public AddTaskControl()
        {
            InitializeComponent();
            DueDate.DisplayDateStart = DateTime.Today.AddDays(1);
            DueDate.DisplayDate = DateTime.Today.AddDays(1);
            CheckValidity();
        }

        private Brush GetBrushValidityFromTextBox(int size)
        {
            var bc = new BrushConverter();
            return (Brush)bc.ConvertFrom((size == 0)? "#ff3f34" : "#0be881");
        }

        private bool CheckValidity()
        {
            int titleSize = TitleTextBox.Text.Count();
            int descriptionSize = DescriptionTextBox.Text.Count();

            SaveButton.Background = GetBrushValidityFromTextBox(titleSize * descriptionSize);
            DescriptionTextBox.BorderBrush = GetBrushValidityFromTextBox(descriptionSize);
            TitleTextBox.BorderBrush = GetBrushValidityFromTextBox(titleSize);

            if (titleSize == 0 || descriptionSize == 0)
            {
                SaveButton.Content = "Please fill the " + ((titleSize == 0) ? "Title" : "Description") + " input";
                return false;
            }
            else
            {
                SaveButton.Content = "SAVE";
                return true;
            }
        }
        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckValidity();
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckValidity();
        }

        private void PrioritySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = PrioritySelection.SelectedItem as ComboBoxItem;
            PriorityDisplay.Fill = item.Background;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidity() == false)
                return;

            string date = DueDate.ToString();
            if (date.Count() == 0)
                date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            
            var data = new Dictionary<string, string>
            {
                { "Title", "'" + TitleTextBox.Text + "'" },
                { "Description", "'" + DescriptionTextBox.Text + "'" },
                { "Tags", "'" + TagsTextBox.Text + "'" },
                { "Priority",  "'" + MainWindow.GetPriorityFromString(((PrioritySelection.SelectedItem as ComboBoxItem).Content as Label).Content.ToString()) + "'"},
                { "EndDate", "'" + date + "'" }
            };

            MainWindow.Db.Insert(table: "Tasks", data: data);
            RaiseEvent(new RoutedEventArgs(AddTaskControl.BackOverviewEvent));
        }
    }
}
