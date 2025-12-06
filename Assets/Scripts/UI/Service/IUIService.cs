namespace UI.Service
{
    public interface IUIService
    {
        T Open<T>() where T : UIWindow;
        void Close<T>() where T : UIWindow;
        T Get<T>() where T : UIWindow;
        bool IsOpen<T>() where T : UIWindow;
    }
}