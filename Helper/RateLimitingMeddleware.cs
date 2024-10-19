namespace MovieAPI.Helper
{
    public class RateLimitingMiddleware
    {
        private RequestDelegate next;
        public RateLimitingMiddleware(RequestDelegate next)
        {
            this.next=next;
        }
        private static int _counter = 0;
        private static DateTime _lastRequestTime=DateTime.Now;
        public async Task Invoke(HttpContext context)
        {
            _counter++;
            if (DateTime.Now.Subtract(_lastRequestTime).Seconds > 10)
            {
                _counter = 1;
                _lastRequestTime = DateTime.Now;
                await next(context);
                return;
            }
            if (_counter < 100)
            {
                _lastRequestTime=DateTime.Now;
                await next(context);
                return;
            }
            _lastRequestTime = DateTime.Now;
            await context.Response.WriteAsync("Requests Limit exceeded");
        }
    }
}
