using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    [NotMapped]
    public class AccountsReceivableViewModel
    {
        public IEnumerable<AuthorLedgerViewModel> AuthorLedgers { get; set; } = Enumerable.Empty<AuthorLedgerViewModel>();
        public decimal Total => AuthorLedgers.Sum(al => al.Total);
    }
}