using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Telstra.Twins.Responses
{
    public partial class TwinsResponse : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context) =>
            await new ObjectResult(this) { StatusCode = (int)this.Status }
                .ExecuteResultAsync(context);
    }
    public partial class TwinsResponse<T> : IActionResult
    {
        public new async Task ExecuteResultAsync(ActionContext context) =>
            await new ObjectResult(this.Status == System.Net.HttpStatusCode.OK ? (object)this.Data : this.Exception.error) { StatusCode = (int)this.Status }
                .ExecuteResultAsync(context);
    }
}