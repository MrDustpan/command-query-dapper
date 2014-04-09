using System.Data;

namespace CommandQuery
{
    public interface ICommand
    {
        void Execute(IDbConnection db);
    }

    public interface IQuery<T>
    {
        T Execute(IDbConnection db);
    }

    public interface IDatabase
    {
        T Execute<T>(IQuery<T> query);
        void Execute(ICommand command);
    }

    public class SqlDatabase : IDatabase
    {
        private readonly string connectionString;

        public SqlDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public T Execute<T>(IQuery<T> query)
        {
            using (var db = GetConnection())
            {
                return query.Execute(db);
            }
        }

        public void Execute(ICommand command)
        {
            using (var db = GetConnection())
            {
                command.Execute(db);
            }
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}