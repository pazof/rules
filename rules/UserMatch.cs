// // UserMatch.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */


namespace rules
{
    public abstract class UserMatch
    {
        public abstract bool Match(string userId);

        public virtual UserMatch And(UserMatch other)
        {
            return new UserMatchIntersection(this, other);
        }

        public virtual UserMatch Or(UserMatch other)
        {
            return new UserMatchUnion(this, other);
        }

        public virtual UserMatch Not()
        {
            return new UserMatchNot(this);
        }
    }
}
