using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmbeddedJsonResourceHandler
    {
        /// <summary>
        /// Return the embedded resource as an object via filename
        /// </summary>
        /// <typeparam name="T">The object being derialized</typeparam>
        /// <param name="filename">The embedded resource filename</param>
        /// <returns>Return the embedded resource as an object</returns>
        public static T Read<T>(string filename) 
        {
            return ReadEmbeddedResource<T>(typeof(EmbeddedJsonResourceHandler).Assembly, filename);
        }

        /// <summary>
        /// Return the embedded resource as an object via assembly and filename
        /// </summary>
        /// <typeparam name="T">The object being derialized</typeparam>
        /// <param name="assembly">The assembly that contains the embedded resource</param>
        /// <param name="filename">The embedded resource filename</param>
        /// <returns>Return the embedded resource as an object</returns>
        public static T ReadEmbeddedResource<T>(Assembly assembly, string filename)
        {
            // Obtain the stream and obtain the contents
            StreamReader streamReader = GetEmbeddedStream(assembly, filename);
            string dataString = streamReader.ReadToEnd();
            streamReader.Close();

            // Return the json serialization of the embedded resource content
            return JsonConvert.DeserializeObject<T>(dataString);
        }

        /// <summary>
        /// Find and return the stream of an rmbedded resource
        /// </summary>
        /// <param name="assembly">The assembly of the embedded resources</param>
        /// <param name="filename">the embedded resources filename</param>
        /// <returns>The embedded resource as a Stream Reader</returns>
        public static StreamReader GetEmbeddedStream(Assembly assembly, string filename)
        {
            // Search through all the embedded resources
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                // When found return the resource stream
                if (resourceName.EndsWith(filename))
                {
                    return new StreamReader(assembly.GetManifestResourceStream(resourceName));
                }
            }
            
            // If nothing is found, send null
            return null;
        }
    }
}
