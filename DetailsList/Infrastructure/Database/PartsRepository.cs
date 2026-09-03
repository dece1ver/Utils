using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DetailsList.Infrastructure.Database
{
    /// <summary>
    /// Читает данные о серийных деталях из БД stanki.
    /// </summary>
    public static class PartsRepository
    {
        /// <summary>
        /// Возвращает список обозначений серийных деталей из cnc_serial_parts.
        /// Обозначение ищется по вхождению в PartName/подпись детали.
        /// </summary>
        public static List<string> GetSerialPartDesignations(string connectionString)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(connectionString))
                return result;

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                "SELECT PartName FROM cnc_serial_parts", connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    var partName = reader.GetString(0);
                    if (!string.IsNullOrWhiteSpace(partName))
                        result.Add(partName);
                }
            }
            return result;
        }
    }
}
