using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App
{
    /// <summary>
    ///     Class used to store and load properties.
    ///     It is used to initialize all simulation parameters.
    ///     If certain properties are absent, a default value will be used.
    /// </summary>
    /// <author>
    ///     We used the code published by Nick Rimmer
    ///     Link :
    ///     (https://stackoverflow.com/questions/485659/can-net-load-and-parse-a-properties-file-equivalent-to-java-properties-class#answer-7696370
    /// </author>
    public class Properties
    {
        private string _filename;
        private Dictionary<string, string> _list;

        public Properties(string file)
        {
            IsJustCreated = Reload(file);
        }

        public bool IsJustCreated { get; }

        /// <summary>
        /// Gets the value of the field by its name or a default value if the field does not exists. 
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="defValue">Default value</param>
        /// <returns>The value of the field or the default value if it doesn't exists</returns>
        public string Get(string field, string defValue)
        {
            return Get(field) == null ? defValue : Get(field);
        }

        /// <summary>
        /// Gets the value of the field by its name.
        /// </summary>
        /// <param name="field">Field name</param>
        /// <returns>The value of the field or null if no field found</returns>
        public string Get(string field)
        {
            return _list.ContainsKey(field) ? _list[field] : null;
        }

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <param name="field">The field name.</param>
        /// <param name="value">The new value of the field.</param>
        public void Set(string field, object value)
        {
            if (!_list.ContainsKey(field))
                _list.Add(field, value.ToString());
            else
                _list[field] = value.ToString();
        }

        /// <summary>
        /// Saves the file at its location.
        /// </summary>
        public void Save()
        {
            Save(_filename);
        }

        /// <summary>
        /// Saves the file at the given location and updates the path currently stored. 
        /// </summary>
        /// <param name="filename">The destination file path</param>
        public void Save(string filename)
        {
            _filename = filename;

            if (!File.Exists(filename))
                using(File.Create(filename))
                {
                }

            StreamWriter file = new StreamWriter(filename);

            foreach (string prop in _list.Keys.ToArray())
                if (!string.IsNullOrWhiteSpace(_list[prop]))
                    file.WriteLine(prop + "=" + _list[prop]);

            file.Close();
        }

        /// <summary>
        /// Reload the values from the file.
        /// </summary>
        public bool Reload()
        {
            return Reload(_filename);
        }

        /// <summary>
        /// Reload the values from the given file or creates the corresponding file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>True if the file had to be created, false otherwise.</returns>
        public bool Reload(string filename)
        {
            _filename = filename;
            _list = new Dictionary<string, string>();

            if (File.Exists(filename))
            {
                LoadFromFile(filename);
                return false;
            }

            using (File.Create(filename))
            {
            }

            return true;
        }

        private void LoadFromFile(string file)
        {
            foreach (string line in File.ReadAllLines(file))
                if (!string.IsNullOrEmpty(line) &&
                    !line.StartsWith(";") &&
                    !line.StartsWith("#") &&
                    !line.StartsWith("'") &&
                    line.Contains('='))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if (value.StartsWith("\"") && value.EndsWith("\"") ||
                        value.StartsWith("'") && value.EndsWith("'"))
                        value = value.Substring(1, value.Length - 2);


                    _list.Add(key, value);
                }
        }
    }
}