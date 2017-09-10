using System;

namespace FFF.Base.Util
{
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {

        public static T Instance => inst ?? (inst = new T());

        private static T inst;

        protected Singleton()
        {
            if (inst != null)
            {
                throw new SingletonConflictException();
            }
            inst = (T) this;
        }

    }

    public class SingletonConflictException : Exception
    {
        
    }
}
