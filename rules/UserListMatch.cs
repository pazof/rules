// // UserListMatch.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */
using System.Collections.Generic;

namespace rules
{
    public class UserListMatch : UserMatch
    {
        readonly List<string> userNames;

        public UserListMatch(string[] userNames)
        {
            this.userNames = new List<string>(userNames);
        }

        public override bool Match(string userId)
        {
            return userNames.Contains(userId);
        }
    }


}
