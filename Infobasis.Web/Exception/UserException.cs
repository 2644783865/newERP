using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infobasis.Web
{
    public class UserException : Exception
    {
        public UserException()
        {
        }


        public UserException(string message)
            : base(message)
        { }

        public UserException(string message, Exception exception)
            : base(message, exception)
        { }
    }

}