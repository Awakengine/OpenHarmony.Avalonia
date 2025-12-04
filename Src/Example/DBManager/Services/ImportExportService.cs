using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace DBManager.Services;

public class ImportExportService
{
    public async Task ExportToCsvAsync(DataTable dataTable, string filePath, bool includeHeaders = true)
    {
        var csv = new StringBuilder();
        
        if (includeHeaders)
        {
            var headers = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => EscapeCsvField(column.ColumnName)));
            csv.AppendLine(headers);
        }
        
        foreach (DataRow row in dataTable.Rows)
        {
            var fields = string.Join(",", row.ItemArray.Select(field => EscapeCsvField(field?.ToString() ?? "")));
            csv.AppendLine(fields);
        }
        
        await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
    }
    
    public async Task ExportToJsonAsync(DataTable dataTable, string filePath)
    {
        var data = new List<Dictionary<string, object>>();
        
        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            data.Add(dict);
        }
        
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
    }
    
    private string EscapeCsvField(string field)
    {
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            field = field.Replace("\"", "\"\"");
            field = $"\"{field}\"";
        }
        return field;
    }
    
    public async Task<DataTable> ImportFromCsvAsync(string filePath, bool firstRowHasHeaders = true)
    {
        var lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        var dataTable = new DataTable();
        
        if (lines.Length == 0)
            return dataTable;
            
        // 解析标题行
        var headerLine = lines[0];
        var headers = ParseCsvLine(headerLine);
        
        if (firstRowHasHeaders)
        {
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }
        }
        else
        {
            for (int i = 0; i < headers.Count; i++)
            {
                dataTable.Columns.Add($"Column{i}");
            }
        }
        
        // 解析数据行
        int startIndex = firstRowHasHeaders ? 1 : 0;
        for (int i = startIndex; i < lines.Length; i++)
        {
            var values = ParseCsvLine(lines[i]);
            dataTable.Rows.Add(values.ToArray());
        }
        
        return dataTable;
    }
    
    private List<string> ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var inQuotes = false;
        var field = new StringBuilder();
        
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            
            if (c == '"' && !inQuotes)
            {
                inQuotes = true;
            }
            else if (c == '"' && inQuotes)
            {
                // 检查是否是转义的引号
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    field.Append('"');
                    i++; // 跳过下一个引号
                }
                else
                {
                    inQuotes = false;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(c);
            }
        }
        
        fields.Add(field.ToString()); // 添加最后一个字段
        return fields;
    }
}