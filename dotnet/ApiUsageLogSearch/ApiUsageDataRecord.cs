using System;
using System.Data.SqlClient;

namespace APILogSearch
{
    public class ApiUsageDataRecord
    {
        public long RowId;
        public DateTime DateInserted;

        public static ApiUsageDataRecord From(SqlDataReader reader)
        {
            return new ApiUsageDataRecord
            {
                RowId = reader.GetInt64(0),
                DateInserted = reader.GetDateTime(1),
            };
        }
    }
}
