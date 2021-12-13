using System.Collections.Generic;
using System.Linq;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.Client.Common
{
    /// <summary>
    /// Обработчика данных о ПО: сравнение и получение обновленного, удаленного и измененного ПО
    /// </summary>
    public class SoftwareProcessor : ISoftwareProcessor
    {
        #region ISoftwareProcessor Members

        public IList<SoftwareStatusDto> GetModifiedSoftwareInfos(IList<SoftwareDto> storedSoftwareInfos,
                                                                 IList<SoftwareDto> currentSoftwareInfos)
        {
            List<SoftwareStatusDto> result = (from currentSoftwareInfo in currentSoftwareInfos
                                              where !storedSoftwareInfos.Any(currentSoftwareInfo.Equals)
                                              select
                                                  new SoftwareStatusDto
                                                      {Software = currentSoftwareInfo, Status = SoftwareStatus.Added}).
                ToList();
            result.AddRange(from storedSoftwareInfo in storedSoftwareInfos
                            where !currentSoftwareInfos.Any(storedSoftwareInfo.Equals)
                            select
                                new SoftwareStatusDto {Software = storedSoftwareInfo, Status = SoftwareStatus.Removed});

            return result;
        }

        #endregion
    }
}