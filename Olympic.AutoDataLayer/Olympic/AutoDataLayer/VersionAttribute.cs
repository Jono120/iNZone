namespace Olympic.AutoDataLayer
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        [CompilerGenerated]
        private bool _autoResolveCurrentUser;
        [CompilerGenerated]
        private bool _checkForStaleData;
        [CompilerGenerated]
        private bool _keepVersionHistory;
        [CompilerGenerated]
        private string _versionHistoryTableName;

        public VersionAttribute()
        {
            this.KeepVersionHistory = false;
            this.AutoResolveCurrentUser = false;
            this.CheckForStaleData = true;
        }

        public bool AutoResolveCurrentUser
        {
            [CompilerGenerated]
            get
            {
                return this._autoResolveCurrentUser;
            }
            [CompilerGenerated]
            set
            {
                this._autoResolveCurrentUser = value;
            }
        }

        public bool CheckForStaleData
        {
            [CompilerGenerated]
            get
            {
                return this._checkForStaleData;
            }
            [CompilerGenerated]
            set
            {
                this._checkForStaleData = value;
            }
        }

        public bool KeepVersionHistory
        {
            [CompilerGenerated]
            get
            {
                return this._keepVersionHistory;
            }
            [CompilerGenerated]
            set
            {
                this._keepVersionHistory = value;
            }
        }

        public string VersionHistoryTableName
        {
            [CompilerGenerated]
            get
            {
                return this._versionHistoryTableName;
            }
            [CompilerGenerated]
            set
            {
                this._versionHistoryTableName = value;
            }
        }
    }
}

