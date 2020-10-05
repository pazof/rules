// // Rule.cs
// /*
// Paul Schneider paul@pschneider.fr 04/10/2020 17:44 20202020 10 4
// */
using System;
using System.Collections.Generic;

namespace rules
{
    public abstract class Rule
    {
        public Rule()
        {
        }

        public abstract bool Allow(string username);
        public abstract bool Deny(string username);
    }
}
