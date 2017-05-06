# ApiUsageLogSearch

Given a start and end `DateTime`, lets you find the `RowId`s that cover that timespan from the `ApiUsageDataRecords`.

Example: `ApiUsageLogSearch.exe "2017/05/05 07:03:00Z" "2017/05/05 07:05:00Z"`

The result is the query to fetch all `ApiUsageDataRecords` within that timespan, printed in the console and copied to the clipboard.