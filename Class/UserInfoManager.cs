using System.Security;

namespace Encrypted_Notebook.Class
{
    class UserInfoManager
    {
        public static string userName;
        public static SecureString userPassword;
        public static int userID;
        public static string userActivNotebook;
        public static byte[] userSalt;
        
        // new byte[] 
        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
    }
}
