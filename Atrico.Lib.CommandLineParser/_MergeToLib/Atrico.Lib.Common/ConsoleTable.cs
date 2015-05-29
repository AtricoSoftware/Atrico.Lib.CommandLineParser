using System;
using System.Collections.Generic;
using System.Text;

namespace Atrico.Lib.Common.Console
{
    /// <summary>
    ///     Write a table to the console
    /// </summary>
    public class ConsoleTable
    {
        private readonly IList<IList<object>> _columns = new List<IList<object>>();

        public int Rows
        {
            get { return _columns.Count == 0 ? 0 : _columns[0].Count; }
        }

        public int Columns
        {
            get { return _columns.Count; }
        }

        /// <summary>
        ///     Appends the row at the bottom of table
        /// </summary>
        /// <param name="row">The items in the row</param>
        public void AppendRow(params object[] row)
        {
            // Ensure there are enough columns
            while (_columns.Count < row.Length)
            {
                var column = new List<object>();
                _columns.Add(column);
                // Pad new column for existing rows
                while (column.Count < _columns[0].Count) column.Add(null);
            }
            // Convert rows to columns
            for (var i = 0; i < row.Length; ++i)
            {
                _columns[i].Add(row[i]);
            }
        }

        /// <summary>
        ///     Tabulates this instance as a list of lines of text
        /// </summary>
        /// <returns>multiple lines of text</returns>
        public IEnumerable<string> Tabulate()
        {
            if (_columns.Count == 0) return new String[] {};
            // Calculate max width of each column
            var width = new List<int>();
            foreach (var column in _columns)
            {
                var colWidth = 0;
                for (var row = 0; row < _columns[0].Count; ++row)
                {
                    var len = column[row].ToString().Length;
                    if (len > colWidth) colWidth = len;
                }
                width.Add(colWidth);
            }
            // Output columns
            var lines = new List<string>();
            for (var row = 0; row < _columns[0].Count; ++row)
            {
                var line = new StringBuilder();
                for (var column = 0; column < _columns.Count; ++column)
                {
                    if (column > 0) line.Append(' ');
                    if (_columns[column][row] != null) line.AppendFormat("{0,-"+width[column]+"}", _columns[column][row]);
                }
                lines.Add(line.ToString());
            }
            return lines;
        }
    }

    // TODO
    // Add borders
    // Add headers
    // Number rows
    // Remove empty columns/rows
    // Parameterise spacing
}