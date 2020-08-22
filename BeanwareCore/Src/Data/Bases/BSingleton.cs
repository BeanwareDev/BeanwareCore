using System;

namespace BeanwareCore.Src.Data.Bases
{
    /// <summary>
    /// Creates a Singleton implementation for the implementing class. Make sure to include a private ctor to prevent pattern circumvention
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    public abstract class BSingleton<T> where T : class
    {
        // Variables
        private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstance());
        public static T Get { get { return _instance.Value; } }

        // Methods
        private static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}
