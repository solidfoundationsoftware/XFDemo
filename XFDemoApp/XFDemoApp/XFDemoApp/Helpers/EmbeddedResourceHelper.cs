using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace XFDemoApp.Helpers
{
    internal static class EmbeddedResourceHelper
    {
        internal static string LoadResource(string embeddedResourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
