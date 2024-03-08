using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructer.Class;

public sealed class VendingConnectionString
{
    /// <summary>
    /// The connection strings key.
    /// </summary>
    public const string SettingsKey = "VendingMachineDb";

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorDomainConnectionString"/> class.
    /// </summary>
    /// <param name="value">The connection string value.</param>
    public VendingConnectionString(string value) => Value = value;

    /// <summary>
    /// Gets the connection string value.
    /// </summary>
    public string Value { get; }

    public static implicit operator string(VendingConnectionString connectionString) => connectionString.Value;
}
