using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure;
public class KeyVaultOptions
{
    public string KeyVaultURL { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string DirectoryID { get; set; }
}
