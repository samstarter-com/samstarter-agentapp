using System.Security;

namespace SWI.SoftStock.Common
{
    /// <summary>
    /// Current user credentials
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Gets Login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login { get; private set; }

        /// <summary>
        /// Gets Password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public SecureString Password { get; private set; }

        /// <summary>
        /// Gets a value indicating whether IsGuest.
        /// </summary>
        /// <value>
        /// The is guest.
        /// </value>
        public bool IsGuest { get; private set; }

        /// <summary>
        /// Gets Type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public AuthenticationType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Credentials"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="isGuest"></param>
        /// <param name="type"></param>
        public Credentials(string login, SecureString password, bool isGuest = false, AuthenticationType type = AuthenticationType.UserName)
        {
            Login = login;
            Password = password;
            IsGuest = isGuest;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Credentials"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="type"></param>
        public Credentials(string login, AuthenticationType type = AuthenticationType.Windows)
        {
            Type = type;
            Login = login;
        }
    }
}
