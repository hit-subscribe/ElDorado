using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PostLineItemViewModel
    {
        public string Title { get; set; }
        public decimal Cost { get; set; }

        public PostLineItemViewModel(BlogPost bp)
        {
            Title = bp.Title;
        }
    }
}