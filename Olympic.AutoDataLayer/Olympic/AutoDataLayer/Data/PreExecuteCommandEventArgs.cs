namespace Olympic.AutoDataLayer.Data
{
    using System;

    public class PreExecuteCommandEventArgs : EventArgs
    {
        private DatabaseAction _action;
        private bool _changed;
        private Type _classType;
        private string _commandText;

        public PreExecuteCommandEventArgs(Type classType, DatabaseAction action, string commandText)
        {
            this._classType = classType;
            this._commandText = commandText;
            this._action = action;
        }

        public DatabaseAction Action
        {
            get
            {
                return this._action;
            }
        }

        public bool Changed
        {
            get
            {
                return this._changed;
            }
            set
            {
                this._changed = value;
            }
        }

        public Type ClassType
        {
            get
            {
                return this._classType;
            }
        }

        public string CommandText
        {
            get
            {
                return this._commandText;
            }
            set
            {
                if (this._commandText != value)
                {
                    this._commandText = value;
                    this._changed = true;
                }
            }
        }
    }
}

