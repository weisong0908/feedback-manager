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

                DrawChart_FeedbackVsQuarter_per_FeedbackNature_for_CurrentYear_Bar();

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackNature_for_CurrentYear_Bar_individualDepartment(department);

                foreach (var department in departments)
                    DrawChart_FeedbackVsQuarter_per_FeedbackCategory_for_CurrentYear_Bar_individualDepartment(department);

                workbook.Close(SaveChanges: true, Filename: $@"{destinationFolder}\charts.xlsx");
                excel.Quit();
            });
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackNature_for_CurrentYear_Bar()
        {
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

            for (int quarter = 1; quarter <= reportDateQuarter; quarter++)
            {
                data.Add($"{reportDate.Year}-Q{quarter}", feedbacks.Where(f => f.DateReceived.Year == reportDate.Year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

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

            ChartCreated?.Invoke(this, $"{chart.ChartTitle.Text} created");
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackNature_for_CurrentYear_Bar_individualDepartment(Department department)
        {
            var chart = (Excel.Chart)workbook.Charts.Add();
            chart.ApplyDataLabels();
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartType = Excel.XlChartType.xlColumnClustered;
            chart.ChartTitle.Text = $"Feedback nature in {reportDate.Year} for {department.Name}";

            var xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Year-quarter";
            var yAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = "Number of feedback";

            var data = new Dictionary<string, IList<Feedback>>();

            for (int quarter = 1; quarter <= reportDateQuarter; quarter++)
            {
                data.Add($"{reportDate.Year}-Q{quarter}", feedbacks.Where(f => f.ResponsibleDepartment == department.Name && f.DateReceived.Year == reportDate.Year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

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

            ChartCreated?.Invoke(this, $"{chart.ChartTitle.Text} created");
        }

        private void DrawChart_FeedbackVsQuarter_per_FeedbackCategory_for_CurrentYear_Bar_individualDepartment(Department department)
        {
            var chart = (Excel.Chart)workbook.Charts.Add();
            chart.ApplyDataLabels();
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartType = Excel.XlChartType.xlColumnClustered;
            chart.ChartTitle.Text = $"Feedback category in {reportDate.Year} for {department.Name}";

            var xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Year-quarter";
            var yAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = "Number of feedback";

            var data = new Dictionary<string, IList<Feedback>>();

            for (int quarter = 1; quarter <= reportDateQuarter; quarter++)
            {
                data.Add($"{reportDate.Year}-Q{quarter}", feedbacks.Where(f => f.ResponsibleDepartment == department.Name && f.DateReceived.Year == reportDate.Year && ((f.DateReceived.Month + 2) / 3) == quarter).ToList());

                if (quarter == reportDateQuarter)
                    break;
            }

            var seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();

            foreach (var category in department.Categories)
            {
                var series = seriesCollection.NewSeries();
                series.Name = category;
                series.XValues = data.Keys.ToArray();
                series.Values = data.Values.Select(v => v.Where(f => f.Category == category).Count()).ToArray();
                series.ApplyDataLabels();
            }

            chart.Export($@"{destinationFolder}\{chartNumber} - {chart.ChartTitle.Text}.png", "PNG");
            chartNumber++;
            chart.Delete();

            ChartCreated?.Invoke(this, $"{chart.ChartTitle.Text} created");
        }
    }
}
