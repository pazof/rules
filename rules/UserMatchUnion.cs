// // UserMatchOr.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */

using System.Collections.Generic;
using System.Linq;

namespace rules
{
    public class UserMatchUnion : UserMatch
    {
        protected List<UserMatch> userMatch = new List<UserMatch>();

        public UserMatchUnion()
        {
        }

        public UserMatchUnion(UserMatch userMatch, UserMatch other)
        {
            this.userMatch.Add(userMatch);
            this.userMatch.Add(other);
        }

        public override bool Match(string userId)
        {
            return userMatch.Any(m => m.Match(userId));
        }

        public override UserMatch Or(UserMatch other)
        {
            userMatch.Add(other);
            return this;
        }
    }


}
