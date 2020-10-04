// // InvalidRuleException.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 16:30 20202020 10 3
// */
using System;
namespace rules
{
    public class InvalidRuleException : Exception
    {
        public InvalidRuleException(string message) : base(message)
        {

        }
    }
}
