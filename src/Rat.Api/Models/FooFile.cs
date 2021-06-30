using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Rat.Api.Models
{
    public class FooFile : Foo
    {
        /// <summary>
        /// Gets or set the file content.
        /// </summary>
        public IFormFile File { get; set; }
    }
}