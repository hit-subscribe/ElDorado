using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PersonLedgerViewModel
    {
        public string Name { get; set; }

        public IEnumerable<LedgerLineItemViewModel> LineItems { get; set; }
        public decimal Total => LineItems.Sum(p => p.Cost);
    }
}