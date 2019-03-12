using FeedbackManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace FeedbackManager.WPF.Helpers
{
    public class ChartGenerator
    {
        private readonly IEnumerable<Feedback> feedbacks;
        private readonly DateTime reportDate;
        private readonly string destinationFolder;

        public ChartGenerator(IEnumerable<Feedback> feedbacks, DateTime reportDate, string destinationFolder)
        {
            this.feedbacks = feedbacks;
            this.reportDate = reportDate;
            this.destinationFolder = destinationFolder;
        }

        public void GenerateCharts()
        {
            var excel = new Excel.Application();
            var workbook = excel.Workbooks.Add();
            var chartNumber = 1;

            var chart = (Excel.Chart)workbook.Charts.Add();
            chart.ApplyDataLabels();
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartType = Excel.XlChartType.xlColumnClustered;
            chart.ChartTitle.Text = $"Feedback trend in {reportDate.Year}";

            var xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Year-quarter";
            var yAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = "Number of feedback";

            var data = new Dictionary<string, IList<Feedback>>();

            var reportDateQuarter = (reportDate.Month + 2) / 3;

            for(int quarter = 1;quarter<=reportDateQuarter;quarter++)
            {
                data.Add($"{reportDate.Year}-Q{reportDateQuarter}", feedbacks.Where(f => f.DateReceived.Year == reportDate.Year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

                if (quarter == reportDateQuarter)
                    break;
            }

            var seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();
            var series0 = seriesCollection.NewSeries();
            series0.Name = "Total feedbacks";
            series0.XValues = data.Keys.ToArray();
            series0.Values = data.Values.Select(v => v.Count()).ToArray();
            series0.ApplyDataLabels();

            var series1 = seriesCollection.NewSeries();
            series1.Name = "Complaints";
            series1.XValues = data.Keys.ToArray();
            series1.Values = data.Values.Select(v => v.Where(f => f.FeedbackNature == FeedbackNature.Complaint).Count()).ToArray();
            series1.ApplyDataLabels();

            var series2 = seriesCollection.NewSeries();
            series2.Name = "Compliments";
            series2.XValues = data.Keys.ToArray();
            series2.Values = data.Values.Select(v => v.Where(f => f.FeedbackNature == FeedbackNature.Compliment).Count()).ToArray();
            series2.ApplyDataLabels();

            var series3 = seriesCollection.NewSeries();
            series3.Name = "For information";
            series3.XValues = data.Keys.ToArray();
            series3.Values = data.Values.Select(v => v.Where(f => f.FeedbackNature == FeedbackNature.Information).Count()).ToArray();
            series3.ApplyDataLabels();

            chart.Export($@"{destinationFolder}\{chartNumber} - {chart.ChartTitle.Text}.png", "PNG");
            chartNumber++;
            chart.Delete();
        }
    }
}
