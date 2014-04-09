##Command/Query Objects with Dapper

A simple framework for using command/query objects to perform data access.  The only dependency is on the `System.Data.IDbConnection` interface, so it works well with dapper-dot-net.

####Sample Usage

```c#
public class FetchAllUsersQuery : IQuery<IEnumerable<User>>
{
    public IEnumerable<User> Execute(IDbConnection db)
    {
        return db.Query<User>("select * from [User]");
    }
}

public class FetchUserByEmailQuery : IQuery<User>
{
    private readonly string email;

    public FetchUserByEmailQuery(string email)
    {
        this.email = email;
    }

    public User Execute(IDbConnection db)
    {
        return db.Query<User>("select * from [User] where [Email] = @email", new { email }).FirstOrDefault();
    }
}

public class UserController
{
    private readonly IDatabase db;

    public UserController(IDatabase db)
    {
        this.db = db;
    }

    public void Create(string email)
    {
        // See if the user exists
        var user = db.Execute(new FetchUserByEmailQuery(email));

        if (user == null)
        {
            // If not, create it
            user = new User { Email = email };
            db.Execute(new CreateUserCommand(user));
        }
    }
}
```