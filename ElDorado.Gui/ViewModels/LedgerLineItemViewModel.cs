using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class LedgerLineItemViewModel
    {
        public string Title { get; set; }
        public decimal Cost { get; set; }

        public LedgerLineItemViewModel(BlogPost bp)
        {
            Title = bp.Title;
        }
    }
}