using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    [NotMapped]
    public class AccountsPayableViewModel
    {
        public IEnumerable<PersonLedgerViewModel> AuthorLedgers { get; set; } = Enumerable.Empty<PersonLedgerViewModel>();

        public IEnumerable<PersonLedgerViewModel> EditorLedgers { get; set; } = Enumerable.Empty<PersonLedgerViewModel>();
        
        public decimal AuthorTotal => AuthorLedgers.Sum(al => al.Total);

        public decimal EditorTotal => EditorLedgers.Sum(el => el.Total);

        public decimal GrandTotal => AuthorTotal + EditorTotal;
    }
}