using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using BlazorAuth.Models;

namespace BlazorAuth.Repositories {
	public interface IBlogRepository {
		Task<IList<Blog>> GetBlogsByUsername(string id);
		Task<IList<Blog>> GetAllBlogs();

		Task<Blog> CreateBlog(Blog b);
		Task<Blog> UpdateBlog(Blog b);
		Task<bool> DeleteBlog(int id);
	}

	public class BlogRepository : IBlogRepository {
		private readonly IConfiguration _config;
		public BlogRepository(IConfiguration config) {
			_config = config;
		}
		public IDbConnection Connection {
			get {
				return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			}
		}

		public async Task<IList<Blog>> GetBlogsByUsername(string username) {
			using (IDbConnection conn = Connection) {
				string sQuery = "SELECT id, username, title, content, created, updated FROM blog WHERE Username = @Username";
				conn.Open();
				var result = await conn.QueryAsync<Blog>(sQuery, new { Username = username });
				return result.ToList();
			}
		}

		public async Task<IList<Blog>> GetAllBlogs() {
			using (IDbConnection conn = Connection) {
				string sQuery = "SELECT id, username, title, content, created, updated FROM blog WHERE Username = 'daniel.forrest@live.com'";
				//string sQuery = "SELECT id, username, title, content, created, updated FROM blog";
				conn.Open();
				var result = await conn.QueryAsync<Blog>(sQuery);
				return result.ToList();
			}
		}

		public async Task<Blog> CreateBlog(Blog b) {
			using (IDbConnection conn = Connection) {
				string sQuery = @"INSERT INTO [blog] (username, title, content, created)
								VALUES (@Username, @Title, @Content, @Created); 
								SELECT CAST (SCOPE_IDENTITY() as int);";
				conn.Open();
				var result = await conn.QueryAsync<int>(sQuery, new { Username = b.Username, Title = b.Title, Content = b.Content, Created = b.Created });
				b.ID = result.FirstOrDefault();
				return b;
			}
		}

		public async Task<Blog> UpdateBlog(Blog b) {
			using (IDbConnection conn = Connection) {
				string sQuery = @"UPDATE [blog] SET 
								username = @Username, title = @Title, content = @Content, updated = @Updated
								WHERE id = @ID";
				conn.Open();
				var result = await conn.QueryAsync<int>(sQuery, new { ID = b.ID, Username = b.Username, Title = b.Title, Content = b.Content, Updated = b.Updated });
				return b;
			}
		}

		public async Task<bool> DeleteBlog(int id) {
			using (IDbConnection conn = Connection) {
				string sQuery = "DELETE FROM blog WHERE ID = @id";
				conn.Open();
				var result = await conn.ExecuteAsync(sQuery, new { ID = id });
				if (result == 1) {
					return true;
				}
				return false;
			}
		}
	}
}
