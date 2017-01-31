using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;

namespace Trivia.Scoring
{
    public static class DataGridRowBehavior
    {
        public static bool GetIsBroughtIntoViewWhenSelected(DataGridRow dataGridRow)
        {
            return (bool)dataGridRow.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(DataGridRow dataGridRow, bool value)
        {
            dataGridRow.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty = DependencyProperty.RegisterAttached(
            "IsBroughtIntoViewWhenSelected",
            typeof(bool),
            typeof(DataGridRowBehavior),
            new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        static void OnIsBroughtIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            DataGridRow item = depObj as DataGridRow;
            if (item == null) return;
            if (e.NewValue is bool == false) return;
            if ((bool)e.NewValue)
            {
                item.Selected += OnDataGridRowSelected;
            }
            else
            {
                item.Selected -= OnDataGridRowSelected;
            }
        }

        static void OnDataGridRowSelected(object sender, RoutedEventArgs e)
        {
            if (!object.ReferenceEquals(sender, e.OriginalSource)) return;
            DataGridRow item = e.OriginalSource as DataGridRow;
            if (item != null) item.BringIntoView();
        }
    }
}
