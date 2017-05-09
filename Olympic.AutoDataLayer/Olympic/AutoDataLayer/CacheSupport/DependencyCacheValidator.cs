namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;

    public class DependencyCacheValidator : CacheValidator
    {
        private Cache _dependency;

        public DependencyCacheValidator(Cache dependency)
        {
            this._dependency = dependency;
            this._dependency.Cleared += new EventHandler(this.Dependency_Cleared);
        }

        private void Dependency_Cleared(object sender, EventArgs e)
        {
            base.OnInvalid();
        }

        public override void Validate()
        {
            this._dependency.Validate();
        }
    }
}

