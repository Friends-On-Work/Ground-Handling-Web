namespace Ground_Handlng.Abstractions.Utility
{
    public interface ILogWriter
    {
        void CreateLog(string logText, string moduleName, string logName);
    }
}
