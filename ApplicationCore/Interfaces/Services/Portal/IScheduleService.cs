using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services.Portal
{
    public interface IScheduleService
    {
        Task<PendingSpeakerDTO> GetNewPendingSpeakerAsync(int id);
    }
}
