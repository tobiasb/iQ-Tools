using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace APILogSearch
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <executable> <from UTC> <to UTC>");
                Console.WriteLine("Example: <executable> \"2017/05/05 07:03:58Z\" \"2017/05/05 07:05:55Z\"");
                return;
            }

            var targetDateStart = ParseDateTime(args[0]);
            var targetDateEnd = ParseDateTime(args[1]);

            if (targetDateStart == null || targetDateEnd == null)
            {
                return;
            }

            var startAndEnd = FindStartAndEndRecord(targetDateStart.Value, targetDateEnd.Value);

            var command = $"select * from ApiUsageDataRecords where rowid between {startAndEnd[0].RowId} and {startAndEnd[1].RowId} ;";
            Clipboard.SetText(command);
            Console.WriteLine($"[{command}] copied to clipboard");
        }

        private static DateTime? ParseDateTime(string input)
        {
            DateTimeOffset result;

            if (!DateTimeOffset.TryParse(input, out result))
            {
                Console.WriteLine($"Not a valid date string: {input}. Example: 2017/05/05 07:05:58Z");
                return null;
            }

            return result.UtcDateTime;
        }

        private static long GetInt64FromQuery(SqlConnection conn, string query)
        {
            return (long)new SqlCommand(query, conn).ExecuteScalar();
        }

        private static ApiUsageDataRecord[] FindStartAndEndRecord(DateTime targetStartDate, DateTime targetEndDate)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApiManagement"].ConnectionString))
            {
                conn.Open();
                
                var minRowId = GetInt64FromQuery(conn, "select min(rowid) from ApiUsageDataRecords;");
                var maxRowId = GetInt64FromQuery(conn, "select max(rowid) from ApiUsageDataRecords;");

                var startRow = FindClosest(conn, targetStartDate, minRowId, maxRowId);
                var endRow = FindClosest(conn, targetEndDate, startRow.RowId, maxRowId);

                return new[] {startRow, endRow};
            }
        }

        private static ApiUsageDataRecord FindClosest(SqlConnection conn, DateTime targetDateTime, long start, long end)
        {
            var pivot = start + (end - start) / 2;

            var apiUsageDataRecord = GetApiUsageDataRecord(conn, pivot);

            if (Math.Abs((apiUsageDataRecord.DateInserted - targetDateTime).TotalSeconds) <= 5)
            {
                return apiUsageDataRecord;
            }

            if (apiUsageDataRecord.DateInserted < targetDateTime)
            {
                return FindClosest(conn, targetDateTime, pivot, end);
            }
            else
            {
                return FindClosest(conn, targetDateTime, start, pivot);
            }
        }

        private static ApiUsageDataRecord GetApiUsageDataRecord(SqlConnection conn, long apiUsageRecordRowId)
        {
            var command = new SqlCommand("select rowid, dateinserted from apiusagedatarecords where rowid = " + apiUsageRecordRowId);
            command.Connection = conn;
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                return ApiUsageDataRecord.From(reader);
            }
        }
    }
}
