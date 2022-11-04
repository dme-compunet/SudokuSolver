using Compunet.SudokuSolver.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Services
{
    public class SettingsService : ISettingsService
    {
        private const char separator = ':';
        private readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "compunet.sudokusolver");

        public int ActivatedTheme { get; set; }
        public string? SudokuBoard { get; set; }

        public Task Save()
        {
            StringBuilder builder = new();

            foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object? value = property.GetValue(this);

                if (value is null)
                    continue;

                builder.Append(property.Name);
                builder.Append(separator);
                builder.Append(value);

                builder.AppendLine();
            }

            string content = builder.ToString();
            File.WriteAllText(filename, content);

            return Tasks.Empty;
        }

        public Task Load()
        {
            if (File.Exists(filename))
            {
                Type type = GetType();
                string[] lines = File.ReadAllLines(filename);

                foreach (var line in lines)
                {
                    string[] parts = line.Split(separator);
                    PropertyInfo? property = type.GetProperty(parts[0]);

                    if (property is null)
                        continue;

                    object? value = Convert.ChangeType(parts[1], property.PropertyType);
                    property.SetValue(this, value);
                }
            }

            return Tasks.Empty;
        }
    }
}
