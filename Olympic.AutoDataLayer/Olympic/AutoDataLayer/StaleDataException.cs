namespace Olympic.AutoDataLayer
{
    using System;

    public class StaleDataException : Exception
    {
        public StaleDataException() : base("Stale data was detected when saving an object to the database.")
        {
        }

        public StaleDataException(string message) : base(message)
        {
        }

        public StaleDataException(Type dataType, int version) : base(string.Format("Stale data was detected when saving an object of type {0}.  Expected version: {1}", dataType.Name, version))
        {
        }
    }
}

