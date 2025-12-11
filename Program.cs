namespace ZooApp
{
    internal static class Program
    {
        // Einstiegspunkt der Anwendung
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
