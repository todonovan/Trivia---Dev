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
using TriviaData.Models;
using TriviaData.Repos;
using System.Data.SQLite;
using System.Configuration;
using TriviaData;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;


namespace Trivia.Teams
{
    /// <summary>
    /// Interaction logic for TeamEditView.xaml
    /// </summary>
    public partial class TeamAddEditView : UserControl
    {
           
        public TeamAddEditView()
        {
            InitializeComponent();
        }
    }
}
