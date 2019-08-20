using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Markdig;

namespace BlazorAuth.Models {
	public class Blog {
		public int ID { get; set; }

		[Required]
		public string Username { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Content { get; set; }
		public string HTMLContent {
			get {
				return Markdown.ToHtml(Content, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
			}
		}
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}
