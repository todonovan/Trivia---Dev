﻿using System;
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

namespace Trivia.Scoring
{
    /// <summary>
    /// Interaction logic for ScorerRoundScorecardView.xaml
    /// </summary>
    public partial class ScorerRoundScorecardView : UserControl
    {
        public ScorerRoundScorecardView()
        {
            InitializeComponent();
            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
