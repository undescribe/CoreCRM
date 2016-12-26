﻿using CoreCRM.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;


namespace CoreCRM.IntegrationTest
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
            
        }

        protected override void ConfigureDbContext(IServiceCollection services)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            services
              .AddEntityFrameworkSqlite()
              .AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite(connection)
              );
        }

        // method in TestStartup.cs
        protected override void EnsureDatabaseCreated(IApplicationBuilder app, ApplicationDbContext dbContext)
        {
            dbContext.Database.OpenConnection(); // see Resource #2 link why we do this

            // now run the real thing
            base.EnsureDatabaseCreated(app, dbContext);

            // Create table not in migrations
            dbContext.Database.EnsureCreated();

			DatabaseFactory(app, dbContext);
		}

        protected virtual void DatabaseFactory(IApplicationBuilder app, ApplicationDbContext dbContext)
        {
            // Fill the database in concrete class.
        }
    }
}