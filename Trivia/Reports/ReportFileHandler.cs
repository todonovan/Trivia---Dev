using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Reports
{
    public class ReportFileHandler
    {
        public void OpenReport(string fileName)
        {
            string fullName = ConfigurationManager.AppSettings["report_save_config"].ToString() + fileName;
        }

        public void DeleteReport(string fileName)
        {
            string fullName = ConfigurationManager.AppSettings["report_save_config"].ToString() + fileName;
        }
    }
}
