// // Rule.cs
// /*
// Paul Schneider paul@pschneider.fr 04/10/2020 17:44 20202020 10 4
// */
using System.Collections.Generic;

namespace rules
{
    public class RuleSetDefault : RuleSet
    {
        List<Rule> innerRules = new List<Rule>();
        bool defaultsToAllow;

        public RuleSetDefault(bool defaultsToAllow = false)
        {
            this.defaultsToAllow = defaultsToAllow;
        }

        public override void Add(Rule rule)
        {
            innerRules.Add(rule);
        }

        public override bool Allow(string username)
        {
            foreach (var r in innerRules)
            {
                if (r.Allow(username))
                    return true;
                if (r.Deny(username))
                    return false;
            }
            return defaultsToAllow;
        }

        public override void Clear()
        {
            innerRules.Clear();
        }

        public override bool Deny(string username)
        {
            foreach (var r in innerRules)
            {
                if (r.Deny(username))
                    return true;

                if (r.Allow(username))
                    return false;
            }
            return !defaultsToAllow;
        }
    }



}
