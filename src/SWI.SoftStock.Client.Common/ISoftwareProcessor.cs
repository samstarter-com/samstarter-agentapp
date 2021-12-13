using System.Collections.Generic;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.Client.Common
{
    /// <summary>
    /// Интерфейс обработчика данных о ПО: сравнение и получение обновленного, удаленного и измененного ПО
    /// </summary>
    public interface ISoftwareProcessor
    {
        IList<SoftwareStatusDto> GetModifiedSoftwareInfos
            (IList<SoftwareDto> storedSoftwareInfos, IList<SoftwareDto> currentSoftwareInfos);
    }
}