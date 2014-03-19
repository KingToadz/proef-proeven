using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace proef_proeven.Components.Util
{
    class IOHelper
    {
        static IOHelper instance;
        public static IOHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOHelper();
                }
                return instance;
            }
        }

        /// <summary>
        /// Check is an file exists
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>If the file exists</returns>
        public bool DoesFileExist(string path)
        {
            return File.Exists(System.Environment.CurrentDirectory + path);
        }

        /// <summary>
        /// Cehcks if an directory exists
        /// </summary>
        /// <param name="path">the path to the directory</param>
        /// <returns>If the directory exists it will return true</returns>
        public bool DoesDirectoryExists(string path)
        {
            return Directory.Exists(System.Environment.CurrentDirectory + path);
        }

        /// <summary>
        /// This will create an directory if it doesn't exist
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <returns>If the directory was created. Will also return true if it already exists</returns>
        public bool CreateDirectory(string path)
        {
            try
            {
                if (!DoesDirectoryExists(System.Environment.CurrentDirectory + path))
                    Directory.CreateDirectory(System.Environment.CurrentDirectory + path);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates or overwrites a file
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <param name="data">Data to put in the file</param>
        /// <returns>If it succeed to write to file</returns>
        public bool WriteFile(string path, string data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(System.Environment.CurrentDirectory + path)))
                {
                    //string[] lines = data.Split('\r');

                    //for (int i = 0; i < lines.Length; i++ )
                    sw.Write(data);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Will read the requested file if it exists
        /// </summary>
        /// <param name="path">the path of the file to read</param>
        /// <returns>The data in the file or ""</returns>
        public string ReadFile(string path)
        {
            string data = "";

            if (DoesFileExist(path))
            {
                using (StreamReader sr = new StreamReader(System.Environment.CurrentDirectory + path))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        data += line;
                    }
                }
            }

            return data;
        }
    }
}
