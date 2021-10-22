using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Encrypted_Notebook.Resources;
using System.Net;

namespace Encrypted_Notebook.Class
{
    class DatabaseManager
    {
        EncryptionManager EMgr = new EncryptionManager();
        public static MySqlConnection con = new MySqlConnection();
        public static MySqlCommand cmd = new MySqlCommand();
        public static MySqlDataReader reader;

        public void connectionString(string dbIp, string dbName, string dbUser, string dbPassword)
        {
            try { con.ConnectionString = $"Server={dbIp};Database={dbName};User Id={dbUser};Password={dbPassword};"; }
            catch { }
        }

        public string dbConnect()
        {
            try
            {
                cmd.Connection = con;
                con.Open();
                if (con.State.ToString() != "Open")
                    dbError();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "successfully connected to the database!";
        }
        public string dbDisconnect()
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "successfully disconnected from the database!";
        }
        public string dbError()
        {
            return "An error occurred while connecting to the database! (Code: 404)";
        }

        public int checkIfUserExist(string userName)
        {
            cmd.CommandText = ($"SELECT user_Username FROM user WHERE (user_Username = '{userName}')");
            try
            {
                if (cmd.ExecuteScalar().ToString() == userName)
                    return 1;
                else
                    return 0;
            }
            catch { return 0; }
        }
        public int checkIfServerIsConfigured()
        {
            cmd.CommandText = ("SELECT setting_Value FROM setting WHERE (setting_Name = 'IsConfigured');");
            try
            {
                cmd.ExecuteScalar();
                return 1;
            }
            catch { return 0; }
        }
        public int ConfiguredServer()
        {
            try
            {
                cmd.CommandText = SQL.ServerTableScript;
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch { return 0; }
        }

        public void createUser(string userName, string userPassword)
        {
            cmd.CommandText = ($"INSERT INTO `user` (`user_Username`, `user_Password`) VALUES ('{userName.ToLower()}', '{EMgr.GetHash_SHA512(userPassword)}');");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ($"INSERT INTO `salt` ( `salt_Value`) VALUES ('{SplitManager.SplitByteArrayIntoString(EMgr.GetNewSalt())}');");
            cmd.ExecuteNonQuery();
            cmd.CommandText = (SQL.UserTableScript_Start + userName.ToLower() + SQL.UserTableScript_End);
            cmd.ExecuteNonQuery();
        }
        public bool loginUser(string userName, string userPassword)
        {
            cmd.CommandText = ($"SELECT user_ID FROM user WHERE (user_Username = '{userName}' and user_Password = '{EMgr.GetHash_SHA512(userPassword)}');");
            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
            {
                cmd.CommandText = ($"SELECT user_ID FROM user WHERE (user_Username = '{userName}' and user_Password = '{EMgr.GetHash_SHA512(userPassword)}');");

                UserInfoManager.userName = userName.ToLower();
                UserInfoManager.userID = Convert.ToInt32(cmd.ExecuteScalar());
                UserInfoManager.userPassword = new NetworkCredential("", userPassword).SecurePassword;
                UserInfoManager.userSalt = SplitManager.SplitStringIntoByteArray(GetSalt());

                return true;
            }  
            else
                return false;
        }

        public List<string> loadNotebooks()
        {
            List<string> Notebooks = new List<string>();

            cmd.CommandText = ($"SELECT notebook_Name FROM `notebooks from {UserInfoManager.userName}`;");
            
            reader = cmd.ExecuteReader();
            while (reader.Read())
                Notebooks.Add(EMgr.DecryptAES256Salt((reader["notebook_Name"].ToString()), new NetworkCredential("", UserInfoManager.userPassword).Password));
            reader.Close();

            return Notebooks;
        }
        public void createNotebook(string notebookName)
        {
            cmd.CommandText = ($"SELECT notebook_Name FROM `notebooks from {UserInfoManager.userName}` WHERE (notebook_Name = '{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password)}');");
            if (cmd.ExecuteScalar() == null)
            {
                cmd.CommandText = ($"INSERT INTO `notebooks from {UserInfoManager.userName}` (`notebook_Name`) VALUES ('{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password)}');");
                cmd.ExecuteNonQuery();
            }
        }
        public void deleteNotebook(string notebookName)
        {
            cmd.CommandText = ($"DELETE FROM `notebooks from {UserInfoManager.userName}` WHERE (`notebook_Name` = '{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password)}');");
            cmd.ExecuteNonQuery();
        }

        public void writeNotes(string notes)
        {
            if (notes != "" || notes != null)
                cmd.CommandText = ($"UPDATE `notebooks from {UserInfoManager.userName}` SET `notebook_Value` = '{EMgr.EncryptAES256Salt(notes, new NetworkCredential("", UserInfoManager.userPassword).Password)}' WHERE (`notebook_Name` = '{UserInfoManager.userActivNotebook}');");
            else
                cmd.CommandText = ($"UPDATE `notebooks from {UserInfoManager.userName}` SET `notebook_Value` = '{notes}' WHERE (`notebook_Name` = '{UserInfoManager.userActivNotebook}');");
            cmd.ExecuteNonQuery();
        }
        public string readNotes()
        {
            try
            {
                cmd.CommandText = ($"SELECT notebook_Value FROM `notebooks from {UserInfoManager.userName}` where (notebook_Name = '{UserInfoManager.userActivNotebook}');");
                string notes = cmd.ExecuteScalar().ToString();
                if (notes == "" || notes == null)
                    return notes;
                else
                    return EMgr.DecryptAES256Salt(notes, new NetworkCredential("", UserInfoManager.userPassword).Password);
            }
            catch { return ""; }
        }

        public string GetSalt()
        {
            cmd.CommandText = ($"SELECT salt_Value FROM salt WHERE (salt_ID = '{UserInfoManager.userID}');");
            return cmd.ExecuteScalar().ToString();
        }
    }
}