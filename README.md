# MoneyTransfer-API
This is my WEB API project (on .Net 5) where users can register, login and make money transfers to other user accounts.
## Details
After registration and logging into the system, the User is assigned a validity token, 
which allows him to perform the following operations: view his balance and transfer money to another User's account.
And the administrator who is in the database (like Admin123) can block the user, after which the user is prohibited from transferring money to another user's account. 
Also the admin has the right to unblock the blocked user.  Also has the right to get complete information about all users or only through userId. It can also output a list of users sorted by Balance.

### Register Form:
/api/User/Register
{
"firstName" : "string",
"lastName" : "string",
"age" : "int",
"userName" : "string",
"password" : "string",
}

### Login Form:
/api/User/Login
{
  "username": "string",
  "password": "string"
}

### Admin:
/api/Admin/GetAllUsers   (Get all Users, what are registered in the system)
/api/Admin/SortUsersByBalance?balance={?}   (Get users by "balance")
/api/Admin/BlockUser?userId={id}            (Block user by "userId")
/api/Admin/UnblockUser?userId={id}          (Unblock user by "userId")
/api/Admin/GetById?userId={id}              (Get full information about User by "userId")

### User:
/api/User/GetOwnBalance                     (Book Own balance)
/api/User/MoneyTransfer:                    (money transfer from one user to anothers):
{
  "receiverId": 0,   <=(Enter the id of the user who you want to receive money from your account)
  "amount": 0        <=(Enter the amount of money to be transferred)
}

## What I have made:
I created models: User, TransferModel. I created 2 Role-Based authentication, I used: Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.AspNetCore.Identity.EntityFrameworkCore. 
I created 2 roles: Admin and User. I made validations for new User registration. I used FluentValidator. I created the TokenGenerator service, 
which after logging into the system generates a token that can be used to perform specific operations. I conventionally chose 365 days as the token validation time. 
I added a log model to the project, which means that when a User logs into the system, this information is stored in the database, which User entered, what role this User has, 
and the date when he logged into the system. I wrote HashSettings, the function of which is to pass the fixed password in a hashed state to SQL, for this I used TweetinviAPI.
I performed migrations, connected my project to the database. For this I used: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, Microsoft.EntityFrameworkCore.
I tested the project with Swagger and Postman and it works fine, returning all the status codes I expected.
I wrote tests in the Nunit project, where I created FakeServices and tested one by one the servers I have in the project: TokenGenerator, UserService, AdminService.
Everything works perfectly.
