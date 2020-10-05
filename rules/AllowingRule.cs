// // Rule.cs
// /*
// Paul Schneider paul@pschneider.fr 04/10/2020 17:44 20202020 10 4
// */

namespace rules
{
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
}
