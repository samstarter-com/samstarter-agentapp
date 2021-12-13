using System.Collections.Generic;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.Client.Common
{
    /// <summary>
    /// ��������� ����������� ������ � ��: ��������� � ��������� ������������, ���������� � ����������� ��
    /// </summary>
    public interface ISoftwareProcessor
    {
        IList<SoftwareStatusDto> GetModifiedSoftwareInfos
            (IList<SoftwareDto> storedSoftwareInfos, IList<SoftwareDto> currentSoftwareInfos);
    }
}