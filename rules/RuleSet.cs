// // Rule.cs
// /*
// Paul Schneider paul@pschneider.fr 04/10/2020 17:44 20202020 10 4
// */

namespace rules
{
    public abstract class RuleSet : Rule
    {
        public abstract void Add(Rule rule);
        public abstract void Clear();
    }
}
