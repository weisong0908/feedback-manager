namespace FeedbackManager.WPF.Services
{
    public interface IWindowService
    {
        void ShowChartsGeneratorWindow();
        void ShowMessageBox(string text, string caption);
    }
}