namespace Parcial3.UI.Interfaces
{
    public interface IPresentator
    {
        static abstract void Clear();
        static abstract string ReadLineWithCountDown(int seconds);
        static abstract void Write(string msg);
        static abstract void WriteLine(string msg);
    }
}