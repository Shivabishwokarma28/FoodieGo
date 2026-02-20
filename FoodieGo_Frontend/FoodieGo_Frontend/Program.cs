var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
// This registers the standard MVC services (Controllers and Razor Views).
builder.Services.AddControllersWithViews();

// 2. Configure HttpClient for API Communication
// This sets up a "Named Client" so your controllers can easily call the Backend.
builder.Services.AddHttpClient("FoodieGo_Backend", client =>
{
    // The BaseAddress is the starting URL for all API calls.
    // Ensure the Port (7043) matches exactly what your Backend project is running on.
    client.BaseAddress = new Uri("https://localhost:7043/api/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // SSL Bypass: In development, your computer might not trust the local SSL certificate.
    // This line prevents "SSL Connection" errors by forcing the app to connect anyway.
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

// 3. Configure Session Management
// These services allow you to store user data (like JWTokens and UserNames) across pages.
builder.Services.AddDistributedMemoryCache(); // Stores session data in the server's RAM.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Log user out after 30 mins of inactivity.
    options.Cookie.HttpOnly = true;   // Security: Prevents JavaScript from reading the session cookie.
    options.Cookie.IsEssential = true; // Ensures session works even if the user hasn't accepted cookies.
});


var app = builder.Build();

// --- HTTP Request Pipeline (Middleware) ---
// This is the order in which a web request travels through your app.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS tells browsers to only use HTTPS for a more secure connection.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirects http:// requests to https://
app.UseStaticFiles();      // Allows the app to serve CSS, Images, and JavaScript.

app.UseRouting();          // Matches the URL to a specific Controller and Action.

// IMPORTANT: UseSession MUST come after UseRouting and before UseAuthorization/Endpoints.
app.UseSession();

app.UseAuthorization();    // Checks if a user has permission to access a page.

// 4. Default Route Mapping
// If no URL is provided (e.g., localhost:7000), it defaults to HomeController -> Index.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();