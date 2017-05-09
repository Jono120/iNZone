namespace Olympic.AutoDataLayer.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class ResolveCurrentUserEventArgs : EventArgs
    {
        [CompilerGenerated]
        private string _user;

        public string User
        {
            [CompilerGenerated]
            get
            {
                return this._user;
            }
            [CompilerGenerated]
            set
            {
                this._user = value;
            }
        }
    }
}

