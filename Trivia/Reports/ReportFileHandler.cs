using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;
using Excel = Microsoft.Office.Interop.Excel;
using CsvHelper;

namespace Trivia.Reports
{
    public class ReportFileHandler
    {
        /* Note: Excel interop is a bit of a mess due to COM legacy code 
         * and a moderately confusing API. Against part of my better
         * judgment I've left the interop in for file viewing as it provides
         * a simple (for the user) way to view the reports and edit/transfer
         * as needed. If this starts breaking extensively in the future, a
         * better solution will likely be to parse the CSV report data into
         * a DataGrid in a new view -- but note that any editing by the user
         * would then break the report view. The simplest of all solutions,
         * of course, would be to generate the reports but offer no way
         * to manage/view the reports in-app. */


        private Excel.Application _excelApp;

        /// <summary>
        /// Reports are saved as .csv files for increased flexibility.
        /// App currently programmed to open these files as Excel files.
        /// </summary>
        /// <param name="gs"></param>
        public void CreateReport(GameState gs)
        {
            string fullName = ConfigurationManager.AppSettings["report_save_config"].ToString() + gs.FileName + ".csv";
            using (var file = File.CreateText(fullName))
            {
                var csv = new CsvWriter(file);
                foreach (var s in gs.ActiveScorers)
                {
                    csv.WriteField(s.Name);
                    foreach (var t in s.ScoringTeams)
                    {
                        csv.WriteField(t.Team.Name);
                        csv.WriteField(t.Score);
                        var answers = t.GetAllNonBonusAnswers();
                        var answerStrings = new List<List<string>>();
                        answers.ForEach(round => answerStrings.Add(round.Select(a => a.ToString()).ToList()));
                        for (int i = 0; i < answers.Count; i++)
                        {
                            csv.WriteField(i);
                            foreach (var answer in answerStrings[i])
                            {
                                csv.WriteField(answer);
                            }
                        }
                        foreach (var b in t.BonusRoundAnswers)
                        {
                            csv.WriteField(b.Wager);
                            csv.WriteField(b.Answer.ToString());
                        }
                    }
                    csv.NextRecord();
                }
            }
        }

        public void CreateBetterReport(GameState gs)
        {
            string fullName = ConfigurationManager.AppSettings["report_save_config"].ToString() + gs.FileName + ".csv";
            using (var file = File.CreateText(fullName))
            {

            }
        }

        public void OpenReport(string fileName)
        {
            _excelApp = new Excel.Application();
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
