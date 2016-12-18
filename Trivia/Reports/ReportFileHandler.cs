using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Trivia.Reports
{
    public class ReportFileHandler
    {
        private Excel.Application _excelApp;

        public ReportFileHandler()
        {
            _excelApp = new Excel.Application();
        }

        public void OpenReport(string fileName)
        {
            string fullName = ConfigurationManager.AppSettings["report_save_config"].ToString() + fileName;
            try
            {
                Excel.Workbook workbook = _excelApp.Workbooks.Open(fullName);
                _excelApp.Visible = true;
            }
            catch
            {

            }
        }

        public void DeleteReport(string fileName)
        {
            string dirName = ConfigurationManager.AppSettings["report_save_config"].ToString();
            File.Delete(dirName + fileName);            
        }
    }
}
