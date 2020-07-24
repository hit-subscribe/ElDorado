using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.WritingCalendar
{
    public class GoogleSheetRow
    {
        private readonly IList<object> _row;

        public int Count => _row.Count;

        public GoogleSheetRow(IList<object> row)
        {
            _row = row;
        }

        public string Item(int index)
        {
            if (index >= Count)
                return string.Empty;

            return _row[index]?.ToString();
        }

        public DateTime? ItemAsDate(int index)
        {
            return Item(index).SafeToDateTime();
        }

        public bool ItemAsBool(int index)
        {
            return Item(index) == "Yes";
        }

        public int ItemAsInt(int index)
        {
            return string.IsNullOrEmpty(Item(index)) ? 0 : int.Parse(Item(index));
        }
    }
}
