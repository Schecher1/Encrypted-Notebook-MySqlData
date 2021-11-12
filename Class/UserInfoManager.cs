using System.Security;

namespace Encrypted_Notebook.Class{
    class UserInfoManager{
        public static string userName;
        public static SecureString userPassword;
        public static int userID;
        public static string userActivNotebook;
        public static byte[] userSalt;

        public static void userLogout(){
            userName = null;
            userActivNotebook = null;
            userID = -1;
            userSalt = new byte[] { 0 };
            userPassword.Clear();
            System.GC.Collect();
        }
    }
}
