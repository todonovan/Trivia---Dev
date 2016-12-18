using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.Reports
{
    public class ReportClassMap : CsvClassMap<Question>
    {
        public ReportClassMap()
        {
            Map(q => q);
        }
    }
}
