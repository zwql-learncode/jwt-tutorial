# JWT Project

This is my excercise to learn JWT

## 🚀 Quick start

1.  **Step 1.**
    Clone the project
    ```sh
    git clone https://github.com/zhengwuqingling28/jwt-tutorial.git
    ```
1.  **Step 2.**
    change connection string in Server/appsettings.json
    ```sh
     "ConnectionStrings": {
    "DefaultConnection": "server=localhost\\sqlexpress;database=jwt_tutorial;trusted_connection=true"
    },
    ```
 1. **Step 3.**
    import database
    ```sh
    add-migration InitialDatabase
    ```
    ```sh
    update-database
    ```
1.  **Step 4.**
    Run project
