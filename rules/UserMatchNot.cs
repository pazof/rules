// // UserMatchNot.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */

namespace rules
{
    public class UserMatchNot : UserMatch
    {
        UserMatch inner;
        public UserMatchNot(UserMatch inner)
        {
            this.inner = inner;
        }

        public override bool Match(string userId)
        {
            return !inner.Match(userId);
        }

    }


}
