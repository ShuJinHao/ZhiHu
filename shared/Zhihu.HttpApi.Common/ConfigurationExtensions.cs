using Dapr.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.HttpApi.Common;

public static class ConfigurationExtensions
{
    public static void AddDaprConfiguration(this ConfigurationManager configuration, string[]? specKeys = null)
    {
        var client = new DaprClientBuilder().Build();

        var keys = new List<string>
        {
            "appsettings.json"
        };
        if (specKeys is not null) keys.AddRange(specKeys);

        const string daprConfigStore = "redis-config";

        var config = client.GetConfiguration(daprConfigStore, keys).GetAwaiter().GetResult();

        foreach (var item in config.Items)
        {
            configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(item.Value.Value)));
        }
    }
}