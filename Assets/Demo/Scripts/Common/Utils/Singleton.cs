namespace PFF.Common
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;

        private static readonly object _lock = new object();

        protected Singleton()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}