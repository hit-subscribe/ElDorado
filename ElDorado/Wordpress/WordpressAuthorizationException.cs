using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Wordpress
{
    public class WordpressAuthorizationException : Exception
    {
        public WordpressAuthorizationException() : base() { }
        public WordpressAuthorizationException(string message) : base(message) { }
        public WordpressAuthorizationException(Exception ex) : base("Error authenticating with Wordpress", ex) { }
    }
}
