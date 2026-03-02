using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermus.Api
{
    public sealed class SmtpSettings
    {
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string User { get; set; } = "";
    public string Pass { get; set; } = ""; // yo la cargo desde User Secrets / env var

    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
    }
}