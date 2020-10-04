// // SingleUserMatch.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 14:40 20202020 10 3
// */

namespace rules
{
    public class SingleUserMatch : UserMatch
    {
        readonly string userName;
        public SingleUserMatch(string userName)
        {
            this.userName = userName;
        }

        public override bool Match(string userId)
        {
            return userId == userName;
        }
    }


}
