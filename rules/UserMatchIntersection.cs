// // UserMatchAnd.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */

using System.Collections.Generic;
using System.Linq;

namespace rules
{
    public class UserMatchIntersection : UserMatch
    {
        private List<UserMatch> userMatch = new List<UserMatch>();

        public UserMatchIntersection()
        {
        }

        public UserMatchIntersection(UserMatch userMatch, UserMatch other)
        {
            this.userMatch.Add(userMatch);
            this.userMatch.Add(other);
        }

        public override bool Match(string userId)
        {
            return userMatch.All(m => m.Match(userId));
        }

        public override UserMatch And(UserMatch other)
        {
            userMatch.Add(other);
            return this;
        }

    }

}
