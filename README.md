# Habbo v1
fork from prjOwnage for the first version of Habbo.

# HOW-TO: Compile Source:
- Download Visual Studio or Visual C# 2010 Express.
- Import project solution.
- Compile the code.

# HOW-TO: Run the server:
- Import "db.sql" in your MySQL server.
- Create a file "Server.ini" and add this code:

```
[MySQL]
hostname= Your MySQL Hostname
username= Your MySQL Username
password= Your MySQL Password
database= Your MySQL Database
port=3306

[Server]
ip= Your Server IP
port=37120 #You can leave it 37120 or you can change it if you wish.
```
- Go into bin/debug directory and put Server.ini in the same directory of "Habbo v1.exe".
- Execute "Habbo v1.exe".