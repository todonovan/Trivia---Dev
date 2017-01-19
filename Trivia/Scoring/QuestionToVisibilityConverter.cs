using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class QuestionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Question q = (Question)value;
            if (q == Question.NotAnswered) return new SolidColorBrush(Colors.Yellow);
            else if (q == Question.NotJudged) return new SolidColorBrush(Colors.Black);
            else if (q == Question.Correct) return new SolidColorBrush(Colors.Green);
            else return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
