namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;

    public class TimeCacheValidator : CacheValidator
    {
        private DateTime _cacheTime;
        public TimeInterval Interval;

        public TimeCacheValidator()
        {
            this._cacheTime = DateTime.Now;
            this.Interval = TimeInterval.Day;
        }

        public TimeCacheValidator(TimeInterval interval)
        {
            this._cacheTime = DateTime.Now;
            this.Interval = TimeInterval.Day;
            this.Interval = interval;
        }

        public override void Validate()
        {
            bool flag = true;
            DateTime now = DateTime.Now;
            switch (this.Interval)
            {
                case TimeInterval.Minute:
                    flag = (((this._cacheTime.Minute == now.Minute) && (this._cacheTime.Hour == now.Hour)) && ((this._cacheTime.Day == now.Day) && (this._cacheTime.Month == now.Month))) && (this._cacheTime.Year == now.Year);
                    break;

                case TimeInterval.Hour:
                    flag = (((this._cacheTime.Hour == now.Hour) && (this._cacheTime.Day == now.Day)) && (this._cacheTime.Month == now.Month)) && (this._cacheTime.Year == now.Year);
                    break;

                case TimeInterval.Day:
                    flag = ((this._cacheTime.Day == now.Day) && (this._cacheTime.Month == now.Month)) && (this._cacheTime.Year == now.Year);
                    break;
            }
            if (!flag)
            {
                this._cacheTime = now;
                base.OnInvalid();
            }
        }

        public enum TimeInterval
        {
            Minute,
            Hour,
            Day
        }
    }
}

