using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.API.Interfaces
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
