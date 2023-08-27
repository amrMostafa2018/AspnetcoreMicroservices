using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    //Interfaces in Persistence that Not Belong To Ordering
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
