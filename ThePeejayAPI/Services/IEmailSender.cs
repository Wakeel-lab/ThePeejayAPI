using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.DTOs;

namespace ThePeejayAPI.Services
{
    public interface IEmailSender
    {
        Task SendEmail(string sourceAddress, string destinationAddress, string subject, string message);
    }
}
