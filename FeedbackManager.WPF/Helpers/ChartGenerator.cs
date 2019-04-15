using FeedbackManager.WPF.Models;
using FeedbackManager.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace FeedbackManager.WPF.Helpers
{
    public class ChartsGenerator
    {
        private readonly IFeedbackService feedbackService;
        private readonly IEnumerable<Feedback> feedbacks;
        private readonly DateTime reportDate;
        private readonly string destinationFolder;
        private int reportDateQuarter => (reportDate.Month + 2) / 3;
        private readonly IEnumerable<Department> departments;

        public EventHandler<string> ChartCreated;

        Excel.Application excel;
        Excel.Workbook workbook;
        int chartNumber;

        public ChartsGenerator(IFeedbackService feedbackService, IEnumerable<Feedback> feedbacks, DateTime reportDate, string destinationFolder)
        {
            this.feedbackService = feedbackService;
            this.feedbacks = feedbacks;
            this.reportDate = reportDate;
            this.destinationFolder = destinationFolder;

            departments = feedbackService.GetAllDepartments();
        }

        public async Task GenerateChartsAsync()
        {
            await Task.Run(() =>
            {
                excel = new Excel.Application();
                workbook = excel.Workbooks.Add();
                chartNumber = 1;

                DrawChart_FeedbackVsQuarter_per_FeedbackNature(startYear: 2015);
                DrawChart_FeedbackVsQuarter_per_FeedbackNature(startYear: reportDate.Year);
                DrawChart_FeedbackVsQuarter_per_FeedbackNature(startYear: 2015, isPercentage: true);
                DrawChart_FeedbackVsQuarter_per_FeedbackNature(startYear: reportDate.Year, isPercentage: true);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackNature_individualDepartment(department, startYear: 2015);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackNature_individualDepartment(department, startYear: reportDate.Year);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackNature_individualDepartment(department, startYear: 2015, isPercentage: true);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackNature_individualDepartment(department, startYear: reportDate.Year, isPercentage: true);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackCategory_individualDepartment(department, startYear: 2015);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackCategory_individualDepartment(department, startYear: 2015, isPercentage: true);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackCategory_individualDepartment(department, startYear: reportDate.Year);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackCategory_individualDepartment(department, startYear: reportDate.Year, isPercentage: true);

                workbook.Close(SaveChanges: true, Filename: $@"{destinationFolder}\charts.xlsx");
                excel.Quit();
            });
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackNature(int startYear, bool isPercentage = false)
        {
            Excel.Chart chart = (startYear == reportDate.Year) ?
                InitialiseChart($"Feedback trend in {reportDate.Year}-Q{reportDateQuarter}{(isPercentage ? " (percentage)" : "")}", isPercentage) :
                InitialiseChart($"Feedback trend since {startYear}{(isPercentage ? " (percentage)" : "")}", isPercentage);

            var data = new Dictionary<string, IList<Feedback>>();

            for (int year = startYear; year <= reportDate.Year; year++)
            {
                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    data.Add($"{year}-Q{quarter}", feedbacks.Where(f => f.DateReceived.Year == year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

                    if (year == reportDate.Year && quarter == reportDateQuarter)
                        break;
                }
            }

            var feedbackNatures = FeedbackNature.FeedbackNaturesForChart;

            SetChartData(chart, data, feedbackNatures, isPercentage);

            ExportChart(chart);
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackNature_individualDepartment(Department department, int startYear, bool isPercentage = false)
        {
            Excel.Chart chart = (startYear == reportDate.Year) ?
                InitialiseChart($"Feedback nature in {reportDate.Year}-Q{reportDateQuarter} for {department.Name}{(isPercentage ? " (percentage)" : "")}", isPercentage) :
                InitialiseChart($"Feedback nature trend since {startYear} for {department.Name}{(isPercentage ? " (percentage)" : "")}", isPercentage);

            var data = new Dictionary<string, IList<Feedback>>();

            for (int year = startYear; year <= reportDate.Year; year++)
            {
                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    data.Add($"{year}-Q{quarter}", feedbacks.Where(f => f.ResponsibleDepartment == department.Name && f.DateReceived.Year == year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

                    if (year == reportDate.Year && quarter == reportDateQuarter)
                        break;
                }
            }

            var feedbackNatures = FeedbackNature.FeedbackNaturesForChart;

            SetChartData(chart, data, feedbackNatures, isPercentage);

            ExportChart(chart);
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackCategory_individualDepartment(Department department, int startYear, bool isPercentage = false)
        {
            Excel.Chart chart = (startYear == reportDate.Year) ?
                InitialiseChart($"Feedback category in {reportDate.Year}-Q{reportDateQuarter} for {department.Name}{(isPercentage ? " (percentage)" : "")}", isPercentage) :
                InitialiseChart($"Feedback category trend since {startYear} for {department.Name}{(isPercentage ? " (percentage)" : "")}", isPercentage);

            var data = new Dictionary<string, IList<Feedback>>();

            for (int year = startYear; year <= reportDate.Year; year++)
            {
                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    data.Add($"{year}-Q{quarter}", feedbacks.Where(f => f.ResponsibleDepartment == department.Name && f.DateReceived.Year == year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

                    if (year == reportDate.Year && quarter == reportDateQuarter)
                        break;
                }
            }

            var seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();
            foreach (var category in department.Categories)
            {
                var series = seriesCollection.NewSeries();
                series.Name = category;
                series.XValues = data.Keys.ToArray();
                if (isPercentage)
                    series.Values = data.Values.Select(v => GetRatio(v.Where(f => f.Category == category).Count(), v.Count)).ToArray();
                else
                    series.Values = data.Values.Select(v => v.Where(f => f.Category == category).Count()).ToArray();
                series.ApplyDataLabels();
                if (isPercentage)
                    series.DataLabels().NumberFormat = "0%";
            }

            ExportChart(chart);
        }

        private Excel.Chart InitialiseChart(string chartTitle, bool isPercentage)
        {
            var chart = (Excel.Chart)workbook.Charts.Add();
            chart.ApplyDataLabels();
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartType = Excel.XlChartType.xlColumnClustered;
            chart.ChartTitle.Text = chartTitle;

            var xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Year-quarter";
            var yAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = "Number of feedback";
            if (isPercentage)
            {
                yAxis.TickLabels.NumberFormat = "0%";
                yAxis.MaximumScale = 1;
            }

            return chart;
        }

        private static void SetChartData(Excel.Chart chart, Dictionary<string, IList<Feedback>> data, IEnumerable<string> feedbackNatures, bool isPercentage)
        {
            var seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();
            foreach (var feedbackNature in feedbackNatures)
            {
                var series = seriesCollection.NewSeries();
                series.Name = feedbackNature;
                series.XValues = data.Keys.ToArray();
                if (feedbackNature == FeedbackNature.TotalFeedbacks)
                {
                    if (isPercentage)
                        series.Values = data.Values.Select(v => GetRatio(v.Count, v.Count)).ToArray();
                    else
                        series.Values = data.Values.Select(v => v.Count()).ToArray();
                }
                else
                {
                    if (isPercentage)
                        series.Values = data.Values.Select(v => GetRatio(v.Where(f => f.FeedbackNature == feedbackNature).Count(), v.Count)).ToArray();
                    else
                        series.Values = data.Values.Select(v => v.Where(f => f.FeedbackNature == feedbackNature).Count()).ToArray();
                }

                series.ApplyDataLabels();
                if (isPercentage)
                    (series.DataLabels()).NumberFormat = "0%";
            }
        }

        private void ExportChart(Excel.Chart chart)
        {
            chart.Export($@"{destinationFolder}\{chartNumber} - {chart.ChartTitle.Text}.png", "PNG");
            chartNumber++;
            chart.Delete();

            ChartCreated?.Invoke(this, $"{chart.ChartTitle.Text} created");
        }

        private static double GetRatio(int value, int total)
        {
            var result = (double)value / total;

            return (total == 0) ? 0 : result;
        }
    }
}
