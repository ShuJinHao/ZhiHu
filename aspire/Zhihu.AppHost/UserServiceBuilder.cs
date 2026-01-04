using CommunityToolkit.Aspire.Hosting.Dapr;
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
        IResourceBuilder<MySqlServerResource> mysql,
         DaprSidecarOptions? daprSidecarOptions = null)
    {
        var db = mysql.AddDatabase("zhihu-user");

        var migration = builder.AddProject<Zhihu_UserService_MigrationWorker>("UserService-MigrationWorker")
            .WithReference(db, MasterDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_UserService_HttpApi>("UserService-HttpApi")
            .WithReference(db, MasterDb)
            .WithReference(db, SlaveDb)
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(mysql)
            .WaitForCompletion(migration);
    }
}