using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Encrypted_Notebook.Resources;
using System.Net;

namespace Encrypted_Notebook.Class{
    class DatabaseManager{

        EncryptionManager EMgr = new EncryptionManager();
        public static MySqlConnection con = new MySqlConnection();
        public static MySqlCommand cmd = new MySqlCommand();
        public static MySqlDataReader reader;

        public void connectionString(string dbIp, string dbName, string dbUser, string dbPassword){
            try { con.ConnectionString = $"Server={dbIp};Database={dbName};User Id={dbUser};Password={dbPassword};"; }
            catch { }
        }

        public string dbConnect(){
            try{
                cmd.Connection = con;
                con.Open();
                if (con.State.ToString() != "Open")
                    dbError();
            }
            catch (Exception ex){
                return ex.Message;
            }

            return "successfully connected to the database!";
        }
        public string dbDisconnect(){
            try{
                con.Close();
            }
            catch (Exception ex){
                return ex.Message;
            }

            return "successfully disconnected from the database!";
        }
        public string dbError(){
            return "An error occurred while connecting to the database! (Code: 404)";
        }

        public int checkIfUserExist(string userName){
            cmd.CommandText = ($"SELECT user_Username FROM user WHERE (user_Username = '{userName}')");
            try{
                if (cmd.ExecuteScalar().ToString() == userName)
                    return 1;
                else
                    return 0;
            }
            catch { return 0; }
        }
        public int checkIfServerIsConfigured(){
            cmd.CommandText = ("SELECT setting_Value FROM setting WHERE (setting_Name = 'IsConfigured');");
            try{
                cmd.ExecuteScalar();
                return 1;
            }
            catch { return 0; }
        }
        public int ConfiguredServer(){
            try{
                cmd.CommandText = SQL.ServerTableScript;
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch { return 0; }
        }

        public void createUser(string userName, string userPassword){
            cmd.CommandText = ($"INSERT INTO `user` (`user_Username`, `user_Password`) VALUES ('{userName.ToLower()}', '{EMgr.GetHash_SHA512(userPassword)}');");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ($"INSERT INTO `salt` ( `salt_Value`) VALUES ('{SplitManager.SplitByteArrayIntoString(EMgr.GetNewSalt())}');");
            cmd.ExecuteNonQuery();
        }
        public void deleteUser(){
            cmd.CommandText = ("SET SQL_SAFE_UPDATES = 0;");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ($"DELETE FROM `user` WHERE (`user_ID` = '{UserInfoManager.userID}');");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ($"DELETE FROM `salt` WHERE (`salt_ID` = '{UserInfoManager.userID}');");
            cmd.ExecuteNonQuery();
            cmd.CommandText = ($"DELETE FROM `notebooks` WHERE (`notebook_Owner` = '{UserInfoManager.userID}');");
            cmd.ExecuteNonQuery();
        }
        public bool loginUser(string userName, string userPassword){
            cmd.CommandText = ($"SELECT user_ID FROM user WHERE (user_Username = '{userName}' and user_Password = '{EMgr.GetHash_SHA512(userPassword)}');");
            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0){
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

        public List<string> loadNotebooks(){
            List<string> Notebooks = new List<string>();

            cmd.CommandText = ($"SELECT notebook_Name FROM notebooks WHERE (notebook_Owner = '{UserInfoManager.userID}');");

            reader = cmd.ExecuteReader();
            while (reader.Read())
                Notebooks.Add(EMgr.DecryptAES256Salt((reader["notebook_Name"].ToString()), new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt));
            reader.Close();

            return Notebooks;
        }
        public void createNotebook(string notebookName){
            cmd.CommandText = ($"SELECT notebook_Name FROM notebooks WHERE (notebook_Name = '{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt)}' AND notebook_Owner = '{UserInfoManager.userID}');");
            if (cmd.ExecuteScalar() == null)
            {
                cmd.CommandText = ($"INSERT INTO notebooks (`notebook_Owner`, `notebook_Name`) VALUES ('{UserInfoManager.userID}', '{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt)}');");
                cmd.ExecuteNonQuery();
            }
        }
        public void deleteNotebook(string notebookName){
            cmd.CommandText = ($"DELETE FROM notebooks WHERE (`notebook_Name` = '{EMgr.EncryptAES256Salt(notebookName, new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt)}' AND notebook_Owner = '{UserInfoManager.userID}');");
            cmd.ExecuteNonQuery();
        }

        public void writeNotes(string notes){
            if (notes != "" || notes != null)
                cmd.CommandText = ($"UPDATE notebooks SET `notebook_Value` = '{EMgr.EncryptAES256Salt(notes, new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt)}' WHERE (`notebook_Name` = '{UserInfoManager.userActivNotebook}' AND notebook_Owner = '{UserInfoManager.userID}');");
            else
                cmd.CommandText = ($"UPDATE notebooks SET `notebook_Value` = '{notes}' WHERE (`notebook_Name` = '{UserInfoManager.userActivNotebook}' AND notebook_Owner = '{UserInfoManager.userID}');");
            cmd.ExecuteNonQuery();
        }
        public string readNotes(){
            try{
                cmd.CommandText = ($"SELECT notebook_Value FROM notebooks WHERE (notebook_Name = '{UserInfoManager.userActivNotebook}' AND notebook_Owner = '{UserInfoManager.userID}');");
                string notes = cmd.ExecuteScalar().ToString();
                if (notes == "" || notes == null)
                    return notes;
                else
                    return EMgr.DecryptAES256Salt(notes, new NetworkCredential("", UserInfoManager.userPassword).Password, UserInfoManager.userSalt);
            }
            catch { return ""; }
        }

        public string GetSalt(){
            cmd.CommandText = ($"SELECT salt_Value FROM salt WHERE (salt_ID = '{UserInfoManager.userID}');");
            return cmd.ExecuteScalar().ToString();
        }

        public List<string> ExportAllNotebooks(string exportPassword){
            List<string> exportData = new List<string>();
            byte[] salt = EMgr.GetNewSalt();
            exportData.Add(SplitManager.SplitByteArrayIntoString(salt));
            string new_EncryptedNotebookValue, new_EncryptedNotebookName;

            cmd.CommandText = ($"SELECT notebook_Name,notebook_Value FROM notebooks WHERE (notebook_Owner = {UserInfoManager.userID});");

            reader = cmd.ExecuteReader();
            while (reader.Read()){
                new_EncryptedNotebookName = 
                    EMgr.EncryptAES256Salt(
                        EMgr.DecryptAES256Salt(
                            reader["notebook_Name"].ToString(), 
                            new NetworkCredential("", UserInfoManager.userPassword).Password, 
                            UserInfoManager.userSalt),
                        exportPassword, 
                        salt);

                if (reader["notebook_Value"].ToString() == null || reader["notebook_Value"].ToString() == ""){
                    new_EncryptedNotebookValue = "NULL";
                }
                else{
                    new_EncryptedNotebookValue =
                        EMgr.EncryptAES256Salt(
                            EMgr.DecryptAES256Salt(
                                reader["notebook_Value"].ToString(),
                                new NetworkCredential("", UserInfoManager.userPassword).Password,
                                UserInfoManager.userSalt),
                        exportPassword,
                        salt);
                }
                exportData.Add($"{new_EncryptedNotebookName}:{new_EncryptedNotebookValue}");
            }
            reader.Close();
            return exportData;
        }
        public List<string> ExportCustomNotebooks(string exportPassword, List<string> listOfNotebooks){
            List<string> exportData = new List<string>();
            byte[] salt = EMgr.GetNewSalt();
            exportData.Add(SplitManager.SplitByteArrayIntoString(salt));
            string new_EncryptedNotebookValue, new_EncryptedNotebookName;

            foreach (var notebook in listOfNotebooks){
                cmd.CommandText = ($"SELECT notebook_Name,notebook_Value FROM notebooks WHERE (notebook_Owner = {UserInfoManager.userID} and notebook_Name = '{EMgr.EncryptAES256Salt(notebook, new NetworkCredential("", UserInfoManager.userPassword).Password,UserInfoManager.userSalt)}');");

                reader = cmd.ExecuteReader();
                while (reader.Read()){
                    new_EncryptedNotebookName =
                        EMgr.EncryptAES256Salt(
                            EMgr.DecryptAES256Salt(
                                reader["notebook_Name"].ToString(),
                                new NetworkCredential("", UserInfoManager.userPassword).Password,
                                UserInfoManager.userSalt),
                            exportPassword,
                            salt);

                    if (reader["notebook_Value"].ToString() == null || reader["notebook_Value"].ToString() == ""){
                        new_EncryptedNotebookValue = "NULL";
                    }
                    else{
                        new_EncryptedNotebookValue =
                            EMgr.EncryptAES256Salt(
                                EMgr.DecryptAES256Salt(
                                    reader["notebook_Value"].ToString(),
                                    new NetworkCredential("", UserInfoManager.userPassword).Password,
                                    UserInfoManager.userSalt),
                            exportPassword,
                            salt);
                    }

                    exportData.Add($"{new_EncryptedNotebookName}:{new_EncryptedNotebookValue}");
                }
                reader.Close();    
            }
            return exportData;
        }
        public string ImportAllNotebooks(string importPassword, List<string> importData){
            try{
                byte[] salt = SplitManager.SplitStringIntoByteArray(importData[0]);
                importData.RemoveAt(0);
                string new_EncryptedNotebookValue = null, new_EncryptedNotebookName = null;

                foreach (var notebook in importData){
                    new_EncryptedNotebookName = null;
                    new_EncryptedNotebookValue = null;

                    string[] _tmp = notebook.Split(':');

                    new_EncryptedNotebookName =
                        EMgr.EncryptAES256Salt(
                            EMgr.DecryptAES256Salt(_tmp[0], importPassword, salt),
                            new NetworkCredential("", UserInfoManager.userPassword).Password,
                            UserInfoManager.userSalt);

                    if (_tmp[1] == "NULL")
                        new_EncryptedNotebookValue = null;
                    else{
                        new_EncryptedNotebookValue =
                            EMgr.EncryptAES256Salt(
                                EMgr.DecryptAES256Salt(_tmp[1], importPassword, salt),
                                new NetworkCredential("", UserInfoManager.userPassword).Password,
                                UserInfoManager.userSalt);
                    }

                    cmd.CommandText = ($"INSERT INTO notebooks (`notebook_Owner`, `notebook_Name`, `notebook_Value`) VALUES ('{UserInfoManager.userID}', '{new_EncryptedNotebookName}', '{new_EncryptedNotebookValue}');");
                    cmd.ExecuteNonQuery();
                }
                return "";
            }
            catch { return null; }
         }
    }
}