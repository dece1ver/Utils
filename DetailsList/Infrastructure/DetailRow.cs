using System;

namespace DetailsList.Infrastructure
{
    /// <summary>
    /// Строка результата поиска: одна строка соответствует одному файлу УП.
    /// </summary>
    public class DetailRow
    {
        public string Name { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string ProgramFile { get; set; } = string.Empty;
        public DateTime Modified { get; set; }
        public bool IsSerial { get; set; }
    }
}
