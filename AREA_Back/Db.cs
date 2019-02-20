using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System.Threading.Tasks;

namespace AREA_Back
{
    public class Db
    {
        public Db()
        {
            R = RethinkDB.R;
        }

        public async Task InitAsync(string dbName = "AREA")
        {
            this.dbName = dbName;
            conn = await R.Connection().ConnectAsync();
            if (!await R.DbList().Contains(dbName).RunAsync<bool>(conn))
                R.DbCreate(dbName).Run(conn);
            if (!await R.Db(dbName).TableList().Contains("Users").RunAsync<bool>(conn))
                R.Db(dbName).TableCreate("Users").Run(conn);
        }

        public async Task<bool> AddUserAsync(string username, string mail, string password)
        {
            if (await R.Db(dbName).Table("Users").GetAll(mail).Count().Eq(1).RunAsync<bool>(conn))
                return (false);
            await R.Db(dbName).Table("Guilds").Insert(R.HashMap("mail", mail)
                    .With("Username", username)
                    .With("Password", password)
                    ).RunAsync(conn);
            return (true);
        }

        private RethinkDB R;
        private Connection conn;

        private string dbName;
    }
}
