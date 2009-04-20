namespace DSASample
{
    public interface IShell
    {
        void UpdateStatusMessage(string message);
        void ShowProcessing(int seconds);
        void Show();
    }
}