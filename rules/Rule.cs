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

    public class AllowingRule : Rule
    {
        UserMatch exp;
        public AllowingRule(UserMatch exp)
        {
            this.exp = exp;
        }
        public override bool Allow(string username)
        {
            return exp.Match(username);
        }

        public override bool Deny(string username)
        {
            return false;
        }
    }

    public class DenyingRule : Rule
    {
        UserMatch exp;
        public DenyingRule(UserMatch exp)
        {
            this.exp = exp;
        }

        public override bool Allow(string username)
        {
            return false;
        }

        public override bool Deny(string username)
        {
            return exp.Match(username);
        }
    }

    public abstract class RuleSet : Rule
    {
        public abstract void Add(Rule rule);
        public abstract void Clear();
    }

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
