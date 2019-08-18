/*
 * 作者:黄平
 * 创建时间:2014-10-24
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 黄平        2014-10-24      创建
 * 赵聪        2015-08-17      添加了BulkLoad数据的方法
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using RuanYun.Logger;

namespace Common
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;


    /// <summary>
    /// 数据库访问层需要使用的基类，主要用来生成数据库连接环境
    /// </summary>
    public class DataAccessBase
    {
        private const int TimeOut = 600;
        public static IDbContext CurrentConnectStringContext;

        /// <summary>
        /// 默认的构造函数使用Config库
        /// </summary>
        public DataAccessBase()
        {
            CurrentConnectStringContext = CreateMainContext();
            CurrentConnectStringContext.CommandTimeout(TimeOut);
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        private IDbContext CreateMainContext()
        {
            return new DbContext().ConnectionStringName("ConfigDbPrefix", new MySqlProvider());
        }

        /// <summary>
        ///  直接Bulk Load数据
        ///  只用于文件处理服务器的数据导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int BulkLoadData(string fileName, string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(CurrentConnectStringContext.Data.ConnectionString))
            {
               
                MySqlBulkLoader bl = new MySqlBulkLoader(conn);
                bl.TableName = tableName;
                bl.FileName = fileName;
                bl.LineTerminator = "\r\n";
                bl.FieldTerminator = ",";
                bl.FieldQuotationCharacter = '"';
                bl.EscapeCharacter = '"';
                bl.NumberOfLinesToSkip = 0;
                conn.Open();

                // Upload data from file
                int count = bl.Load();
                return count;
            }
        }

        /// <summary>
        ///  批量插入数据
        ///  
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnsName"></param>
        /// <returns></returns>
        public static int BulkInsert(DataTable table, List<string> columnsName = null)
        {
            if (string.IsNullOrEmpty(table.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            using (MySqlConnection conn = new MySqlConnection(CurrentConnectStringContext.Data.ConnectionString))
            {

                var bulk = new MySqlBulkLoader(conn)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = tmpPath,
                    NumberOfLinesToSkip = 0,
                    TableName = table.TableName,
                };
                if (columnsName != null && columnsName.Any())
                {
                    bulk.Columns.Clear();
                    columnsName.ForEach(x => bulk.Columns.Add(x));
                }
                conn.Open();
                int count = bulk.Load();
                DeleteTempFile(tmpPath);
                return count;
            }
        }

        /// <summary>
        ///  批量更新数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnsName"></param>
        /// <returns></returns>
        public static int BulkUpdate(DataTable table, List<string> columnsName = null)
        {
            if (string.IsNullOrEmpty(table.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            using (MySqlConnection conn = new MySqlConnection(CurrentConnectStringContext.Data.ConnectionString))
            {
                var bulk = new MySqlBulkLoader(conn)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = tmpPath,
                    NumberOfLinesToSkip = 0,
                    TableName = table.TableName,
                    ConflictOption = MySqlBulkLoaderConflictOption.Replace
                };
                if (columnsName != null && columnsName.Any())
                {
                    bulk.Columns.Clear();
                    columnsName.ForEach(x => bulk.Columns.Add(x));
                }
                conn.Open();
                int count = bulk.Load();
                DeleteTempFile(tmpPath);
                return count;
            }
        }

        ///<summary> 
        ///将DataTable转换为标准的CSV  
        /// </summary>  
        /// <param name="table">数据表</param>  
        /// <returns>返回标准的CSV</returns>  
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn column;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    column = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (column.DataType == typeof(string) && row[column].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[column].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[column].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <param name="columnsName"> </param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> list, List<string> columnsName = null)
        {

            if (list == null || list.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            var entityType = list[0].GetType();
            var entityProperties = entityType.GetProperties();
            if (columnsName == null || columnsName.Count < 1)
            {
                columnsName = new List<string>();
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    columnsName.Add(entityProperties[i].Name);
                }
            }
            var dt = new DataTable();
            foreach (PropertyInfo t in entityProperties)
            {
                if (columnsName.Contains(t.Name))
                {
                    dt.Columns.Add(t.Name);
                }
            }

            foreach (object entity in list)
            {
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                var entityValues = new object[columnsName.Count];
                var m = 0;
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    if (columnsName.Contains(entityProperties[i].Name))
                    {
                        entityValues[m] = entityProperties[i].GetValue(entity, null);
                        m++;
                    }
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        /// 删除临时文件
        /// </summary>
        /// <param name="filePath"></param>
        private static void DeleteTempFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Write("批量插入后删除临时文件出错", MessageType.Warn, typeof(DataAccessBase), ex);
            }

        }
    }
}
