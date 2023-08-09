using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Text;

namespace KH.Lab.WebAPI_Dapper_Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        /// <summary>
        /// 檢查有沒有sqlite檔案，沒有就新增，並增加一筆資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("InsertAsync")]
        public async Task<IActionResult> InsertAsync()
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //當找不到sqlite檔案時，建立新表，新表創建後就會產生sqlite檔案了
            //if (System.IO.File.Exists(@".\Student.sqlite"))
            //{
            //    //組語法，新建名為Student的表
            //    SQL.Append("CREATE TABLE Student( \n");
            //    //Id欄位設定數字型別為PKey，並且自動遞增
            //    SQL.Append("Id INTEGER PRIMARY KEY AUTOINCREMENT, \n");
            //    //Name欄位設定為VARCHAR(32)不允許是null
            //    SQL.Append("Name VARCHAR(32) NOT NULL, \n");
            //    //Age欄位設定為int
            //    SQL.Append("Age INTEGER) \n");
            //    //執行sql語法
            //    await conn.ExecuteAsync(SQL.ToString());
            //    //清除字串內的值
            //    SQL.Clear();
            //}
            //組語法
            SQL.Append("INSERT INTO Student (Name, Age) VALUES (@Name, @Age);");
            //建立SQL參數化要使用的變數
            DynamicParameters parameters = new();
            //參數1
            parameters.Add("Name", "BillHuang");
            //參數2
            parameters.Add("Age", 20);
            //執行語法，insert一筆資料到Student
            var Result = await conn.ExecuteAsync(SQL.ToString(), parameters);
            //回傳執行成功的數量
            return Ok(Result);
        }

        /// <summary>
        /// 取得Student所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("NoCacheSelectAsync")]
        public async Task<IActionResult> NoCacheSelectAsync()
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //組語法
            SQL.Append("select * from Student");
            //執行，並且將執行結果存為強型別
            var Result = await conn.QueryAsync<Student>(SQL.ToString());
            //回傳結果
            return Ok(Result);
        }

        /// <summary>
        /// 取得Student所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("AnySelectAsync")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> AnySelectAsync()
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //組語法
            SQL.Append("select * from Student");
            //執行，並且將執行結果存為強型別
            var Result = await conn.QueryAsync<Student>(SQL.ToString());
            //回傳結果
            return Ok(Result);
        }
        
        /// <summary>
        /// 取得Student資料
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("AnySelectByIdAsync")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "id" })]
        public async Task<IActionResult> AnySelectByIdAsync(int id)
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //組語法
            SQL.Append("select * from Student where Id = @id");
            //執行，並且將執行結果存為強型別
            var Result = await conn.QueryAsync<Student>(SQL.ToString(), new { id });
            //回傳結果
            return Ok(Result);
        }

        /// <summary>
        /// 取得Student資料
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("UserAgentSelectByIdAsync")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent", VaryByQueryKeys = new string[] { "id" })]
        public async Task<IActionResult> UserAgentSelectByIdAsync(int id)
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //組語法
            SQL.Append("select * from Student where Id = @id");
            //執行，並且將執行結果存為強型別
            var Result = await conn.QueryAsync<Student>(SQL.ToString(), new { id });
            //回傳結果
            return Ok(Result);
        }


        /// <summary>
        /// 取得Student所有資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("DapperCacheSelectAsync")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> DapperCacheSelectAsync()
        {
            //連接sqlite資料庫
            using var conn = new SqliteConnection("Data Source=Student.sqlite");
            var SQL = new StringBuilder();
            //組語法
            SQL.Append("select * from Student");            
            //執行，並且將執行結果存為強型別
            var Result = await conn.QueryAsync<Student>(sql: SQL.ToString(), commandType: System.Data.CommandType.Text);


            //回傳結果
            return Ok(Result);
        }


        /// <summary>
        /// 學生
        /// </summary>
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; } = "BillHuang";
            public int Age { get; set; }
        }
    }
}
