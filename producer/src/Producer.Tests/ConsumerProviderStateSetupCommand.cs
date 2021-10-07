using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Producer.Tests;

public class ConsumerProviderStateSetupCommand
{
    private const string ConsumerName = "Consumer";
    private readonly IDictionary<string, Action> _providerStates;

    public ConsumerProviderStateSetupCommand()
    {
        _providerStates = new Dictionary<string, Action>
        {
            {
                "There is no data",
                RemoveAllData
            },
            {
                "There is data",
                AddData
            }
        };
    }

    public void Execute(ProviderState providerState)
    {
        // A null or empty provider state key must be handled
        if (!string.IsNullOrEmpty(providerState?.State) && providerState.Consumer == ConsumerName)
        {
            _providerStates[providerState.State].Invoke();
        }
    }

    private void RemoveAllData()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../../data");
        var deletePath = Path.Combine(path, "somedata.txt");

        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
    }

    private void AddData()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../../data");
        var writePath = Path.Combine(path, "somedata.txt");

        if (!File.Exists(writePath))
        {
            File.Create(writePath);
        }
    }
}
