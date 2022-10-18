# MVCIdentityBookRecords

Inpdependent project for Developer bootcamp 2022. This Version Uses C# AspNet MVC and Identity Framework. The basic web pages uses Cookie based authentication
and API uses Jwt-token based authentication.
## Github Repository
<a href="https://github.com/Lepito88/MVCIdentityBookRecords" >MVCIdentityBookRecords</a>
# ER-Model

<img src="./ER-Model.png" alt="ER-model"/>

# Configuration
One needs to create user-secrets file to add necessary config strings.
# Endpoints

Application API Controllers uses following Base_url: https://localhost:7041/api <br>
Endpoints are listed as following format: Method: URL
 ## User
 - GET: /users
   - Fetch all users from database
 - GET: /users/{id}
   - Fetch specific user from database
 - POST: /users
   - Create new user
     - Parameters for POST:

 - PUT: /users/{id}
   - Update specific user who has id n.
     - Parameters for PUT:
 - DELETE: /users/{id}
   - Delete user from database.
 ## Categories
  - GET: /categories
   - Fetch all users from database
 - GET: /categories/{id}
   - Fetch specific categories from database
 - POST: /categories
   - Create new category
     - Parameters for POST:

 - PUT: /categories/{id}
   - Update specific category who has id n.
     - Parameters for PUT:
 - DELETE: /categories/{id}
   - Delete category from database.
 ## Books
  - GET: /books
   - Fetch all users from database
 - GET: /books/{id}
   - Fetch specific book from database
 - POST: /books
   - Create new book
     - Parameters for POST:

 - PUT: /books/{id}
   - Update specific book who has id n.
     - Parameters for PUT:
 - DELETE: /books/{id}
   - Delete book from database.

 ## Author
 - GET: /author
   - Fetch all users from database
 - GET: /author/{id}
   - Fetch specific author from database
 - POST: /author
   - Create new author
     - Parameters for POST:

 - PUT: /author/{id}
   - Update specific author who has id n.
     - Parameters for PUT:
 - DELETE: /author/{id}
   - Delete author from database.



  ## ERRORCODES USED IN RESPONSES
  - L01 : Username or Email not found
  - L02 : Invalid Password
  - L03 : Unable to logout user

  - R01 : User already exists with same Email or Username
  - R02 : Password and confirm password do not match
  - R03 : Password is weak
  - R04 : Unable to save user