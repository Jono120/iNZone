namespace Olympic.AutoDataLayer.Tools
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    internal class ResourceAccessor
    {
        private static Hashtable _resourceNameLookup = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        private static string[] _resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        public static string GetText(string resourceName)
        {
            string name = ResolveResourceName(resourceName);
            TextReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
            return reader.ReadToEnd();
        }

        private static string ResolveResourceName(string name)
        {
            if (_resourceNameLookup.ContainsKey(name))
            {
                return (string) _resourceNameLookup[name];
            }
            foreach (string str in _resources)
            {
                if (str.ToLower().EndsWith(name.ToLower()))
                {
                    _resourceNameLookup[name] = str;
                    return str;
                }
            }
            return null;
        }
    }
}

