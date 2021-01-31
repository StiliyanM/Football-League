namespace FootballLeague.Infrastructure.Middleware
{
    using FootballLeague.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(
            RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = new MediaTypeHeaderValue(Constants.APPLICATION_JSON).ToString();

            try
            {
               await this.next(context);
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(this.CreateResponseContent(ex.Message), Encoding.UTF8);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(this.CreateResponseContent(ex.Message), Encoding.UTF8);
            }
            catch (DbUpdateException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(this.CreateResponseContent(Constants.DATABASE_ERROR));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(this.CreateResponseContent(Constants.SERVER_ERROR));
            }
        }

        private string CreateResponseContent(string message)
        {
            return "{\"messages\":[\"" + message + "\"]}";
        }
    }

}
