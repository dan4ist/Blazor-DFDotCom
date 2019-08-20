using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorAuth.Models;
using BlazorAuth.Repositories;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlazorAuth.Services {
	public class BlogService {

		public IBlogRepository _blogRepo;
		public BlogService(IBlogRepository blogRepo) {
			_blogRepo = blogRepo;
		}

		public async Task<IList<Blog>> GetBlogsByUserID(string id) {
			return await _blogRepo.GetBlogsByUsername(id);
		}

		public async Task<IList<Blog>> GetAllBlogs() {
			return await _blogRepo.GetAllBlogs();
		}

		public async Task<Blog> CreateBlog(Blog b) {
			if (b.ID == 0) {
				b.Created = DateTime.UtcNow;
				return await _blogRepo.CreateBlog(b);
			} else {
				b.Updated = DateTime.UtcNow;
				return await _blogRepo.UpdateBlog(b);
			}
		}

		public async Task<bool> DeleteBlog(int id) {
			return await _blogRepo.DeleteBlog(id);
		}

		#region emailtest
		public async Task<bool> SendEmailWrapper() {
			var msg = CreateTestEmail();
			return await SendEmail(msg);
		}

		public SendGridMessage CreateTestEmail() {
			var msg = new SendGridMessage();

			msg.SetFrom(new EmailAddress("sendgridtest@spam.org", "SendGridTestEmailer"));

			var recipients = new List<EmailAddress> {
				new EmailAddress("dforrest@air.org", "Dan Forrest")//,
				//new EmailAddress("gbinner@air.org", "Gary Binner")
			};

			msg.AddTos(recipients);
			msg.SetSubject("Testing SendGrid Email");

			msg.AddContent(MimeType.Html, "<strong>Test Email in HTML</strong>");

			return msg;
		}

		public async Task<bool> SendEmail(SendGridMessage msg) {
			var apiKey = "[API KEY]";
			var client = new SendGridClient(apiKey);

			Response response = await client.SendEmailAsync(msg);

			if (response.StatusCode == System.Net.HttpStatusCode.Accepted) {
				return true;
			} else {
				return false;
			}
		}
		#endregion
	}
}
