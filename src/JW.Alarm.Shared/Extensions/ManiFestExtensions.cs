using System.Reflection;

namespace JW.Alarm
{
    public static class ManifestExtensions
    {

        /// <summary>
        /// Get a resource file embedded with the given assembly manifest
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetManifestResourceFile(string fileName)
        {
            var assembly = typeof(ManifestExtensions).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(string.Format("JW.Alarm.Shared.Resources.{0}", fileName.Replace("-", "_")));

            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine(">>> " + res);
            }
            using (var reader = new System.IO.StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
 
    }
}
