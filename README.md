<h1>Archive Reason: It should be work, but it is using "MySqlData" and not "Entity-Framework"!</h1>

<h1>Encrypted Notebook (written in WPF C#)  </h1>

## Important Info!
This is a project of mine, which I have released for further 
development or similar, i.e. I am not liable if important information 
of yours remains encrypted forever! You are responsible for yourselves!


## Application description:

This is not a normal notebook, this is an encrypted notebook 
where you are the master of your data. Because you have to 
host your own DB-Server (Maria/MySQL)! And don't forget, all 
important data is hashed or encrypted, with SHA512 or AES256 (with salt).


## How to Set-Up the Database:
Don't worry, it's not difficult, only the following data are requested 
at the start of the program (Server-Ip, Username, Password and a 
database name). Just enter them and press the button. Everything 
will be automatically set up for you and then you can directly create a user.


## How to Download:

Go to the "DOWNLOAD" folder and download any version (for security the latest one). 
Or [press here](https://github.com/Schecher1/Encrypted-Notebook-RawSQL/raw/master/DOWNLOAD/Latest%20Version.zip) to download if you want the latest one


## Features:

✔️ Beautiful and simple UI			                                                                                                    	<br/>
✔️ automatic setup							                                                                                                      <br/>
✔️ user-friendly / beginner-friendly	                                                                                                <br/>
✔️ encrypted or hashed                                                                                                                <br/>

## Images:
### Server-Login:
-If there is no password just leave it blank (I don't recommend this)                                                                 <br/>
-The "Save" button saves everything except the password, because it will be saved unencrypted in a file (because of the security)     <br/>
![Server-Login](IMAGES/Version%202.2.0.0/ServerLogin.PNG)

### Server-Login-OneWayLogin:                                                                                
![Server-Configure](IMAGES/Version%202.2.0.0/ServerLoginOneWayLogin.PNG)

### Server-Create-OneWayLoginKey:                                                                                   
![Server-Configure](IMAGES/Version%202.2.0.0/ServerCreateOneWayLoginKey.PNG)	

### Server-Configure:
-This appears only once when the database is new!                                                                                     <br/>
![Server-Configure](IMAGES/Version%201.0.0.0/ServerConfigure.PNG)


### User-Create:
![User-Create](IMAGES/Version%201.0.0.0/UserCreate.PNG)


### User-Login:
![User-Login](IMAGES/Version%201.0.0.0/UserLogin.PNG)


### User-Home:
![User-Home](IMAGES/Version%202.1.0.0/UserHomeMenu.PNG)


### Import-Export:
-All or specific notebooks can be exported																								<br/>
-The file is of course also re-encrypted																									<br/>
-DO NOT EDIT THE FILE OR EVERYTHING WILL BE GONE																			<br/>
![Import-Export](IMAGES/Version%202.1.0.0/NotebookExportImport.PNG)


### User-Delete:
-Do it carefully! it cannot be undone!																										<br/>
![User-Delete](IMAGES/Version%202.1.0.0/UserDelete.PNG)


### Notebook:
-Under the "Create" button, there is a text box where you should enter the name of the new notebook.                                  <br/>
-When you delete a notebook, you must select it from the list.                                                                        <br/>
-THERE IS NO AUTOMATIC SAVE, whenever you want to change your book, save it first.                                                    <br/>
![Notebook](IMAGES/Version%202.1.0.0/Notebook.PNG)


### Database-Tables:
![Database-Tables](IMAGES/Version%202.2.0.0/DB-Tables.PNG)


### Database-Notebooks:
-The "NULL" means that the notebook is empty                                                                                          <br/>
![Database-Notebooks](IMAGES/Version%201.1.0.0/DB-Table-Notebooks.PNG)


### Database-Setting:
![Database-Setting](IMAGES/Version%201.1.0.0/DB-Table-Setting.PNG)


### Database-Salt:
![Database-Setting](IMAGES/Version%201.0.0.0/DB-Table-Salt.PNG)


### Database-User:
![Database-User](IMAGES/Version%201.0.0.0/DB-Table-User.PNG)
