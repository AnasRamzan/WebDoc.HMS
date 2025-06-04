using System.Threading.Tasks.Sources;
using WebDoc.HMS.Models;
namespace WebDoc.HMS.StateContainers
{
    public class UserStateContainer
    {
        public User _user { set; get; }
        public event Action OnStateChange;

        public void set_user(User user)
        {
            this._user = user;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChange?.Invoke();
        }
    }
}
