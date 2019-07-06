using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PersonLedgerViewModel
    {
        public string Name { get; set; }

        public IEnumerable<PostLineItemViewModel> Posts { get; set; }
        public decimal Total => Posts.Sum(p => p.Cost);
    }
}