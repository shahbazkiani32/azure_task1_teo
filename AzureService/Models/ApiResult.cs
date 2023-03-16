using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureService.Models
{
    public class ApiLogs : TableEntity
    {
        public bool Success { get; set; }
        public string Error { get; set; }

    }
}
