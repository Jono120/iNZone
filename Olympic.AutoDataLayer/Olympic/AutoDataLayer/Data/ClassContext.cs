namespace Olympic.AutoDataLayer.Data
{
    using System;

    internal class ClassContext
    {
        private DatabaseAction _action;
        private Type _classType;
        private object _sender;

        public ClassContext(object sender, DatabaseAction action, Type classType)
        {
            this._sender = sender;
            this._action = action;
            this._classType = classType;
        }

        public DatabaseAction Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        public Type ClassType
        {
            get
            {
                return this._classType;
            }
            set
            {
                this._classType = value;
            }
        }

        public object Sender
        {
            get
            {
                return this._sender;
            }
            set
            {
                this._sender = value;
            }
        }
    }
}

