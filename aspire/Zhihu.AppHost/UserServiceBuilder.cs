using Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.AppHost;

public static class UserServiceBuilder
{
    private const string MasterDb = "MasterDb";
    private const string SlaveDb = "SlaveDb";

    public static void AddUserService(this IDistributedApplicationBuilder builder,
        IResourceBuilder<MySqlServerResource> mysql)
    {
        var userDb = mysql.AddDatabase("UserDb");

        builder.AddProject<Zhihu_UserService_HttpApi>("UserService-HttpApi")
            .WithReference(userDb, MasterDb)
            .WithReference(userDb, SlaveDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_UserService_MigrationWorker>("UserService-MigrationWorker")
            .WithReference(userDb, MasterDb)
            .WaitFor(mysql);
    }
}